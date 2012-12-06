using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using RedditStoreApp.ViewModels;

namespace RedditStoreApp.Views.Converters
{
    class BrowserStateVisibility : IValueConverter
    {

        private BrowserState _visibleState;

        public BrowserState VisibileState
        {
            get
            {
                return _visibleState;
            }
            set
            {
                _visibleState = value;
            }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            BrowserState state = (BrowserState)value;
            return state == _visibleState ? Visibility.Visible : Visibility.Collapsed;
            if (state == _visibleState)
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
