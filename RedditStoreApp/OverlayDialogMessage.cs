using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp
{
    class OverlayDialogMessage : MessageBase
    {
        
        private bool _showing = false;

        public bool Showing
        {
            get
            {
                return _showing;
            }
            set
            {
                _showing = value;
            }
        }
    }
}
