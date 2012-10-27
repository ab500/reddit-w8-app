using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.Data.Factory
{
    public enum FactoryExceptionType { Connection, Parse, NotAuthed };

    public class FactoryException : Exception
    {
        public FactoryExceptionType ExceptionType { get; private set; }

        public FactoryException(FactoryExceptionType type) : base(GetStringFromType(type))
        {
            this.ExceptionType = type;
        }

        private static string GetStringFromType(FactoryExceptionType type)
        {
            switch (type)
            {
                case FactoryExceptionType.Connection:
                    return "A connection error has occured. Please try again";
                case FactoryExceptionType.Parse:
                    return "The data received was not in its intented format. Please try again";
                case FactoryExceptionType.NotAuthed:
                    return "You're not currently logged in or your password has changed.";
                default:
                    return "An unspecified error has occurred";
            }
        }
    }
}
