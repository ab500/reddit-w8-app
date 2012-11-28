using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.ViewModels
{
    public class CommentViewModel
    {
        private Comment _comment;
        private IncrementalObservableCollection<CommentViewModel> _replies;
        private bool _isLoading;

        public CommentViewModel(Comment c)
        {
            _comment = c;

        }

        public string Body
        {
            get
            {
                return _comment.Body;
            }
        }

        public string Author
        {
            get
            {
                return _comment.Author;
            }
        }

        public int Ups
        {
            get
            {
                return _comment.Ups;
            }
        }

        public int Downs
        {
            get
            {
                return _comment.Downs;
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
        }

        public IncrementalObservableCollection<CommentViewModel> Replies
        {
            get
            {
                return _replies;
            }
        }
    }
}
