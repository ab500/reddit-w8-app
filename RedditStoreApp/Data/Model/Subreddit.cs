using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.Data.Model
{
    class Subreddit
    {
        public string DisplayName { get; private set; }
        public string Title { get; private set; }
        public DateTime Created { get; private set; }
        public bool Over18 { get; private set; }
        public int Subscribers { get; private set; }
        public string Description { get; private set; }
        public string Id { get; private set; }
        public string Type { get; private set; }

        public Subreddit(JObject jobj)
        {
            var data = (JObject)jobj["data"];
            this.DisplayName = data["display_name"].Value<string>();
            this.Title = data["title"].Value<string>();
            this.Created = Helpers.FromUnixTime(data["created_utc"].Value<long>());
            this.Over18 = data["over18"].Value<string>() == "true" ? true : false;
            this.Subscribers = data["subscribers"].Value<int>();
            this.Description = data["public_description"].Value<string>();
            this.Id = data["id"].Value<string>();
            this.Type = jobj["kind"].Value<string>();
        }

        public override string ToString()
        {
            return Title + "- " + Description.Substring(0, 50);
        }
    }
}
