using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace RedditStoreApp.Controls
{
    public sealed class CueTextBox : TextBox 
    {
        private enum TextBoxState { Empty, Typing, HasText };
        private TextBoxState _currentState = TextBoxState.Empty;

        public CueTextBox()
        {
            this.DefaultStyleKey = typeof(CueTextBox);
            this.GotFocus += CueTextBox_GotFocus;
            this.LostFocus += CueTextBox_LostFocus;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void CueTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_currentState == TextBoxState.Empty)
            {
                this.ActualText = "";
                this.ActualTextBrush = this.Foreground;
            }
            _currentState = TextBoxState.Typing;
        }

        private void CueTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.ActualText))
            {
                this.ActualText = this.CueText;
                this.ActualTextBrush = this.CueTextBrush;
                _currentState = TextBoxState.Empty;
            }
            else
            {
                _currentState = TextBoxState.HasText;
            }
        }

        public static DependencyProperty CueTextProperty = DependencyProperty.Register("CueText", typeof(string), typeof(CueTextBox), new PropertyMetadata("", OnCueTextChanged));

        private static void OnCueTextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CueTextBox c = (CueTextBox)sender;
            c.OnCueTextChanged(e);
        }

        private void OnCueTextChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_currentState == TextBoxState.Empty)
            {
                this.ActualText = this.CueText;
                this.ActualTextBrush = this.CueTextBrush;                
            }
        }

        public string CueText
        {
            get
            {
                return (string)GetValue(CueTextProperty);
            }
            set
            {
                SetValue(CueTextProperty, value);
            }
        }

        public static DependencyProperty ActualTextProperty = DependencyProperty.Register("ActualText", typeof(string), typeof(CueTextBox), new PropertyMetadata("", OnActualTextChanged));

        public string ActualText
        {
            get
            {
                return (string)GetValue(ActualTextProperty);
            }
            set
            {
                SetValue(ActualTextProperty, value);
            }
        }

        private static void OnActualTextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CueTextBox c = (CueTextBox)sender;
            c.OnActualTextChanged(e);
        }

        private void OnActualTextChanged(DependencyPropertyChangedEventArgs e)
        {
            if (_currentState == TextBoxState.Typing)
            {
                this.Text = this.ActualText;
            }
        }

        public static DependencyProperty ActualTextBrushProperty = DependencyProperty.Register("ActualTextBrush", typeof(Brush), typeof(CueTextBox), new PropertyMetadata(null));

        public Brush ActualTextBrush
        {
            get
            {
                return (Brush)GetValue(ActualTextBrushProperty);
            }

            set
            {
                SetValue(ActualTextBrushProperty, value);
            }
        }

        public static DependencyProperty CueTextBrushProperty = DependencyProperty.Register("CueTextBrush", typeof(Brush), typeof(CueTextBox), new PropertyMetadata(new SolidColorBrush(Windows.UI.Colors.Gray)));

        public Brush CueTextBrush
        {
            get
            {
                return (Brush)GetValue(CueTextBrushProperty);
            }

            set
            {
                SetValue(CueTextBrushProperty, value);
            }
        }
    }
}
