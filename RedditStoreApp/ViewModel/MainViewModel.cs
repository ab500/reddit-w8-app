using GalaSoft.MvvmLight;
using VModel = RedditStoreApp.ViewModel.Model;
using DModel = RedditStoreApp.Data.Model;
using System.Collections.ObjectModel;

namespace RedditStoreApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            this.Subreddits = new ObservableCollection<VModel.Subreddit>();

            
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        public string HelloString { get { return "hello!"; } }

        public ObservableCollection<VModel.Subreddit> Subreddits { get; private set; }

    }
}