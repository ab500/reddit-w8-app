using GalaSoft.MvvmLight;
using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedditStoreApp.ViewModels;
using System.Collections.ObjectModel;
using DataNs = RedditStoreApp.Data;

using GalaSoft.MvvmLight.Command;

namespace RedditStoreApp.ViewModels
{
    public class SubredditViewModel : ViewModelBase
    {
        private Subreddit _subreddit;
        private ObservableCollection<PostViewModel> _posts;

        private bool _isLoading = false;

        private RelayCommand _refreshCommand;
        private RelayCommand _loadMoreCommand;

        public SubredditViewModel(Subreddit s)
        {
            _posts = new ObservableCollection<PostViewModel>();
            _subreddit = s;
            _refreshCommand = new RelayCommand(RefreshAction);
            _loadMoreCommand = new RelayCommand(LoadMoreAction);
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
                ChangeSort(value);
                RaisePropertyChanged("CurrentSort");
            }
        }

        public bool IsLoaded
        {
            get
            {
                return _subreddit.Posts.IsLoaded;
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

        public ObservableCollection<PostViewModel> Posts
        {
            get
            {
                return _posts;
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

        public RelayCommand Refresh
        {
            get
            {
                return _refreshCommand;
            }
        }

        public RelayCommand LoadMore
        {
            get
            {
                return _loadMoreCommand;
            }
        }

        private async void ChangeSort(Sort value)
        {
            this.IsLoading = true;
            _posts.Clear();

            Func<Task> func = async () =>
            {
                await _subreddit.Posts.ChangeSort(value);
            };
            await DataNs.Helpers.EnsureCompletion(func);

            foreach (var post in _subreddit.Posts)
            {
                _posts.Add(new PostViewModel(post));
            }

            this.IsLoading = false;
        }

        private async void RefreshAction()
        {
            this.IsLoading = true;
            _posts.Clear();

            await DataNs.Helpers.EnsureCompletion(_subreddit.Posts.Refresh);

            foreach (var post in _subreddit.Posts)
            {
                _posts.Add(new PostViewModel(post));
            }

            this.IsLoading = false;
        }

        private async void LoadMoreAction()
        {
            this.IsLoading = true;

            int newPosts = await DataNs.Helpers.EnsureCompletion<int>(_subreddit.Posts.More);

            for (int i = _subreddit.Posts.Count - newPosts; i < _subreddit.Posts.Count; i++)
            {
                _posts.Add(new PostViewModel(_subreddit.Posts[i]));
            }
        }

        private void MergeCollections(Listing<Post> input)
        {
            // Step one.... remove any deleted items.
            List<PostViewModel> toRemove = new List<PostViewModel>();

            foreach (var postvm in _posts)
            {
                bool doesExist = (from post in input where post.Name == postvm.Name select post).Count() > 0;
                if (!doesExist)
                {
                    toRemove.Add(postvm);
                }
            }

            foreach (var postvm in toRemove)
            {
                _posts.Remove(postvm);
            }

            // Step two.... add any new items.
            foreach (var post in input)
            {
                bool doesExist = (from postvm in _posts where postvm.Name == post.Name select postvm).Count() > 0;
                if (!doesExist)
                {
                    _posts.Add(new PostViewModel(post));
                }
            }
        }
    }
}
