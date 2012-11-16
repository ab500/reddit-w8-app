using GalaSoft.MvvmLight;
using RedditStoreApp.ViewModel;
using RedditStoreApp.Data.Model;
using RedditStoreApp.Data.Factory;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;

namespace RedditStoreApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private readonly IRedditApi _dataService;

        private SubredditViewModel _currentSubreddit;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IRedditApi dataService)
        {
            this._dataService = dataService;
            this.Subreddits = new ObservableCollection<SubredditViewModel>();
            this.TestAction = new RelayCommand(async () =>
            {
                Listing<Subreddit> subreddits = await _dataService.GetPopularSubredditsListAsync();
                this.CurrentSubreddit = new SubredditViewModel(subreddits[0]);
            });
        }

        public string HelloString { get { return "hello!"; } }

        public RelayCommand TestAction { get; private set; }

        public ObservableCollection<SubredditViewModel> Subreddits { get; private set; }

        public SubredditViewModel CurrentSubreddit {
            get
            {
                return _currentSubreddit;
            }
            set
            {
                _currentSubreddit = value;
                RaisePropertyChanged("CurrentSubreddit");
            }
        }

    }
}