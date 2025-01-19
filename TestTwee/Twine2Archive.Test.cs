using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class Twine2ArchiveTest
    {
        [Test]
        public void Parse_ValidInput_ReturnsTwine2Archive()
        {
            // Arrange
            string input = """<!DOCTYPE html><html><head><meta charset="utf-8"><title>Test Story</title></head><body><tw-storydata name="Test Story" ifid="12345678-1234-5678-1234-567812345678"></tw-storydata></body></html>""";

            // Act
            Twine2Archive result = Twine2Archive.Parse(input);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Stories.Count, Is.EqualTo(1));
            Assert.That(result.Stories[0].Name, Is.EqualTo("Test Story"));
            Assert.That(result.Stories[0].IFID, Is.EqualTo("12345678-1234-5678-1234-567812345678"));
        }

        [Test]
        public void Parse_InvalidInput_ThrowsException()
        {
            // Arrange
            string input = "<html></html>";

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => Twine2Archive.Parse(input));
            Assert.That(ex.Message, Is.EqualTo("ERROR: The document does not contain a <tw-storydata> element."));
        }

        [Test]
        public void CreateHTML_ValidStories_ReturnsCombinedHTML()
        {
            // Arrange
            Twine2Archive archive = new();
            archive.Stories.Add(new Story { Name = "Story 1", IFID = "11111111-1111-1111-1111-111111111111" });
            archive.Stories.Add(new Story { Name = "Story 2", IFID = "22222222-2222-2222-2222-222222222222" });

            // Act
            string result = archive.CreateHTML();

            // Assert
            Assert.That(result, Does.Contain("<tw-storydata name=\"Story 1\""));
            Assert.That(result, Does.Contain("<tw-storydata name=\"Story 2\""));
        }

        [Test]
        public void CreateHTML_EmptyStories_ReturnsEmptyString()
        {
            // Arrange
            Twine2Archive archive = new();

            // Act
            string result = archive.CreateHTML();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Twine2Archive_SetStories() {
            // Arrange
            Twine2Archive archive = new();
            List<Story> stories = [new Story { Name = "Test Story", IFID = "12345678-1234-5678-1234-567812345678" }];

            // Act
            archive.Stories = stories;

            // Assert
            Assert.That(archive.Stories, Is.EqualTo(stories));
        }
    }
}