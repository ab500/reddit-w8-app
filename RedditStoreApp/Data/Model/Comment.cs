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

        public string Author { get; private set; }
        public string Body { get; private set; }
        public string BodyHtml { get; private set; }
        public string LinkId { get; private set; }

        public int Ups { get; private set; }
        public int Downs { get; private set; }
        public DateTime Created { get; private set; }
        public bool? Likes { get; private set; }

        public Listing<Comment> Replies { get; private set; }

        public Comment(JObject jobj, RequestService reqServ) : base(jobj, reqServ)
        {
            var data = (JObject)jobj["data"];

            this.Author = data["author"].Value<string>();
            this.Body = data["body"].Value<string>();
            this.BodyHtml = data["body"].Value<string>();
            this.LinkId = data["link_id"].Value<string>();

            this.Ups = data["ups"].Value<int>();
            this.Downs = data["downs"].Value<int>();
            this.Created = Helpers.FromUnixTime(data["created"].Value<long>());

            string likeValue = data["likes"].Value<string>();
            if (likeValue == "true")
            {
                this.Likes = true;
            }
            else if (likeValue == "false")
            {
                this.Likes = false;
            }
            else
            {
                this.Likes = null;
            }

            JObject replyObj = data["replies"] as JObject;

            if (replyObj != null)
            {
                this.Replies = new Listing<Comment>(GetRepliesResource(), replyObj, _reqServ, true);   
            }
            else
            {
                this.Replies = new Listing<Comment>("", _reqServ);
            }

            this.Replies.SetLinkId(this.LinkId);
        }

        private string GetRepliesResource()
        {
            return "comments/" + this.LinkId.Split(new char[] { '_' })[1] + "/_/" + this.Id;
        }
    }
}
