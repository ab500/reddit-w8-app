using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace RedditStoreApp.ViewModels
{
    public class SupportIncrementalLoadingWrapper<T>: ObservableCollection<T>, ISupportIncrementalLoading
    {
        private Func<bool> _hasMoreItems;
        private Func<uint, IAsyncOperation<LoadMoreItemsResult>> _loadMoreItems;

        public SupportIncrementalLoadingWrapper(Func<bool> hasMoreItems, Func<uint, IAsyncOperation<LoadMoreItemsResult>> loadMoreItems)
        {
            _hasMoreItems = hasMoreItems;
            _loadMoreItems = loadMoreItems;
        }

        public bool HasMoreItems
        {
            get { return _hasMoreItems(); }
        }

        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return _loadMoreItems(count);
        }
    }
}
