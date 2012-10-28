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
            ParseData((JObject)source["data"]);
        }

        public bool HasMore { get { return _nextId != null; } }

        private string _nextId;
        private List<T> _items;
        private bool _ignoreParent;

        public async Task<int> More()
        {
            if (_nextId == null)
            {
                return 0;
            }
            return await RetrieveData(this.Resource + "?after=" + _nextId);
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
                JObject respObj = typeof(T) == typeof(Comment) ? 
                    (JObject)JArray.Parse(resp.Content)[1] : JObject.Parse(resp.Content);
                this.LoadThingData(respObj);
                JObject thingArray = (JObject)respObj["data"];
                return ParseData(thingArray);
            }
            catch (JsonException ex)
            {
                Helpers.DebugWrite(ex.Message);
                throw new FactoryException(FactoryExceptionType.Parse);
            }
        }

        private int ParseData(JObject source)
        {
            JArray children = (JArray)source["children"];

            List<T> _tempList = new List<T>();

            foreach (var child in children)
            {
                // Instances must implement a JObject constructor. Scary.
                _tempList.Add((T)Activator.CreateInstance(typeof(T), new object[] { child, _reqServ }));
            }

            _nextId = (string)source["after"];
            _items.AddRange(_tempList);
            return _tempList.Count;
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
    }
}
