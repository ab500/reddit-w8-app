using GalaSoft.MvvmLight.Command;
using RedditStoreApp.Data.Model;
using Data = RedditStoreApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace RedditStoreApp.ViewModels
{
    public class MoreActionViewModel: FlatCommentCollectionItem
    {
        private Listing<Comment> _parent;
        private int _indentLevel;
        private FlatCommentCollection _listParent;
        private RelayCommand _loadMore;
        private bool _isInProcess;

        public MoreActionViewModel(Listing<Comment> parent, int indentLevel, FlatCommentCollection listParent) : base()
        {
            _isInProcess = false;
            _parent = parent;
            _indentLevel = indentLevel;
            _listParent = listParent;

            _loadMore = new RelayCommand(LoadMoreAction);
        }

        public RelayCommand LoadMore
        {
            get
            {
                return _loadMore;
            }
        }

        public bool IsInProgress
        {
            get
            {
                return _isInProcess;
            }
            set
            {
                _isInProcess = value;
                RaisePropertyChanged("IsInProgress");
                RaisePropertyChanged("CommandText");
            }
        }

        public string CommandText
        {
            get
            {
                if (_isInProcess)
                {
                    return (string)Application.Current.Resources["LoadMore_InProgress"];
                }
                else
                {
                    return (string)Application.Current.Resources["LoadMore_Default"];
                }
            }
        }

        public bool HasMore
        {
            get
            {
                return _parent.HasMore;
            }
        }

        public int IndentLevel
        {
            get
            {
                return _indentLevel;
            }
        }

        public async void LoadMoreAction()
        {
            this.IsInProgress = true;

            if (_parent.HasMore || !_parent.IsLoaded)
            {
                int newComments = 0;

                if (!_parent.IsLoaded)
                {
                    newComments = await Data.Helpers.EnsureCompletion<int>(_parent.Load);
                }
                else
                {
                    newComments = await Data.Helpers.EnsureCompletion<int>(_parent.More);
                }

                for (int i = _parent.Count - newComments; i < _parent.Count; i++)
                {
                    _listParent.VisitComment(_parent[i], _indentLevel, this);
                }

                if (!_parent.HasMore)
                {
                    _listParent.Remove(this);
                    RaisePropertyChanged("HasMore");
                }
            }

            this.IsInProgress = false;
        }
    }
}
