using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.ViewModels
{
    public enum BrowserState { Web, Image, Video };

    public class BrowserViewModel : ViewModelBase
    {
        private BrowserState _browserState;
        private Uri _currentUri;
        private Uri _rootUri;

        private RelayCommand _goHome;
        private RelayCommand _goBack;

        private Stack<Uri> _historyStack;

        public BrowserViewModel()
        {
            _historyStack = new Stack<Uri>();
            _browserState = ViewModels.BrowserState.Web;

            _goBack = new RelayCommand(GoBackAction);
            _goHome = new RelayCommand(GoHomeAction);

            Messenger.Default.Register<PropertyChangedMessage<PostViewModel>>(this, (msg) =>
            {
                if (!msg.NewValue.IsSelf)
                {
                    this.CurrentUri = msg.NewValue.Link;
                    this._rootUri = this.CurrentUri;
                    this.BrowserState = ViewModels.BrowserState.Web;
                }
            });
        }

        private void GoHomeAction()
        {
            _historyStack.Clear();
            this.CurrentUri = _rootUri;
        }

        private void GoBackAction()
        {
            if (_historyStack.Count > 0)
            {
                this.CurrentUri = _historyStack.Pop();
            }
        }

        public void PushUri(Uri uri)
        {
            _historyStack.Push(uri);
        }

        public Uri CurrentUri
        {
            get
            {
                return _currentUri;
            }
            private set
            {
                _currentUri = value;
                RaisePropertyChanged("CurrentUri");
            }
        }

        public BrowserState BrowserState
        {
            get
            {
                return _browserState;
            }
            private set
            {
                _browserState = value;
                RaisePropertyChanged("BrowserState");
            }
        }

        public RelayCommand GoBack
        {
            get
            {
                return _goBack;
            }
        }

        public RelayCommand GoHome
        {
            get
            {
                return _goHome;
            }
        }
    }
}
