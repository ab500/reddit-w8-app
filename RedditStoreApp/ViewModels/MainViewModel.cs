using GalaSoft.MvvmLight;
using RedditStoreApp.ViewModels;
using RedditStoreApp.Data.Model;
using RedditStoreApp.Data.Factory;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RedditStoreApp.Data.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RedditStoreApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        private readonly IRedditApi _dataService;

        private bool _isInitialized = false;
        private bool _isLeft = false;
        private bool _isLoading = true;

        private string _quickNavText;
        private string _subredditListHeader;

        private RelayCommand _changeView;
        private bool _isShowingComments = false;

        private SubredditViewModel _currentSubreddit;
        private PostViewModel _currentPost;
        private CommentViewModel _currentComment;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IRedditApi dataService)
        {
            this._dataService = dataService;
            this.Subreddits = new ObservableCollection<SubredditViewModel>();
            this.BackArrowPress = new RelayCommand(BackArrowPressAction);
            this.QuickNavigate = new RelayCommand(QuickNavigateAction);

            Messenger.Default.Register<PropertyChangedMessage<PostViewModel>>(this, (msg) =>
            {
                if (msg.PropertyName == "SelectedPost")
                {
                    this.CurrentPost = msg.NewValue;
                }
            });

            Messenger.Default.Register<LinkClickedMessage>(this, (msg) =>
            {
                if (this.IsShowingComments)
                {
                    ChangeViewAction();
                }
            });

            _changeView = new RelayCommand(ChangeViewAction);
        }

        public async void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            Listing<Subreddit> subreddits = null;

            if (PasswordVaultWrapper.IsStored())
            {
                Func<Task<bool>> loginAction = async () => {
                   return await _dataService.LoginAsync(PasswordVaultWrapper.GetUsername(), PasswordVaultWrapper.GetPassword()); 
                };

                bool didLogin = await Data.Helpers.EnsureCompletion<bool>(loginAction);

                if (didLogin)
                {
                    this.SubredditListHeader = (string)App.Current.Resources["MySubreddits"];
                    subreddits = await Data.Helpers.EnsureCompletion<Listing<Subreddit>>(_dataService.GetMySubredditsListAsync);
                }
            }
            else
            {
                this.SubredditListHeader = (string)App.Current.Resources["PopularSubreddits"];
                subreddits = await Data.Helpers.EnsureCompletion<Listing<Subreddit>>(_dataService.GetPopularSubredditsListAsync);
            }

            if (subreddits == null)
            {
                this.IsLoading = false;
                return;
            }

            foreach (var subreddit in subreddits)
            {
                this.Subreddits.Add(new SubredditViewModel(subreddit));
            }

            this.CurrentSubreddit = this.Subreddits[0];
            _isInitialized = true;
            this.IsLoading = false;
        }

        public ObservableCollection<SubredditViewModel> Subreddits { get; private set; }

        public SubredditViewModel CurrentSubreddit
        {
            get
            {
                return _currentSubreddit;
            }
            set
            {
                value.IsActive = true;
                if (_currentSubreddit != null)
                {
                    _currentSubreddit.IsActive = false;
                }

                _currentSubreddit = value;
                if (!_currentSubreddit.IsLoaded)
                {
                    _currentSubreddit.Refresh.Execute(null);
                }

                //IsLeft = true;

                RaisePropertyChanged("CurrentSubreddit");
            }
        }

        public PostViewModel CurrentPost
        {
            get
            {
                return _currentPost;
            }
            set
            {
                PostViewModel oldValue = _currentPost;
                _currentPost = value;
                if (_currentPost != null)
                {
                    this.IsLeft = true;

                    if (!_currentPost.HasLoadedComments)
                    {
                        ((MoreActionViewModel)_currentPost.Comments[0]).LoadMore.Execute(null);
                    }
                }
                RaisePropertyChanged("CurrentPost", oldValue, _currentPost, true);
            }
        }

        public CommentViewModel CurrentComment
        {
            get
            {
                return _currentComment;
            }
            set
            {
                CommentViewModel oldValue = _currentComment;
                _currentComment = value;

                if (oldValue != null && oldValue is CommentViewModel)
                {
                    oldValue.IsSelected = false;
                }

                if (_currentComment != null && _currentComment is CommentViewModel)
                {
                    _currentComment.IsSelected = true;
                }

                RaisePropertyChanged("CurrentComment", oldValue, _currentComment, true);
            }
        }

        public bool IsLeft
        {
            get
            {
                return _isLeft;
            }
            private set
            {
                _isLeft = value;
                RaisePropertyChanged("IsLeft");
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            private set
            {
                _isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        public string SubredditListHeader
        {
            get
            {
                return _subredditListHeader;
            }
            private set
            {
                _subredditListHeader = value;
                RaisePropertyChanged("SubredditListHeader");
            }
        }

        public string QuickNavText
        {
            get
            {
                return _quickNavText;
            }
            set
            {
                _quickNavText = value;
                RaisePropertyChanged("QuickNavText");
            }
        }

        public RelayCommand QuickNavigate { get; private set; }
        public RelayCommand BackArrowPress { get; private set; }

        private void BackArrowPressAction()
        {
            this.IsLeft = !this.IsLeft;
        }

        private async void QuickNavigateAction()
        {
            string subredditName = ParseQuickNav();
            string notFoundError = (string)App.Current.Resources["Error_NotFound"];

            if (subredditName == null)
            {
                Messenger.Default.Send<DialogMessage>(new DialogMessage()
                {
                    Message = notFoundError
                });
                return;
            }

            SubredditViewModel subredditViewModel = null;

            if (!TryGetExistingSubreddit(subredditName, out subredditViewModel))
            {

                Func<Task<Subreddit>> action = async () =>
                {
                    return await _dataService.GetSubredditByName(ParseQuickNav());
                };

                Subreddit sr = await Data.Helpers.EnsureCompletion<Subreddit>(action, notFoundError);

                if (sr != null)
                {
                    subredditViewModel = new SubredditViewModel(sr);
                    this.Subreddits.Insert(0, subredditViewModel);
                }
            }

            if (subredditViewModel != null)
            {
                this.CurrentSubreddit = subredditViewModel;
            }
        }

        private bool TryGetExistingSubreddit(string subredditName, out SubredditViewModel result)
        {
            result = (from sr in this.Subreddits where subredditName.Equals(sr.DisplayName, StringComparison.OrdinalIgnoreCase) select sr).FirstOrDefault();
            return result != null;
        }

        private string ParseQuickNav()
        {
            string val = this.QuickNavText;
            if (string.IsNullOrEmpty(val))
            {
                return null;
            }

            val = val.Trim();
            val = val.Trim(new char[] { '/' });

            if (val.StartsWith("r/"))
            {
                val = val.Remove(0, 2);
            }

            val = val.Replace(" ", "");

            return val;
        }

        private void ChangeViewAction()
        {
            this.IsShowingComments = !this.IsShowingComments; 
        }

        public bool IsShowingComments
        {
            get
            {
                return _isShowingComments || (_currentPost != null && _currentPost.IsSelf && !String.IsNullOrEmpty(_currentPost.UriString));
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

    }
}