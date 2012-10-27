using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditStoreApp.Data.Core;

namespace RedditStoreApp.Data.Model
{
    class Listing<T>
    {
        public Listing(string resource, RequestService reqServ) 
        {
            _resource = resource;
            _reqServ = reqServ;
            _hasMore = false;
            _items = new List<T>();
        }

        public Listing(string resource, JObject source, RequestService reqServ) : this(resource, reqServ)
        {
            ParseData(source);
        }

        public bool HasMore { get { return _hasMore; } }
        public string Resource { get { return _resource; } }

        private bool _hasMore;
        private string _resource;
        private string _nextId;
        private RequestService _reqServ;
        private List<T> _items;

        public async Task<int> More()
        {
            if (!_hasMore)
            {
                return 0;
            }
            return await RetrieveData(_nextId);
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
                throw new GeneratorException(GeneratorExceptionType.Connection);
            }

            try
            {
                JObject respObj = JObject.Parse(resp.Content);
                JObject subredditArray = (JObject)respObj["data"];
                return ParseData(subredditArray);
            }
            catch (JsonException ex)
            {
                throw new GeneratorException(GeneratorExceptionType.Parse);
            }
        }

        private int ParseData(JObject source)
        {
            return 0;
        }
    }
}
