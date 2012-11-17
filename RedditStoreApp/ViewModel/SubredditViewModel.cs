using GalaSoft.MvvmLight;
using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.ViewModel
{
    public class SubredditViewModel : ViewModelBase
    {
        private Subreddit _subreddit;

        public SubredditViewModel(Subreddit s)
        {
            _subreddit = s;
            
        }

        public Sort CurrentSort
        {
            get
            {
                return _subreddit.Posts.CurrentSort;
            }
            set
            {
                // NOTE: Fires off async call.
                _subreddit.Posts.ChangeSort(value);
                RaisePropertyChanged("CurrentSort");
            }
        }

        public List<Sort> SortValues
        {
            get
            {
                return Enum.GetValues(typeof(Sort)).OfType<Sort>().ToList();
            }
        }

        public string DisplayName
        {
            get
            {
                return _subreddit.Title;
            }
        }

        public string Description
        {
            get
            {
                return _subreddit.Description;
            }
        }
    }
}
