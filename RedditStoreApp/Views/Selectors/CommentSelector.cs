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
    class CommentSelector: DataTemplateSelector
    {
        private DataTemplate _moreTemplate;
        private DataTemplate _standardTemplate;
        private DataTemplate _newCommentTemplate;

        public DataTemplate MoreTemplate
        {
            get
            {
                return _moreTemplate;
            }
            set
            {
                _moreTemplate = value;
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

        public DataTemplate NewCommentTemplate
        {
            get
            {
                return _newCommentTemplate;
            }
            set
            {
                _newCommentTemplate = value;
            }
        }

        protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item, Windows.UI.Xaml.DependencyObject container)
        {
            if (item is MoreActionViewModel)
            {
                return _moreTemplate;
            }
            else if (item is NewCommentViewModel)
            {
                return _newCommentTemplate;
            }
            else
            {
                return _standardTemplate;
            }
        }
    }
}
