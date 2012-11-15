using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.Data.Factory
{
    public enum RedditApiExceptionType { Connection, Parse, NotAuthed };

    public class RedditApiException : Exception
    {
        public RedditApiExceptionType ExceptionType { get; private set; }

        public RedditApiException(RedditApiExceptionType type) : base(GetStringFromType(type))
        {
            this.ExceptionType = type;
        }

        private static string GetStringFromType(RedditApiExceptionType type)
        {
            switch (type)
            {
                case RedditApiExceptionType.Connection:
                    return "A connection error has occured. Please try again";
                case RedditApiExceptionType.Parse:
                    return "The data received was not in its intented format. Please try again";
                case RedditApiExceptionType.NotAuthed:
                    return "You're not currently logged in or your password has changed.";
                default:
                    return "An unspecified error has occurred";
            }
        }
    }
}
