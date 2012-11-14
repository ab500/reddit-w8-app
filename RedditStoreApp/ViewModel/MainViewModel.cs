using GalaSoft.MvvmLight;
using RedditStoreApp.ViewModel;
using RedditStoreApp.Data.Model;
using RedditStoreApp.Data.Factory;
using System.Collections.ObjectModel;

namespace RedditStoreApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private readonly IRedditApi _dataService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IRedditApi dataService)
        {
            this._dataService = dataService;
            this.Subreddits = new ObservableCollection<SubredditViewModel>();
        }

        

        public string HelloString { get { return "hello!"; } }

        public ObservableCollection<SubredditViewModel> Subreddits { get; private set; }

    }
}