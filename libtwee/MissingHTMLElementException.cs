using System;

namespace libtwee
{
    public class MissingHTMLElementException : Exception
    {
        public MissingHTMLElementException()
            : base("HTML Element cannot be found.")
        {
        }

        public MissingHTMLElementException(string message)
            : base(message)
        {
        }

        public MissingHTMLElementException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}