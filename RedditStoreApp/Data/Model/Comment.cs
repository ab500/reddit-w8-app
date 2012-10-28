using Newtonsoft.Json.Linq;
using RedditStoreApp.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.Data.Model
{
    class Comment : Thing
    {
        public Comment(JObject jobj, RequestService reqServ) : base(jobj, reqServ)
        {
            System.Diagnostics.Debugger.Break();
        }
    }
}
