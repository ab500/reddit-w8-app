using RedditStoreApp.Data;
using RedditStoreApp.Data.Core;
using RedditStoreApp.Data.Factory;
using RedditStoreApp.Data.Model;
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

namespace RedditStoreApp
{
    public sealed partial class MainPage : Page
    {
        private bool _isLeft = false;

        public MainPage()
        {
            this.InitializeComponent();
            if (this.DataContext != null)
            {
                MainViewModel mvm = (MainViewModel)this.DataContext;
                mvm.PropertyChanged += mvm_PropertyChanged;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            this.MainGrid.Width = availableSize.Width + this.MainGrid.ColumnDefinitions[0].Width.Value;
            this.Content.Measure(new Size(availableSize.Width + 500, availableSize.Height));
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.Content.Arrange(new Rect(0, 0, finalSize.Width + 500, finalSize.Height));
            return finalSize;
        }

        private void mvm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLeft")
            {
                MainViewModel mvm = (MainViewModel)this.DataContext;

                double to = 0;
                double from = 0;

                if (mvm.IsLeft && !_isLeft)
                {
                    to = -MainGrid.ColumnDefinitions[0].Width.Value;
                }
                else if (!mvm.IsLeft && _isLeft)
                {
                    from = -MainGrid.ColumnDefinitions[0].Width.Value;
                }

                if (_isLeft != mvm.IsLeft)
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
                    Storyboard.SetTarget(d, MainGrid);
                    Storyboard.SetTargetProperty(d, "(UIElement.RenderTransform).(TranslateTransform.X)");
                    s.Begin();

                    _isLeft = !_isLeft;
                }
            }
        }
    }
}
