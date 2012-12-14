using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace RedditStoreApp.ViewModels
{
    public class PostViewModel: ViewModelBase
    {
        private Post _post;
        private FlatCommentCollection _comments;
        private bool _isLoading;
        private NewCommentViewModel _currentEditBox;

        private RelayCommand _upvote;
        private RelayCommand _downvote;
        private RelayCommand _addComment;

        public PostViewModel(Post post)
        {
            _post = post;
            _comments = new FlatCommentCollection(_post.Comments);

            _comments.EditBoxAdded += delegate(object sender, EditBoxAddedEventArgs e)
            {
                this.CurrentEditBox = e.NewCommentViewModel;
            };

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
            _comments.AddReplyBox(null);
        }

        public NewCommentViewModel CurrentEditBox
        {
            get
            {
                return _currentEditBox;
            }
            set
            {
                if (_currentEditBox != null)
                {
                    // Removes the current edit box from the comment pane.
                    _currentEditBox.Dismiss.Execute(null);
                }
                _currentEditBox = value;
            }
        }
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        public FlatCommentCollection Comments
        {
            get
            {
                return _comments;
            }
        }

        public string ThumbnailUrl
        {
            get
            {
                return _post.ThumbnailUrl;
            }
        }

        public string Name
        {
            get
            {
                return _post.Name;
            }
        }

        public string SelfText
        {
            get
            {
                return _post.SelfText;
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

        public DateTime Created
        {
            get
            {
                return _post.Created;
            }
        }

        public bool HasLoadedComments
        {
            get
            {
                return _post.Comments.IsLoaded;
            }
        }

        public bool HasSelfText
        {
            get
            {
                return !String.IsNullOrEmpty(_post.SelfText) && _post.IsSelf;
            }
        }

        public bool IsSelf
        {
            get
            {
                return _post.IsSelf;
            }
        }

        public Uri Link
        {
            get
            {
                return new Uri(_post.Url, UriKind.Absolute);
            }
        }

        public string UriString
        {
            get
            {
                return _post.Url;
            }
        }

        public bool? Likes
        {
            get
            {
                return _post.Likes;
            }
        }
    }
}
