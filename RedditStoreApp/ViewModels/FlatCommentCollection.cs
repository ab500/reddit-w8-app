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

        public NewCommentViewModel AddReplyBox(FlatCommentCollectionItem insertBefore)
        {
            var ncvm = new NewCommentViewModel("", 0, this);

            if (insertBefore == null)
            {
                this.Insert(0, ncvm);
            }
            else
            {
                this.Insert(this.IndexOf(insertBefore)+1, ncvm);
            }

            return ncvm;
        }

        private void Initialize()
        {
            foreach (Comment comment in _root)
            {
                VisitComment(comment, 0);
            }
            AddGetMoreToList(_root, 0, null);
        }

        public void VisitComment(Comment comment, int indentLevel, FlatCommentCollectionItem insertBefore = null)
        {
            AddCommentToList(comment, indentLevel, insertBefore);
            foreach (Comment childComment in comment.Replies)
            {
                VisitComment(childComment, indentLevel + 1, insertBefore);
            }

            if (comment.Replies.HasMore)
            {
                AddGetMoreToList(comment.Replies, indentLevel + 1, insertBefore);
            }
        }

        private void AddCommentToList(Comment comment, int indentLevel, FlatCommentCollectionItem insertBefore)
        {
            CommentViewModel cvm = new CommentViewModel(comment, indentLevel);

            if (insertBefore != null)
            {
                this.Insert(IndexOf(insertBefore), cvm);
            }
            else
            {
                this.Add(cvm);
            }
        }

        private void AddGetMoreToList(Listing<Comment> listing, int indentLevel, FlatCommentCollectionItem insertBefore)
        {
            MoreActionViewModel mavm = new MoreActionViewModel(listing, indentLevel, this);
             if (insertBefore != null)
            {
                this.Insert(IndexOf(insertBefore), mavm);
            }
            else
            {
                this.Add(mavm);
            }
        }
    }
}
