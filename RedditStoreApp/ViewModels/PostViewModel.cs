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
    }
}
