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
        private IncrementalObservableCollection<CommentViewModel> _comments;
        private bool _isLoading;
        private RelayCommand _changeView;

        private bool _isShowingComments = false;

        public PostViewModel(Post post)
        {
            _isShowingComments = true;
            _post = post;
            _comments = new IncrementalObservableCollection<CommentViewModel>(
                () => { return _post.Comments.HasMore; },
                (uint count) =>
                {
                    Func<Task<LoadMoreItemsResult>> taskFunc = async () =>
                    {
                        this.IsLoading = true;
                        int newComments = await _post.Comments.More();

                        int currentPostCount = _comments.Count;
                        for (int i = currentPostCount; i < _post.Comments.Count; i++)
                        {
                            _comments.Add(new CommentViewModel(_post.Comments[i]));
                        }
                        this.IsLoading = false;

                        return new LoadMoreItemsResult()
                        {
                            Count = (uint)newComments
                        };                       
                    };
                    Task<LoadMoreItemsResult> loadMorePostsTask = taskFunc();
                    return loadMorePostsTask.AsAsyncOperation<LoadMoreItemsResult>();
                }
            );

            _changeView = new RelayCommand(ChangeViewAction, () =>
            {
                return !_post.IsSelf && !String.IsNullOrEmpty(_post.Url);
            });
        }


        private void ChangeViewAction()
        {
            this.IsShowingComments = !this.IsShowingComments; 
        }

        public bool IsShowingComments
        {
            get
            {
                return _isShowingComments || (_post.IsSelf && !String.IsNullOrEmpty(_post.Url));
            }
            set
            {
                if (this.IsShowingComments != value)
                {
                    _isShowingComments = value;
                    RaisePropertyChanged("IsShowingComments", !_isShowingComments, _isShowingComments, true);
                }
            }
        }

        public RelayCommand ChangeView
        {
            get
            {
                return _changeView;
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

        public IncrementalObservableCollection<CommentViewModel> Comments
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
    }
}
