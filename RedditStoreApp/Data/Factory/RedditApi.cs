using RedditStoreApp.Data.Core;
using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RedditStoreApp.Data.Factory
{
    public class RedditApi : IRedditApi
    {
        public enum SortType { Hot, New, Controversial, Top };

        private RequestService _reqServ;
        private bool _isLoggedIn = false;

        public bool IsLoggedIn { get { return _isLoggedIn; } }

        public RedditApi()
        {
            _reqServ = new RequestService();
        }

        public async Task<bool> LoginAsync(string userName, string password)
        {
            var keyList = new List<KeyValuePair<string, string>>();
            keyList.Add(new KeyValuePair<string,string>("user", userName));
            keyList.Add(new KeyValuePair<string,string>("passwd", password));

            Response resp = await _reqServ.PostAsync("api/login", keyList);

            if (!resp.IsSuccess)
            {
                throw new FactoryException(FactoryExceptionType.Connection);
            }

            _isLoggedIn = !resp.Content.Contains("invalid password");
            return _isLoggedIn;
        }

        public async Task<Listing<Subreddit>> GetMySubredditsListAsync()
        {
            return await GetSubredditListAsync("reddits/mine");
        }

        public async Task<Listing<Subreddit>> GetPopularSubredditsListAsync()
        {
            return await GetSubredditListAsync("reddits");
        }

        private async Task<Listing<Subreddit>> GetSubredditListAsync(string resourcePath)
        {
            Listing<Subreddit> _list = new Listing<Subreddit>(resourcePath, _reqServ);
            await _list.Refresh();
            return _list;
        }
    }
}
