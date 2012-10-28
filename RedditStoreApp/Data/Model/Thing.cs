using Newtonsoft.Json.Linq;
using RedditStoreApp.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.Data.Model
{
    class Thing
    {
        public string Name { get; private set; }
        public string Id { get; private set; }
        public string Type { get; private set; }
        public string Resource { get; private set; }

        public bool IsLoaded { get; private set; }

        protected RequestService _reqServ;

        public Thing(JObject jobj, RequestService reqServ)
        {
            _reqServ = reqServ;
            LoadThingData(jobj);
        }

        public Thing(string resource, RequestService reqServ)
        {
            _reqServ = reqServ;
            this.Resource = resource;
        }

        protected void LoadThingData(JObject jobj)
        {
            if (this.IsLoaded)
            {
                return;
            }

            if (jobj["name"] != null)
            {
                this.Name = jobj["name"].Value<string>();
            }
            else if (jobj["data"]["name"] != null)
            {
                this.Name = jobj["data"]["name"].Value<string>();
            }

            if (jobj["id"] != null)
            {
                this.Id = jobj["id"].Value<string>();
            }
            else if (jobj["data"]["id"] != null)
            {
                this.Id = jobj["data"]["id"].Value<string>();
            }

            if (jobj["kind"] != null)
            {
                this.Type = jobj["kind"].Value<string>();
            }

            this.IsLoaded = true;
        }

        public override string ToString()
        {
            return base.ToString() + "- Name: " + this.Name;
        }
    }
}
