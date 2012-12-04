using GalaSoft.MvvmLight.Command;
using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.ViewModels
{
    public class MoreActionViewModel: FlatCommentCollectionItem
    {
        private Listing<Comment> _parent;
        private int _indentLevel;
        private FlatCommentCollection _listParent;
        private RelayCommand _loadMore;

        public MoreActionViewModel(Listing<Comment> parent, int indentLevel, FlatCommentCollection listParent) : base()
        {
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
            if (_parent.HasMore || !_parent.IsLoaded)
            {
                int newComments = 0;

                if (!_parent.IsLoaded)
                {
                    newComments = await _parent.Load();
                }
                else
                {
                    newComments = await _parent.More();
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
        }
    }
}
