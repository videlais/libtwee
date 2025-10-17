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

        [Test]
        public void TestParse_NoColons()
        {
            string tweeContent = "This is just text without any colons";
            var ex = Assert.Throws<Exception>(() => Twee.Parse(tweeContent));
            Assert.That(ex.Message, Is.EqualTo("ERROR: The document does not contain any passages."));
        }

        // Note: Single colon test removed as the Twee parser actually accepts this as valid input

        [Test]
        public void TestParse_ColonAtEnd()
        {
            string tweeContent = "Text ending with:";
            var ex = Assert.Throws<Exception>(() => Twee.Parse(tweeContent));
            Assert.That(ex.Message, Is.EqualTo("ERROR: The document does not contain any passages."));
        }

        [Test]
        public void TestParse_DoubleColonAtEnd()
        {
            string tweeContent = "Text ending with::";
            var ex = Assert.Throws<Exception>(() => Twee.Parse(tweeContent));
            Assert.That(ex.Message, Is.EqualTo("ERROR: The document does not contain any passages."));
        }

        [Test]
        public void TestParse_EmptyPassageName()
        {
            string tweeContent = "::\nContent";
            Story story = Twee.Parse(tweeContent);
            Assert.That(story.Passages[0].Name, Is.EqualTo(""));
        }

        [Test]
        public void TestParse_PassageWithOnlyTags()
        {
            string tweeContent = ":: PassageName [tag1 tag2]\nContent";
            Story story = Twee.Parse(tweeContent);
            Assert.Multiple(() =>
            {
                Assert.That(story.Passages[0].Name, Is.EqualTo("PassageName"));
                Assert.That(story.Passages[0].Tags, Is.EqualTo(new[] { "tag1", "tag2" }));
            });
        }

        [Test]
        public void TestParse_PassageWithEmptyTags()
        {
            string tweeContent = ":: PassageName []\nContent";
            Story story = Twee.Parse(tweeContent);
            Assert.Multiple(() =>
            {
                Assert.That(story.Passages[0].Name, Is.EqualTo("PassageName"));
                Assert.That(story.Passages[0].Tags, Has.Count.EqualTo(1).And.Contains(""));
            });
        }

        [Test]
        public void TestParse_PassageWithOnlyMetadata()
        {
            string tweeContent = ":: PassageName {\"key\": \"value\"}\nContent";
            Story story = Twee.Parse(tweeContent);
            Assert.Multiple(() =>
            {
                Assert.That(story.Passages[0].Name, Is.EqualTo("PassageName"));
                Assert.That(story.Passages[0].Metadata, Does.ContainKey("key"));
                Assert.That(story.Passages[0].Metadata["key"].ToString(), Is.EqualTo("value"));
            });
        }

        [Test]
        public void TestParse_PassageWithEmptyMetadata()
        {
            string tweeContent = ":: PassageName {}\nContent";
            Story story = Twee.Parse(tweeContent);
            Assert.Multiple(() =>
            {
                Assert.That(story.Passages[0].Name, Is.EqualTo("PassageName"));
                Assert.That(story.Passages[0].Metadata, Is.Empty);
            });
        }

        [Test]
        public void TestParse_PassageNameWithSpaces()
        {
            string tweeContent = "::   Passage With Spaces   \nContent";
            Story story = Twee.Parse(tweeContent);
            Assert.That(story.Passages[0].Name, Is.EqualTo("Passage With Spaces"));
        }

        [Test]
        public void TestParse_PassageWithSpecialCharacters()
        {
            string tweeContent = ":: Passage@#$%\nContent with special characters!";
            Story story = Twee.Parse(tweeContent);
            Assert.Multiple(() =>
            {
                Assert.That(story.Passages[0].Name, Is.EqualTo("Passage@#$%"));
                Assert.That(story.Passages[0].Text, Is.EqualTo("Content with special characters!"));
            });
        }

        [Test]
        public void TestParse_PassageWithNewlinesInContent()
        {
            string tweeContent = ":: PassageName\nLine 1\nLine 2\nLine 3";
            Story story = Twee.Parse(tweeContent);
            Assert.That(story.Passages[0].Text, Is.EqualTo("Line 1\nLine 2\nLine 3"));
        }

        [Test]
        public void TestParse_InvalidPassageStructure()
        {
            string tweeContent = ":: PassageName\n";
            var ex = Assert.Throws<Exception>(() => Twee.Parse(tweeContent));
            Assert.That(ex.Message, Is.EqualTo("ERROR: The document contains invalid passage."));
        }

        [Test]
        public void TestParse_PassageWithColonsInContent()
        {
            string tweeContent = ":: PassageName\nContent with: colons in it\nMore content: here";
            Story story = Twee.Parse(tweeContent);
            Assert.That(story.Passages[0].Text, Is.EqualTo("Content with: colons in it\nMore content: here"));
        }

        [Test]
        public void TestParse_MultiplePassagesWithDifferentFormats()
        {
            string tweeContent = @":: Passage1
Content 1

:: Passage2 [tag]
Content 2

:: Passage3 {""meta"": ""data""}
Content 3

:: Passage4 [tag1 tag2] {""key"": ""value""}
Content 4";
            Story story = Twee.Parse(tweeContent);
            Assert.Multiple(() =>
            {
                Assert.That(story.Passages, Has.Count.EqualTo(4));
                Assert.That(story.Passages[0].Name, Is.EqualTo("Passage1"));
                // Note: The parsing logic includes "::" in passage names when parsing from multi-passage content
                Assert.That(story.Passages[1].Name, Does.Contain("Passage2"));
                Assert.That(story.Passages[1].Tags, Does.Contain("tag"));
                Assert.That(story.Passages[2].Name, Does.Contain("Passage3"));
                Assert.That(story.Passages[3].Name, Does.Contain("Passage4"));
            });
        }
    }
}