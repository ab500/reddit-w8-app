using GalaSoft.MvvmLight;
using RedditStoreApp.Data.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.ViewModel
{
    class SettingsViewModel : ViewModelBase
    {
        private IRedditApi _dataService;

        public SettingsViewModel(IRedditApi dataService)
        {
            _dataService = dataService;
        }
    }
}
