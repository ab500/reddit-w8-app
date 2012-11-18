using GalaSoft.MvvmLight;
using RedditStoreApp.ViewModels;
using RedditStoreApp.Data.Model;
using RedditStoreApp.Data.Factory;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace RedditStoreApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        private readonly IRedditApi _dataService;

        private bool _isInitialized = false;
        private bool _isLeft = true;

        private SubredditViewModel _currentSubreddit;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IRedditApi dataService)
        {
            this._dataService = dataService;
            this.Subreddits = new ObservableCollection<SubredditViewModel>();
            this.BackArrowPress = new RelayCommand(BackArrowPressAction);
        }

        public async void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            Listing<Subreddit> subreddits = await Data.Helpers.EnsureCompletion<Listing<Subreddit>>(_dataService.GetPopularSubredditsListAsync);

            if (subreddits == null)
            {
                return;
            }

            foreach (var subreddit in subreddits)
            {
                this.Subreddits.Add(new SubredditViewModel(subreddit));
            }

            this.CurrentSubreddit = this.Subreddits[0];
            _isInitialized = true;
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

                IsLeft = true;

                RaisePropertyChanged("CurrentSubreddit");
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

        public RelayCommand BackArrowPress { get; private set; }

        private void BackArrowPressAction()
        {
            this.IsLeft = !this.IsLeft;
        }
    }
}