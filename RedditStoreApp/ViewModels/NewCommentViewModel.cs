using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.ViewModels
{
    public class NewCommentViewModel : FlatCommentCollectionItem
    {
        private FlatCommentCollection _parentList;
        private int _indentLevel;
        private string _parentId;

        private RelayCommand _dismiss;
        private RelayCommand _post;

        private string _newCommentText;

        public NewCommentViewModel(string parentId, int indentLevel, FlatCommentCollection parentList)
            : base()
        {
            _parentId = parentId;
            _indentLevel = indentLevel;
            _parentList = parentList;

            _newCommentText = "";

            _dismiss = new RelayCommand(DismissAction);
            _post = new RelayCommand(PostAction);
        }

        private void DismissAction()
        {
            if (_parentList != null)
            {
                _parentList.Remove(this);
                _parentList = null;
            }
        }

        private void PostAction()
        {

        }

        public RelayCommand Dismiss
        {
            get
            {
                return _dismiss;
            }
        }

        public RelayCommand Post
        {
            get
            {
                return _post;
            }
        }

        public string Body
        {
            get
            {
                return _newCommentText;
            }
            set
            {
                _newCommentText = value;
                RaisePropertyChanged("Body");
            }
        }

        public int IndentLevel
        {
            get
            {
                return _indentLevel;
            }
        }
    }
}
