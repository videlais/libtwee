using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class CreateTweeTests
    {
        [Test]
        public void TestCreate_ValidStory()
        {
            Story story = new()
            {
                IFID = "12345",
                Format = "2.3.13",
                FormatVersion = "2.3.13",
                Start = "Start",
                TagColors = new Dictionary<string, string> { { "tag1", "#ff0000" } },
                Zoom = 1.0f
            };

            string result = Twee.Create(story);
            // Assert that the result is not null or empty.
            Assert.That(result, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void TestCreate_StoryWithoutPassage()
        {
            Story story = new();
            string result = Twee.Create(story);
            // Assert that the result is not null or empty.
            Assert.That(result, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void TestCreate_StoryWithPassage()
        {
            Story story = new();
            story.AddPassage(new Passage("Start", "This is the start passage."));
            string result = Twee.Create(story);
            // Assert that the result contains the passage name.
            Assert.That(result, Does.Contain(":: Start"));
        }

        [Test]
        public void TestCreate_StoryWithMultiplePassages()
        {
            Story story = new();
            story.AddPassage(new Passage("Start", "This is the start passage."));
            story.AddPassage(new Passage("End", "This is the end passage."));
            string result = Twee.Create(story);
            // Assert that the result contains both passage names.
            Assert.That(result, Does.Contain(":: Start"));
            Assert.That(result, Does.Contain(":: End"));
        }

        [Test]
        public void TestCreate_StoryDataExists()
        {
            Story story = new();
            story.AddPassage(new Passage("StoryData", "This is the story data passage."));
            string result = Twee.Create(story);
            // Assert that the result contains the story data passage.
            Assert.That(result, Does.Contain(":: StoryData"));
        }

        [Test]
        public void TestCreate_HasValidIFID()
        {
            Story story = new()
            {
                IFID = Babel.GenerateTwineIFID()
            };
            string result = Twee.Create(story);
            // Assert that the result contains a valid IFID.
            Assert.That(result, Does.Contain($"\"ifid\": \"{story.IFID}\""));
        }
    }
}