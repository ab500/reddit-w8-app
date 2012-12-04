using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.ViewModels
{
    public class CommentViewModel: FlatCommentCollectionItem
    {
        private Comment _comment;

        public CommentViewModel(Comment c, int indentLevel) : base()
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
    }
}
