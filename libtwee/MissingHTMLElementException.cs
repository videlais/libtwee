using System;

namespace libtwee
{
    /// <summary>
    /// Exception thrown when a required HTML element cannot be found during parsing.
    /// </summary>
    public class MissingHTMLElementException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingHTMLElementException"/> class with a default message.
        /// </summary>
        public MissingHTMLElementException()
            : base("HTML Element cannot be found.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingHTMLElementException"/> class with a specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MissingHTMLElementException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingHTMLElementException"/> class with a specified message and inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MissingHTMLElementException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}