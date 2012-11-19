using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.ViewModels
{
    public class PostViewModel
    {
        private Post _post;

        public PostViewModel(Post post)
        {
            _post = post;
        }

        public string Name
        {
            get
            {
                return _post.Name;
            }
        }

        public string Title
        {
            get
            {
                return _post.Title;
            }
        }

        public int CommentCount
        {
            get
            {
                return _post.CommentCount;
            }
        }

        public int Upvotes
        {
            get
            {
                return _post.Ups;
            }
        }

        public int Downvotes
        {
            get
            {
                return _post.Downs;
            }
        }

        public string Domain 
        {
            get
            {
                return _post.Domain;
            }
        }

        public string Author
        {
            get
            {
                return _post.Author;
            }
        }
    }
}
