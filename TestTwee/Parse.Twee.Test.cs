using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class ParseTweeTests
    {
        [Test]
        public void TestParse_InvalidDocument()
        {
            string tweeContent = "";
            // Assert that an exception is thrown if the document does not contain any passages.
            Assert.That(() => Twee.Parse(tweeContent), Throws.Exception);
        }

        [Test]
        public void TestParse_ValidDocument()
        {
            string tweeContent = ":: StoryTitle\n\nThis is a passage.";
            // Assert that the document is parsed successfully.
            Assert.That(() => Twee.Parse(tweeContent), Is.Not.Null);
        }

        [Test]
        public void TestParse_ValidDocumentWithTags()
        {
            string tweeContent = ":: StoryTitle [tag1 tag2]\n\nThis is a passage.";
            // Assert that the document is parsed successfully.
            Assert.That(() => Twee.Parse(tweeContent), Is.Not.Null);
        }

        [Test]
        public void TestParse_ValidDocumentWithMetadata()
        {
            string tweeContent = ":: StoryTitle {\"tag1\": \"value1\", \"tag2\": \"value2\"}\n\nThis is a passage.";
            // Assert that the document is parsed successfully.
            Assert.That(() => Twee.Parse(tweeContent), Is.Not.Null);
        }

        [Test]
        public void TestParse_ValidDocumentWithTagsAndMetadata()
        {
            string tweeContent = ":: StoryTitle [tag1 tag2] {\"tag1\": \"value1\", \"tag2\": \"value2\"}\n\nThis is a passage.";
            // Assert that the document is parsed successfully.
            Assert.That(() => Twee.Parse(tweeContent), Is.Not.Null);
        }

        [Test]
        public void TestParse_ValidDocumentWithMultiplePassages()
        {
            string tweeContent = ":: StoryTitle\n\nThis is a passage.\n\n:: AnotherTitle\n\nThis is another passage.";
            // Assert that the document is parsed successfully.
            Assert.That(() => Twee.Parse(tweeContent), Is.Not.Null);
        }

        [Test]
        public void TestParse_VerifyPassageName()
        {
            string tweeContent = ":: Test\nThis is a passage.";
            // Parse the document.
            Story story = Twee.Parse(tweeContent);
            // Assert that the passage name is correct.
            Assert.That(story.Passages[0].Name, Is.EqualTo("Test"));
        }

        [Test]
        public void TestParse_VerifyPassageTags()
        {
            string tweeContent = ":: Test [tag1 tag2]\nThis is a passage.";
            // Parse the document.
            Story story = Twee.Parse(tweeContent);
            // Assert that the passage tags are correct.
            Assert.That(story.Passages[0].Tags, Is.EqualTo(new string[] { "tag1", "tag2" }));
        }

        [Test]
        public void TestParse_VerifyPassageMetadata()
        {
            string tweeContent = ":: Test {\"tag1\": \"value1\", \"tag2\": \"value2\"}\nThis is a passage.";
            // Parse the document.
            Story story = Twee.Parse(tweeContent);
            Assert.Multiple(() =>
            {
                // Assert that the passage metadata is correct.
                Assert.That(story.Passages[0].Metadata["tag1"].ToString(), Is.EqualTo("value1"));
                Assert.That(story.Passages[0].Metadata["tag2"].ToString(), Is.EqualTo("value2"));
            });

        }

        [Test]
        public void TestParse_VerifyPassageContent()
        {
            string tweeContent = ":: Test\nThis is a passage.";
            // Parse the document.
            Story story = Twee.Parse(tweeContent);
            // Assert that the passage content is correct.
            Assert.That(story.Passages[0].Text, Is.EqualTo("This is a passage."));
        }

        [Test]
        public void TestParse_VerifyMultiplePassages()
        {
            string tweeContent = ":: Test\nThis is a passage.\n\n:: Another\n\nThis is another passage.";
            // Parse the document.
            Story story = Twee.Parse(tweeContent);
            // Assert that the number of passages is correct.
            Assert.That(story.Passages, Has.Count.EqualTo(2));
        }

        [Test]
        public void TestParse_VerifyMultiplePassagesContent()
        {
            string tweeContent = ":: Test\nThis is a passage.\n\n:: Another\nThis is another passage.";
            // Parse the document.
            Story story = Twee.Parse(tweeContent);
            Assert.Multiple(() =>
            {
                // Assert that the passage content is correct.
                Assert.That(story.Passages[0].Text, Is.EqualTo("This is a passage."));
                Assert.That(story.Passages[1].Text, Is.EqualTo("This is another passage."));
            });
        }
    }
}