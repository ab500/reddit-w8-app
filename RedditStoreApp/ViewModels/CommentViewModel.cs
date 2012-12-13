using GalaSoft.MvvmLight.Command;
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
        private FlatCommentCollection _parent;

        private Comment _comment;
        private int _indentLevel;
        private bool _isSelected;

        private RelayCommand _upvote;
        private RelayCommand _downvote;
        private RelayCommand _addComment;


        public CommentViewModel(Comment c, int indentLevel, FlatCommentCollection parent) : base()
        {
            _comment = c;
            _indentLevel = indentLevel;
            _isSelected = false;
            _parent = parent;

            _upvote = new RelayCommand(UpvoteAction);
            _downvote = new RelayCommand(DownvoteAction);
            _addComment = new RelayCommand(AddCommentAction);
        }

        public RelayCommand Upvote
        {
            get
            {
                return _upvote;
            }
        }

        public RelayCommand Downvote
        {
            get
            {
                return _downvote;
            }
        }

        public RelayCommand AddComment
        {
            get
            {
                return _addComment;
            }
        }

        private void UpvoteAction()
        {

        }

        private void DownvoteAction()
        {

        }

        private void AddCommentAction()
        {
            _parent.AddReplyBox(this);
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public int IndentLevel
        {
            get
            {
                return _indentLevel;
            }
        }
        public string Body
        {
            get
            {
                return (new MarkdownSharp.Markdown(new MarkdownSharp.MarkdownOptions()
                {
                    AutoNewlines = false,
                    AutoHyperlink = true,
                    EmptyElementSuffix = "/>"
                })).Transform(_comment.Body);
            }
        }
       
        public string Author
        {
            get
            {
                return _comment.Author;
            }
        }

        public int Upvotes
        {
            get
            {
                return _comment.Ups;
            }
        }

        public int Downvotes
        {
            get
            {
                return _comment.Downs;
            }
        }
    }
}
