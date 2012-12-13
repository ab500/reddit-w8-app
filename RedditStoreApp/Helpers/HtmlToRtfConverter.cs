using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

// Found in the Microsoft samples library, subject to the Microsoft Limited Public License.  
namespace RedditStoreApp.Helpers
{
    public class HtmlToRtfConverter
    {
        public static string GetHtml(DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlProperty);
        }

        public static void SetHtml(DependencyObject obj, string value)
        {
            obj.SetValue(HtmlProperty, value);
        }

        // Using a DependencyProperty as the backing store for Html.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HtmlProperty =
            DependencyProperty.RegisterAttached("Html", typeof(string), typeof(HtmlToRtfConverter), new PropertyMetadata("", OnHtmlChanged));

        private static void OnHtmlChanged(DependencyObject sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            RichTextBlock parent = (RichTextBlock)sender;
            parent.IsTextSelectionEnabled = false;
            parent.Blocks.Clear();

            XmlDocument document = new XmlDocument();
            document.LoadXml((string)"<body>" + eventArgs.NewValue + "</body>");

            ParseElement((XmlElement)(document.GetElementsByTagName("body")[0]), new RichTextBlockTextContainer(parent));
        }

        private static void ParseElement(XmlElement element, ITextContainer parent)
        {

            foreach (var child in element.ChildNodes)
            {
                if (child is Windows.Data.Xml.Dom.XmlText)
                {
                    if (string.IsNullOrEmpty(child.InnerText) ||
                        child.InnerText == "\n")
                    {
                        continue;
                    }

                    parent.Add(new Run { Text = child.InnerText.Trim(new char[] {'\n'}) });
                }
                else if (child is XmlElement)
                {
                    XmlElement e = (XmlElement)child;
                    switch (e.TagName.ToUpper())
                    {
                        case "P":
                            var paragraph = new Paragraph();
                            parent.Add(paragraph);
                            ParseElement(e, new ParagraphTextContainer(paragraph));
                            break;
                        case "STRONG":
                            var bold = new Bold();
                            parent.Add(bold);
                            ParseElement(e, new SpanTextContainer(bold));
                            break;
                        case "U":
                            var underline = new Underline();
                            parent.Add(underline);
                            ParseElement(e, new SpanTextContainer(underline));
                            break;
                        case "A":
                            var inlineElt = new InlineUIContainer();
                            var hyperlink = new HyperlinkButton();
                            inlineElt.Child = hyperlink;
                            hyperlink.Style = (Style)App.Current.Resources["HyperlinkButtonStyle"];
                            hyperlink.Content = e.InnerText;
                            hyperlink.Click += delegate(object sender, RoutedEventArgs eventArgs)
                            {
                                string uriString = e.GetAttribute("href");
                                Messenger.Default.Send<LinkClickedMessage>(new LinkClickedMessage()
                                {
                                    UriToNavigate = uriString
                                });
                            };
                            parent.Add(inlineElt);
                            //ParseElement(e, parent);
                            break;
                        case "BR":
                                parent.Add(new LineBreak());
                            break;
                    }
                }


            }
        }

        private interface ITextContainer
        {
            void Add(Inline text);
            void Add(Block paragraph);
        }

        private sealed class SpanTextContainer : ITextContainer
        {
            private readonly Span _span;

            public SpanTextContainer(Span span)
            {
                _span = span;
            }

            public void Add(Inline text)
            {
                _span.Inlines.Add(text);
            }

            public void Add(Block paragraph)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class ParagraphTextContainer : ITextContainer
        {
            private readonly Paragraph _block;

            public ParagraphTextContainer(Paragraph block)
            {
                _block = block;
            }

            public void Add(Inline text)
            {
                _block.Inlines.Add(text);
            }

            public void Add(Block paragraph)
            {
                throw new NotSupportedException();
            }
        }

        private sealed class RichTextBlockTextContainer : ITextContainer
        {
            private readonly RichTextBlock _richTextBlock;

            public RichTextBlockTextContainer(RichTextBlock richTextBlock)
            {
                _richTextBlock = richTextBlock;
            }

            public void Add(Inline text)
            {
                //throw new NotSupportedException();
                var paragraph = new Paragraph();
                paragraph.Inlines.Add(text);

                _richTextBlock.Blocks.Add(paragraph);
            }

            public void Add(Block paragraph)
            {
                _richTextBlock.Blocks.Add(paragraph);
            }
        }
    } 
}
