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
    class Listing<T> : IReadOnlyList<T> where T: class
    {
        public Listing(string resource, RequestService reqServ) 
        {
            _resource = resource;
            _reqServ = reqServ;
            _nextId = null;
            _items = new List<T>();
        }

        public Listing(string resource, JObject source, RequestService reqServ) : this(resource, reqServ)
        {
            ParseData(source);
        }

        public bool HasMore { get { return _nextId != null; } }
        public string Resource { get { return _resource; } }

        private string _resource;
        private string _nextId;
        private RequestService _reqServ;
        private List<T> _items;
        private string _type;

        public async Task<int> More()
        {
            if (_nextId == null)
            {
                return 0;
            }
            return await RetrieveData(_resource + "?after=" + _nextId);
        }

        public async Task<int> Refresh()
        {
            _items.Clear();
            return await RetrieveData(_resource);
        }

        private async Task<int> RetrieveData(string source)
        {
            Response resp = await _reqServ.GetAsync(source, true);

            if (!resp.IsSuccess)
            {
                throw new FactoryException(FactoryExceptionType.Connection);
            }

            try
            {
                JObject respObj = JObject.Parse(resp.Content);
                JObject subredditArray = (JObject)respObj["data"];
                return ParseData(subredditArray);
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
                // THIS ISN'T COMPILE-TIME CHECKED.
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
