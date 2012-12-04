using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.ViewModels
{
    public class FlatCommentCollection: ObservableCollection<FlatCommentCollectionItem>
    {
        Listing<Comment> _root;

        public FlatCommentCollection(Listing<Comment> root)
        {
            _root = root;
            Initialize();
        }

        private void Initialize()
        {
            foreach (Comment comment in _root)
            {
                VisitComment(comment, 0);
            }
            AddGetMoreToList(_root, 0);
        }

        private void VisitComment(Comment comment, int indentLevel)
        {
            AddCommentToList(comment, indentLevel);
            foreach (Comment childComment in comment.Replies)
            {
                VisitComment(childComment, indentLevel + 1);
            }

            if (comment.Replies.HasMore)
            {
                AddGetMoreToList(comment.Replies, indentLevel + 1);
            }
        }

        private void AddCommentToList(Comment comment, int indentLevel)
        {
            this.Add(new CommentViewModel(comment, indentLevel));
        }

        private void AddGetMoreToList(Listing<Comment> listing, int indentLevel)
        {
            this.Add(new MoreActionViewModel(listing, indentLevel, this));
        }
    }
}
