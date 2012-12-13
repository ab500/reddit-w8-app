using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp
{
    class LinkClickedMessage : MessageBase 
    {
        private string _uriToNavigate;

        public string UriToNavigate
        {
            get
            {
                return _uriToNavigate;
            }
            set
            {
                _uriToNavigate = value;
            }
        }
    }
}
