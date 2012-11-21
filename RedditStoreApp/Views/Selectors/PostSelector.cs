using RedditStoreApp.Data.Model;
using RedditStoreApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace RedditStoreApp.Views.Selectors
{
    class PostSelector : DataTemplateSelector
    {
        private DataTemplate _thumbnailTemplate;
        private DataTemplate _standardTemplate;

        public DataTemplate ThumbnailTemplate
        {
            get
            {
                return _thumbnailTemplate;
            }
            set
            {
                _thumbnailTemplate = value;
            }
        }

        public DataTemplate StandardTemplate
        {
            get
            {
                return _standardTemplate;
            }
            set
            {
                _standardTemplate = value;
            }
        }

        protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item, Windows.UI.Xaml.DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is PostViewModel)
            {
                PostViewModel post = item as PostViewModel;

                if (String.IsNullOrEmpty(post.ThumbnailUrl))
                {
                    return this.StandardTemplate;
                }
                else
                {
                    return this.ThumbnailTemplate;
                }
            }
            return null;
        }
    }
}
