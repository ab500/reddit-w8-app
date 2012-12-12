using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
                    SetUri(msg.NewValue.Link);
                    this._rootUri = this.CurrentUri;
                }
            });
        }

        private void GoHomeAction()
        {
            _historyStack.Clear();
            SetUri(_rootUri);
        }

        private void GoBackAction()
        {
            if (_historyStack.Count > 0)
            {
                SetUri(_historyStack.Pop());
            }
        }

        public void PushUri(Uri uri)
        {
            _historyStack.Push(uri);
        }

        private void SetUri(Uri uri)
        {
            Uri finalUri = uri;
            if (uri != null)
            {
                string uriString = uri.ToString();

                Regex imageRegex1 = new Regex(".+.imgur.com/.+.(?:jpg|jpeg|png)");
                Regex imageRegex2 = new Regex("http://imgur.com/(.*)");
                if (imageRegex1.IsMatch(uriString))
                {
                    this.BrowserState = ViewModels.BrowserState.Image;
                }
                else if (imageRegex2.IsMatch(uriString))
                {
                    this.BrowserState = ViewModels.BrowserState.Image;
                    finalUri = new Uri("http://i.imgur.com/" + imageRegex2.Match(uriString).Groups[1].Value + ".jpg", UriKind.Absolute);
                }
                else
                {
                    this.BrowserState = ViewModels.BrowserState.Web;
                }
            }

            this.CurrentUri = finalUri;
        }

        public Uri CurrentUri
        {
            get
            {
                return _browserState == ViewModels.BrowserState.Web ? _currentUri : null;
            }
            private set
            {
                _currentUri = value;
                RaisePropertyChanged("CurrentUri");
                RaisePropertyChanged("CurrentImageUri");
            }
        }

        public Uri CurrentImageUri
        {
            get
            {
                return _browserState == ViewModels.BrowserState.Image? _currentUri : null;
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
                RaisePropertyChanged("CurrentUri");
                RaisePropertyChanged("CurrentImageUri");
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
