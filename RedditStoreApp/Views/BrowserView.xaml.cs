using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RedditStoreApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrowserView : Page
    {
        private int _showCount = 0;

        public BrowserView()
        {
            this.InitializeComponent();
            Messenger.Default.Register<OverlayDialogMessage>(this, async (msg) =>
            {
                if (msg.Showing)
                {
                    this.WebViewBrush.Redraw();

                    // BUGFIX: Wait for redraw to occur.
                    await Task.Delay(50);

                    this.WebViewRect.Visibility = Visibility.Visible;

                    await Task.Delay(50);

                    this.WebView.Visibility = Visibility.Collapsed;

                    _showCount++;
                }
                else
                {
                    _showCount--;
                    if (_showCount == 0)
                    {
                        await Task.Delay(100);
                        this.WebView.Visibility = Visibility.Visible;
                        this.WebViewRect.Visibility = Visibility.Collapsed;
                    }
                }
            });
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
