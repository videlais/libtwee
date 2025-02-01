using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class CompileTwine2HTMLTest
    {
        [Test]
        public void Compile_InvalidInput_ThrowsException_StoryFormatSourceEmpty()
        {
            // Arrange
            var story = new Story { Name = "Test Story" };
            var storyFormat = new StoryFormat { Source = "" };

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => Twine2HTML.Compile(story, storyFormat));
            Assert.That(ex.Message, Is.EqualTo("ERROR: The source of the story format is empty."));
        }

        [Test]
        public void Compile_InvalidInput_ThrowsException_StoryIFIDInvalid()
        {
            // Arrange
            var story = new Story { Name = "Test Story", IFID = "invalid-ifid" };
            var storyFormat = new StoryFormat { Source = "<html>{{STORY_DATA}}</html>" };

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => Twine2HTML.Compile(story, storyFormat));
            Assert.That(ex.Message, Is.EqualTo("ERROR: The story IFID is not a valid."));
        }

        [Test]
        public void Compile_ValidInput_MinimumStory()
        {
            // Arrange
            var story = new Story { Name = "Test Story", IFID = "12345678-1234-5678-1234-567812345678" };
            var storyFormat = new StoryFormat { Source = """<!DOCTYPE html><html><head><meta charset=\"utf-8\"><title>{{STORY_NAME}}</title></head><body>{{STORY_DATA}}</body></html>""" };

            // Act
            string result = Twine2HTML.Compile(story, storyFormat);

            // Assert contains story name
            Assert.That(result, Does.Contain("<tw-storydata name=\"Test Story\""));
            // Assert contains IFID
            Assert.That(result, Does.Contain("ifid=\"12345678-1234-5678-1234-567812345678\""));
        }
    }
}