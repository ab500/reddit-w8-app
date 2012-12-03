using GalaSoft.MvvmLight.Messaging;
using RedditStoreApp.ViewModels;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RedditStoreApp.Views
{
    public sealed partial class PostHeader : Page
    {
        private bool _isShowingComments = true;
        private double _translateAmount = 0.0;

        public PostHeader()
        {
            this.InitializeComponent();

            Messenger.Default.Register<PropertyChangedMessage<PostViewModel>>(this, (msg) =>
            {
                if (msg.PropertyName == "CurrentPost")
                {
                    UpdatePosition(msg.NewValue.IsShowingComments);
                }
            });

            Messenger.Default.Register<PropertyChangedMessage<bool>>(this, (msg) =>
            {
                if (msg.PropertyName == "IsShowingComments")
                {
                    UpdatePosition(msg.NewValue);
                }
            });
        }

        private void UpdatePosition(bool isShowingComments)
        {
            double to = 0.0;
            double from = 0.0;

            if (isShowingComments && !_isShowingComments)
            {
                to = -_translateAmount;
            }
            else if (!isShowingComments && _isShowingComments)
            {
                from = -_translateAmount;
            }

            if (isShowingComments != _isShowingComments)
            {
                DoubleAnimation d = new DoubleAnimation()
                {
                    BeginTime = new TimeSpan(0),
                    Duration = new Duration(TimeSpan.FromMilliseconds(400)),
                    From = from,
                    To = to,
                    FillBehavior = FillBehavior.HoldEnd,
                    EasingFunction = new QuadraticEase()
                };

                Storyboard s = new Storyboard();
                s.Children.Add(d);
                Storyboard.SetTarget(d, ChildGrid);
                Storyboard.SetTargetProperty(d, "(UIElement.RenderTransform).(TranslateTransform.Y)");
                s.Begin();

                d.Completed += delegate
                {
                    ((TranslateTransform)ChildGrid.RenderTransform).Y = to;
                    s.Stop();
                };

                _isShowingComments = !_isShowingComments;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double fixedSize = 0;
            fixedSize += Math.Max(Row1Piece1.DesiredSize.Height, Row1Piece2.DesiredSize.Height);
            fixedSize += Row2Piece.DesiredSize.Height;
            fixedSize += Row3Piece.DesiredSize.Height;
            fixedSize += Row4Piece.DesiredSize.Height;

            _translateAmount = finalSize.Height - fixedSize;
            double finalHeight = (finalSize.Height - fixedSize) * 2 + fixedSize;

            if (_isShowingComments)
            {
                ((TranslateTransform)ChildGrid.RenderTransform).Y = -_translateAmount;
            }

            this.Content.Arrange(new Rect(0, 0, finalSize.Width, finalHeight));
            return finalSize;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
