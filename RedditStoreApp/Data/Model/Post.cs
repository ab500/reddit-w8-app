﻿using Newtonsoft.Json.Linq;
using RedditStoreApp.Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.Data.Model
{
    public class Post : Thing
    {

        public string Author { get; private set; }
        public string Domain { get; private set; }
        public bool IsSelf { get; private set; }
        public int CommentCount { get; private set; }
        public bool Over18 { get; private set; }
        public string PermaLink { get; private set; }
        public bool IsHidden { get; private set; }
        public string SelfText { get; private set; }
        public string Subreddit { get; private set; }
        public string ThumbnailUrl { get; private set; }
        public string Title { get; private set; }
        public string Url { get; private set; }

        public int Ups { get; private set; }
        public int Downs { get; private set; }
        public DateTime Created { get; private set; }
        public bool? Likes { get; private set; }

        public Listing<Comment> Comments { get; private set; }

        public Post(JObject jobj, RequestService reqServ) : base(jobj, reqServ)
        {
            var data = (JObject)jobj["data"];

            this.Author = data["author"].Value<string>();
            this.Domain = data["domain"].Value<string>();
            this.IsSelf = data["is_self"].Value<bool>();
            this.CommentCount = data["num_comments"].Value<int>();
            this.Over18 = data["over_18"].Value<bool>();
            this.PermaLink = data["permalink"].Value<string>();
            this.IsHidden = data["hidden"].Value<bool>();
            this.SelfText = data["selftext"].Value<string>();
            this.Subreddit = data["subreddit"].Value<string>();
            this.ThumbnailUrl = data["thumbnail"].Value<string>();
            this.Title = data["title"].Value<string>();
            this.Url = data["url"].Value<string>();

            this.Ups = data["ups"].Value<int>();
            this.Downs = data["downs"].Value<int>();
            this.Created = Helpers.FromUnixTime(data["created_utc"].Value<long>());

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

            this.Comments = new Listing<Comment>("comments/" + this.Id, _reqServ);
        }
    }
}
