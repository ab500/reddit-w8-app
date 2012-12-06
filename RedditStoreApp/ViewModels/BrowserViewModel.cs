using GalaSoft.MvvmLight;
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

        public BrowserViewModel()
        {
            _browserState = ViewModels.BrowserState.Video;
            Messenger.Default.Register<PropertyChangedMessage<PostViewModel>>(this, (msg) =>
            {
                if (!msg.NewValue.IsSelf)
                {
                    this.CurrentUri = msg.NewValue.Link;
                    this.BrowserState = ViewModels.BrowserState.Web;
                }
            });
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
    }
}
