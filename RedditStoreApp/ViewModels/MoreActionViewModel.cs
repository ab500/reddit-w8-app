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

        public async void LoadMoreAction()
        {
            if (_parent.HasMore || !_parent.IsLoaded)
            {
                int newComments = await _parent.Load();

                for (int i = _parent.Count - newComments; i < _parent.Count; i++)
                {
                    _listParent.Insert(_listParent.IndexOf(this), new CommentViewModel(_parent[i], _indentLevel)); 
                }

                if (!_parent.HasMore)
                {
                    RaisePropertyChanged("HasMore");
                }
            }
        }
    }
}
