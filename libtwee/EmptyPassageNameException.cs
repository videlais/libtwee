using System;

namespace libtwee
{
    /// <summary>
    /// Exception thrown when a passage name is empty or null.
    /// </summary>
    public class EmptyPassageNameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyPassageNameException"/> class with a default message.
        /// </summary>
        public EmptyPassageNameException()
            : base("Passage name cannot be an empty string.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyPassageNameException"/> class with a specified message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EmptyPassageNameException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyPassageNameException"/> class with a specified message and inner exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public EmptyPassageNameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}