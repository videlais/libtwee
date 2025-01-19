using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class CompileTwine1HTMLTest
    {
        [Test]
        public void Compile_DefaultStory_EmptyEngineHeaderNameCodeJS()
        {
            // Create story.
            var story = new Story { Name = "Test Story" };

            // Create result based on empty header.
            string result = Twine1HTML.Compile(story, "", "");

            // Assert empty string.
            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void Compile_DefaultStory_VERSION()
        {
            // Create story.
            var story = new Story { Name = "Test Story", Creator = "Test Creator" };

            // Create result based on header with VERSION.
            string result = Twine1HTML.Compile(story, "", "VERSION");

            // Assert header with story creator.
            Assert.That(result, Is.EqualTo("Test Creator"));
        }

        [Test]
        public void Compile_DefaultStory_TIME()
        {
            // Create story.
            var story = new Story { Name = "Test Story", Creator = "Test Creator" };

            // Create result based on header with TIME.
            string result = Twine1HTML.Compile(story, "", "TIME");

            // Assert header with current time.
            Assert.That(result, Is.EqualTo(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")));
        }

        [Test]
        public void Compile_DefaultStory_ENGINE()
        {
            // Create story.
            var story = new Story { Name = "Test Story", Creator = "Test Creator" };

            // Create result based on header with ENGINE.
            string result = Twine1HTML.Compile(story, "engine.js", "ENGINE");

            // Assert header with engine.
            Assert.That(result, Is.EqualTo("engine.js"));
        }

        [Test]
        public void Compile_DefaultStory_STORY_SIZE()
        {
            // Create story.
            var story = new Story { Name = "Test Story", Creator = "Test Creator" };

            // Create result based on header with STORY_SIZE.
            string result = Twine1HTML.Compile(story, "", "STORY_SIZE");

            // Assert header with story size.
            Assert.That(result, Is.EqualTo("0"));
        }

        [Test]
        public void Compile_DefaultStory_STORY()
        {
            // Create story.
            var story = new Story { Name = "Test Story", Creator = "Test Creator" };

            // Create result based on header with STORY.
            string result = Twine1HTML.Compile(story, "", "STORY");

            // Assert header with story.
            Assert.That(result, Is.EqualTo(story.ToTwine1HTML()));
        }

        [Test]
        public void Compile_DefaultStory_START_AT()
        {
            // Create story.
            var story = new Story { Name = "Test Story", Creator = "Test Creator" };

            // Create result based on header with START_AT.
            string result = Twine1HTML.Compile(story, "", "START_AT");

            // Assert header with story.
            Assert.That(result, Is.EqualTo(""));
        }
    }
}