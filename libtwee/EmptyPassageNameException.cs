using System;

namespace libtwee
{
    public class EmptyPassageNameException : Exception
    {
        public EmptyPassageNameException()
            : base("Passage name cannot be an empty string.")
        {
        }

        public EmptyPassageNameException(string message)
            : base(message)
        {
        }

        public EmptyPassageNameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}