using System;
using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class EmptyPassageNameExceptionTests
    {
        [Test]
        public void Constructor_ShouldInitializeMessage()
        {
            // Arrange
            var expectedMessage = "Passage name cannot be empty.";

            // Act
            var exception = new EmptyPassageNameException();

            // Assert
            Assert.That(expectedMessage, Is.EqualTo(exception.Message));
        }

        [Test]
        public void Constructor_WithCustomMessage_ShouldInitializeMessage()
        {
            // Arrange
            var customMessage = "Custom error message.";

            // Act
            var exception = new EmptyPassageNameException(customMessage);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(customMessage));
        }

        [Test]
        public void Constructor_WithCustomMessageAndInnerException_ShouldInitializeProperties()
        {
            // Arrange
            var customMessage = "Custom error message.";
            var innerException = new Exception("Inner exception message.");

            // Act
            var exception = new EmptyPassageNameException(customMessage, innerException);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(customMessage));
            Assert.That(exception.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        public void Constructor_ShouldBeOfTypeException()
        {
            // Act
            var exception = new EmptyPassageNameException();

            // Assert
            Assert.That(exception, Is.InstanceOf<Exception>());
        }
    }
}