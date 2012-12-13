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
    public class EditBoxAddedEventArgs : EventArgs
    {
        private NewCommentViewModel _newCommentViewModel;

        public EditBoxAddedEventArgs(NewCommentViewModel ncvm)
        {
            _newCommentViewModel = ncvm;
        }

        public NewCommentViewModel NewCommentViewModel
        {
            get
            {
                return _newCommentViewModel;
            }
        }
    }

    public class FlatCommentCollection : ObservableCollection<FlatCommentCollectionItem>
    {
        public event EventHandler<EditBoxAddedEventArgs> EditBoxAdded;

        Listing<Comment> _root;

        public FlatCommentCollection(Listing<Comment> root)
        {
            _root = root;
            Initialize();
        }

        public void AddReplyBox(FlatCommentCollectionItem insertBefore)
        {
            NewCommentViewModel ncvm = null;

            if (insertBefore == null)
            {
                ncvm = new NewCommentViewModel("", 0, this);
                this.Insert(0, ncvm);
            }
            else
            {
                int newIndentLevel = ((CommentViewModel)insertBefore).IndentLevel + 1;
                ncvm = new NewCommentViewModel("", newIndentLevel, this);
                this.Insert(this.IndexOf(insertBefore) + 1, ncvm);
            }

            if (EditBoxAdded != null)
            {
                EditBoxAdded(this, new EditBoxAddedEventArgs(ncvm));
            }
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
            CommentViewModel cvm = new CommentViewModel(comment, indentLevel, this);

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
