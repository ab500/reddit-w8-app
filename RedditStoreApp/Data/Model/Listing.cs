using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditStoreApp.Data.Core;
using RedditStoreApp.Data.Factory;

namespace RedditStoreApp.Data.Model
{
    class Listing<T> : Thing, IReadOnlyList<T> where T: Thing
    {
        public Listing(string resource, RequestService reqServ, bool ignoreParent = false) : base(resource, reqServ)
        {
            _nextId = null;
            _ignoreParent = ignoreParent;
            _items = new List<T>();
        }

        public Listing(string resource, JObject source, RequestService reqServ, bool ignoreParent = false)
            : this(resource, reqServ)
        {
            _ignoreParent = ignoreParent;
            ParseData((JArray)source["data"]["children"]);
        }

        public bool HasMore { get { return _nextId != null; } }

        private string _nextId;

        // Used by the 'morechildren' api on comments.
        private string _childrenString;
        private string _linkId;

        private List<T> _items;
        private bool _ignoreParent;

        public async Task<int> More()
        {
            if (_nextId == null)
            {
                return 0;
            }

            if (typeof(T) == typeof(Comment))
            {
                return await LoadMoreComments();
            }
            else
            {
                return await RetrieveData(this.Resource + "?after=" + _nextId);
            } 
        }

        public async Task<int> Refresh()
        {
            _items.Clear();
            return await RetrieveData(this.Resource);
        }

        public async Task<int> Load()
        {
            return await Refresh();
        }

        private async Task<int> LoadMoreComments()
        {
            List<KeyValuePair<String, String>> postParams = new List<KeyValuePair<String, String>>();
            postParams.Add(new KeyValuePair<string, string>("link_id", _linkId));
            postParams.Add(new KeyValuePair<string, string>("api_type", "json"));
            postParams.Add(new KeyValuePair<string,string>("children", _childrenString));

            Response resp = await _reqServ.PostAsync("api/morechildren", postParams);

            if (!resp.IsSuccess)
            {
                throw new FactoryException(FactoryExceptionType.Connection);
            }

            try
            {
                JObject respObj = JObject.Parse(resp.Content);
                JArray thingArray = (JArray)respObj.SelectToken("json.data.things");
                _nextId = null;
                return ParseData(thingArray);
            }
            catch (JsonException ex)
            {
                Helpers.DebugWrite(ex.Message);
                throw new FactoryException(FactoryExceptionType.Parse);
            }
        }

        private async Task<int> RetrieveData(string source)
        {
            if (source == null || source == "")
            {
                return 0;
            }

            Response resp = await _reqServ.GetAsync(source, true);

            if (!resp.IsSuccess)
            {
                throw new FactoryException(FactoryExceptionType.Connection);
            }

            try
            {
                // Do this-specific parsing of the message before sending it to the
                // generic parsing function.
                JObject respObj = ParseAndFindListing(resp.Content);

                // Load what we can in terms of thing data. For listings
                // this will be admittedly limited, listings don't
                // truely inherient from things.
                LoadThingData(respObj);

                // If it's a comment we grab the link id. When
                // this fails it just returns null.
                _linkId = (string)respObj.SelectToken("data.children[0].data.link_id");
                _nextId = (string)respObj["after"];

                JArray thingArray = (JArray)respObj["data"]["children"];
                return ParseData(thingArray);
            }
            catch (JsonException ex)
            {
                Helpers.DebugWrite(ex.Message);
                throw new FactoryException(FactoryExceptionType.Parse);
            }
        }

        private int ParseData(JArray source)
        {
            // Create a temp list incase this fails. 
            List<T> _temp = new List<T>();

            foreach (var child in source)
            {
                if (child["kind"].Value<string>() != "more")
                {
                    _temp.Add(CreateThing(child));
                }
                else
                {
                    _nextId = (string)child.SelectToken("data.id");
                    BuildChildrenList((JArray)child.SelectToken("data.children"));
                }
            }

            _items.AddRange(_temp);
            return _temp.Count;
        }

        public T this[int index]
        {
            get { return _items[index]; }
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public void SetLinkId(string linkId)
        {
            _linkId = linkId;
        }

        private T CreateThing(JToken child)
        {
            // Instances must implement a JObject constructor, which is
            // enforced by making them inherient type Thing.
            return (T)Activator.CreateInstance(typeof(T), new object[] { child, _reqServ });
        }

        private void BuildChildrenList(JArray childrenArray)
        {
            List<string> moreChildren = new List<string>();

            foreach (JValue moreChild in (JArray)childrenArray)
            {
                moreChildren.Add(moreChild.Value<string>());
            }

            if (moreChildren.Count > 0)
            {
                StringBuilder childrenSb = new StringBuilder();

                childrenSb.Append(moreChildren[0]);
                moreChildren.RemoveAt(0);
                foreach (string childString in moreChildren)
                {
                    childrenSb.Append("," + childString);
                }

                _childrenString = childrenSb.ToString();
            }
        }

        private JObject ParseAndFindListing(string input)
        {
            // If it's a comment we ignore the post details.
            JObject respObj = typeof(T) == typeof(Comment) ?
                (JObject)JArray.Parse(input)[1] : JObject.Parse(input);

            // If it's a nested comment we ignore the parent.
            respObj = _ignoreParent ? (JObject)respObj.SelectToken("data.children[0].data.replies") : respObj;
            return respObj;
        }

    }
}
