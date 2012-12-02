using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace RedditStoreApp.Views.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public VisibilityConverter()
        {
            this.IsInverse = false;
        }

        public bool IsInverse
        {
            get;
            set;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool objVal = (bool)value;

            return objVal != IsInverse ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
