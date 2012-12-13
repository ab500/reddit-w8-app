using RedditStoreApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.ViewModels
{
    public class CommentViewModel: FlatCommentCollectionItem
    {
        private Comment _comment;
        private int _indentLevel;
        private bool _isSelected;

        public CommentViewModel(Comment c, int indentLevel) : base()
        {
            _comment = c;
            _indentLevel = indentLevel;
            _isSelected = false;
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }

        public int IndentLevel
        {
            get
            {
                return _indentLevel;
            }
        }
        public string Body
        {
            get
            {
                return (new MarkdownSharp.Markdown(new MarkdownSharp.MarkdownOptions()
                {
                    AutoNewlines = false,
                    AutoHyperlink = true
                })).Transform(_comment.Body);
            }
        }
       
        public string Author
        {
            get
            {
                return _comment.Author;
            }
        }

        public int Upvotes
        {
            get
            {
                return _comment.Ups;
            }
        }

        public int Downvotes
        {
            get
            {
                return _comment.Downs;
            }
        }
    }
}
