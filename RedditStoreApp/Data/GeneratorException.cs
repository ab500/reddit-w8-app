using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.Data
{
    public enum GeneratorExceptionType { Connection, Parse, NotAuthed };

    class GeneratorException : Exception
    {
        private GeneratorExceptionType ExceptionType { get; private set; }

        public GeneratorException(GeneratorExceptionType type) : base(GetStringFromType(type))
        {
            this.ExceptionType = type;
        }

        private static string GetStringFromType(GeneratorExceptionType type)
        {
            switch (type)
            {
                case GeneratorExceptionType.Connection:
                    return "A connection error has occured. Please try again";
                case GeneratorExceptionType.Parse:
                    return "The data received was not in its intented format. Please try again";
                case GeneratorExceptionType.NotAuthed:
                    return "You're not currently logged in or your password has changed.";
                default:
                    return "An unspecified error has occurred";
            }
        }
    }
}
