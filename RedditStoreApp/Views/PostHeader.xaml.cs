using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public sealed partial class PostHeader : Page
    {
        public PostHeader()
        {
            this.InitializeComponent();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double fixedSize = 0;
            fixedSize += Math.Max(Row1Piece1.DesiredSize.Height, Row1Piece2.DesiredSize.Height);
            fixedSize += Row2Piece.DesiredSize.Height;
            fixedSize += Row3Piece.DesiredSize.Height;
            fixedSize += Row4Piece.DesiredSize.Height;
            double finalHeight = (finalSize.Height - fixedSize) * 2 + fixedSize;

            ((TranslateTransform)ChildGrid.RenderTransform).Y = -(finalSize.Height - fixedSize);

            this.Content.Arrange(new Rect(0, 0, finalSize.Width, finalHeight));
            return finalSize;
        } 

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
