using RedditStoreApp.Data.Core;
using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.Data
{
    class RootGenerator
    {
        public enum SortType { Hot, New, Controversial, Top };

        private RequestService _reqServ;

        public RootGenerator()
        {
            _reqServ = new RequestService();
        }

        public async Task<bool> Login(string userName, string password)
        {
            var keyList = new List<KeyValuePair<string, string>>();
            keyList.Add(new KeyValuePair<string,string>("user", userName));
            keyList.Add(new KeyValuePair<string,string>("passwd", password));

            Response resp = await _reqServ.PostAsync("api/login", keyList);

            System.Diagnostics.Debugger.Break();

            return !resp.Content.Contains("invalid password");
        }
    }
}
