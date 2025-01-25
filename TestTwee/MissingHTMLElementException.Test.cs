using System;
using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class MissingHTMLElementExceptionTests
    {
        [Test]
        public void Constructor_ShouldInitializeWithDefaultMessage()
        {
            var exception = new MissingHTMLElementException();
            Assert.That(exception.Message, Is.EqualTo("HTML Element cannot be found."));
        }

        [Test]
        public void Constructor_ShouldInitializeWithCustomMessage()
        {
            var customMessage = "Custom error message";
            var exception = new MissingHTMLElementException(customMessage);
            Assert.That(customMessage, Is.EqualTo(exception.Message));
        }

        [Test]
        public void Constructor_ShouldInitializeWithCustomMessageAndInnerException()
        {
            var customMessage = "Custom error message";
            var innerException = new Exception("Inner exception message");
            var exception = new MissingHTMLElementException(customMessage, innerException);

            Assert.Multiple(() =>
            {
                Assert.That(customMessage, Is.EqualTo(exception.Message));
                Assert.That(innerException, Is.EqualTo(exception.InnerException));
            });

        }

        [Test]
        public void Constructor_ShouldInitializeWithInnerException()
        {
            var innerException = new Exception("Inner exception message");
            var exception = new MissingHTMLElementException("Custom error message", innerException);

            Assert.Multiple(() =>
            {
                Assert.That(exception.Message, Is.EqualTo("Custom error message"));
                Assert.That(innerException, Is.EqualTo(exception.InnerException));
            });

        }
    }
}