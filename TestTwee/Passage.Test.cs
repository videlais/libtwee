using System;
using System.Collections.Generic;
using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class PassageTest
    {
        [Test]
        public void TestPassageInitialization()
        {
            Passage passage = new();
            Assert.Multiple(() =>
            {
                Assert.That(passage.Name, Is.EqualTo("Untitled Passage"));
                Assert.That(passage.Text, Is.EqualTo(""));
                Assert.That(passage.Tags.Count, Is.EqualTo(0));
                Assert.That(passage.Metadata.Count, Is.EqualTo(0));
            });

        }

        [Test]
        public void TestPassageInitializationWithNameAndText()
        {
            Passage passage = new("TestPassage", "This is a test passage.");
            Assert.Multiple(() =>
            {
                Assert.That(passage.Name, Is.EqualTo("TestPassage"));
                Assert.That(passage.Text, Is.EqualTo("This is a test passage."));
                Assert.That(passage.Tags.Count, Is.EqualTo(0));
                Assert.That(passage.Metadata.Count, Is.EqualTo(0));
            });

        }

        [Test]
        public void TestAddTag()
        {
            Passage passage = new();
            passage.AddTag("test");
            Assert.That(passage.Tags, Does.Contain("test"));
        }

        [Test]
        public void TestRemoveTag()
        {
            Passage passage = new();
            passage.AddTag("test");
            passage.RemoveTag("test");
            Assert.That(passage.Tags, Does.Not.Contain("test"));
        }

        [Test]
        public void TestAddMetadata()
        {
            Passage passage = new();
            passage.AddMetadata("key", "value");
            Assert.That(passage.Metadata["key"], Is.EqualTo("value"));
        }

        [Test]
        public void TestRemoveMetadata()
        {
            Passage passage = new();
            passage.AddMetadata("key", "value");
            passage.RemoveMetadata("key");
            Assert.That(passage.Metadata.ContainsKey("key"), Is.False);
        }

        [Test]
        public void TestSetMetadata()
        {
            Passage passage = new();
            passage.SetMetadata("key", "value");
            Assert.That(passage.Metadata["key"], Is.EqualTo("value"));
        }

        [Test]
        public void TestGetMetadata()
        {
            Passage passage = new();
            passage.SetMetadata("key", "value");
            Assert.That(passage.GetMetadata("key"), Is.EqualTo("value"));
        }

        [Test]
        public void TestHasMetadata()
        {
            Passage passage = new();
            passage.SetMetadata("key", "value");
            Assert.That(passage.HasMetadata("key"), Is.True);
        }

        [Test]
        public void TestHasTag()
        {
            Passage passage = new();
            passage.AddTag("test");
            Assert.That(passage.HasTag("test"), Is.True);
        }

        [Test]
        public void TestToTwee()
        {
            Passage passage = new()
            {
                Name = "TestPassage",
                Text = "This is a test passage."
            };
            passage.AddTag("tag1");
            passage.AddTag("tag2");
            passage.SetMetadata("author", "John Doe");

            string expectedTwee = ":: TestPassage [tag1 tag2] {\"author\":\"John Doe\"}\nThis is a test passage.";
            Assert.That(passage.ToTwee(), Is.EqualTo(expectedTwee));
        }

        [Test]
        public void TestToTweeWithEmptyTagsAndMetadata()
        {
            Passage passage = new Passage
            {
                Name = "TestPassage",
                Text = "This is a test passage."
            };

            string expectedTwee = ":: TestPassage\nThis is a test passage.";
            Assert.That(passage.ToTwee(), Is.EqualTo(expectedTwee));
        }

        [Test]
        public void TestToTweeWithEmptyText()
        {
            Passage passage = new Passage
            {
                Name = "TestPassage",
                Text = ""
            };

            string expectedTwee = ":: TestPassage\n";
            Assert.That(passage.ToTwee(), Is.EqualTo(expectedTwee));
        }

        [Test]
        public void TestToTweeExceptionWithEmptyName()
        {
            Passage passage = new Passage
            {
                Name = "",
                Text = "This is a test passage."
            };

            Assert.Throws<Exception>(() => passage.ToTwee());
        }

        [Test]
        public void TestToJson()
        {
            Passage passage = new Passage
            {
                Name = "TestPassage",
                Text = "This is a test passage."
            };
            passage.AddTag("tag1");
            passage.AddTag("tag2");
            passage.SetMetadata("author", "John Doe");

            string expectedJson = "{\"name\":\"TestPassage\",\"tags\":[\"tag1\",\"tag2\"],\"metadata\":{\"author\":\"John Doe\"},\"text\":\"This is a test passage.\"}";

            Assert.That(passage.ToJson(), Is.EqualTo(expectedJson));
        }

        [Test]
        public void TestToJsonWithEmptyTagsAndMetadata()
        {
            Passage passage = new()
            {
                Name = "TestPassage",
                Text = "This is a test passage."
            };

            string expectedJson = "{\"name\":\"TestPassage\",\"tags\":[],\"metadata\":{},\"text\":\"This is a test passage.\"}";
            Assert.That(passage.ToJson(), Is.EqualTo(expectedJson));
        }

        [Test]
        public void TestToJsonExceptionWithEmptyName()
        {
            Passage passage = new()
            {
                Name = "",
                Text = "This is a test passage."
            };

            Assert.Throws<Exception>(() => passage.ToJson());
        }

        [Test]
        public void TestToTwine2HTMLDefaultValues()
        {
            Passage passage = new Passage
            {
                Name = "TestPassage",
                Text = "This is a test passage."
            };
            passage.AddTag("tag1");
            passage.AddTag("tag2");

            string expectedHTML = "<tw-passagedata pid=\"1\" name=\"TestPassage\" tags=\"tag1 tag2\">This is a test passage.</tw-passagedata>";
            Assert.That(passage.ToTwine2HTML(), Is.EqualTo(expectedHTML));
        }

        [Test]
        public void TestToTwine2HTMLWithExplicitPID()
        {
            Passage passage = new()
            {
                Name = "TestPassage",
                Text = "This is a test passage."
            };
            passage.AddTag("tag1");
            passage.AddTag("tag2");

            string expectedHTML = "<tw-passagedata pid=\"2\" name=\"TestPassage\" tags=\"tag1 tag2\">This is a test passage.</tw-passagedata>";
            Assert.That(passage.ToTwine2HTML(2), Is.EqualTo(expectedHTML));
        }

        [Test]
        public void TestToTwine2HTMLExceptionWithEmptyName()
        {
            Passage passage = new()
            {
                Name = "",
                Text = "This is a test passage."
            };

            Assert.Throws<Exception>(() => passage.ToTwine2HTML());
        }

        [Test]
        public void TestToTwine2HTMLWithPosition()
        {
            Passage passage = new()
            {
                Name = "TestPassage",
                Text = "This is a test passage."
            };
            passage.AddTag("tag1");
            passage.AddTag("tag2");
            passage.SetMetadata("position", "20,20");

            string expectedHTML = "<tw-passagedata pid=\"1\" name=\"TestPassage\" tags=\"tag1 tag2\" position=\"20,20\">This is a test passage.</tw-passagedata>";
            Assert.That(passage.ToTwine2HTML(), Is.EqualTo(expectedHTML));
        }

        [Test]
        public void TestToTwine2HTMLWithSize()
        {
            Passage passage = new()
            {
                Name = "TestPassage",
                Text = "This is a test passage."
            };
            passage.AddTag("tag1");
            passage.AddTag("tag2");
            passage.SetMetadata("size", "20,20");

            string expectedHTML = "<tw-passagedata pid=\"1\" name=\"TestPassage\" tags=\"tag1 tag2\" size=\"20,20\">This is a test passage.</tw-passagedata>";
            Assert.That(passage.ToTwine2HTML(), Is.EqualTo(expectedHTML));
        }

        [Test]
        public void TestToTwine1HTMLDefaultValues()
        {
            Passage passage = new()
            {
                Name = "TestPassage",
                Text = "This is a test passage."
            };
            passage.AddTag("tag1");
            passage.AddTag("tag2");

            string expectedHTML = "<div tiddler=\"TestPassage\" tags=\"tag1 tag2\" modifier=\"extwee\" twine-position=\"10,10\">This is a test passage.</div>";
            Assert.That(passage.ToTwine1HTML(), Is.EqualTo(expectedHTML));
        }

        [Test]
        public void TestToTwine1HTMLWithPosition()
        {
            Passage passage = new()
            {
                Name = "TestPassage",
                Text = "This is a test passage."
            };
            passage.AddTag("tag1");
            passage.AddTag("tag2");
            passage.SetMetadata("position", "20,20");

            string expectedHTML = "<div tiddler=\"TestPassage\" tags=\"tag1 tag2\" modifier=\"extwee\" twine-position=\"20,20\">This is a test passage.</div>";
            Assert.That(passage.ToTwine1HTML(), Is.EqualTo(expectedHTML));
        }

        [Test]
        public void TestToTwine1HTML_ExceptionWithEmptyName()
        {
            Passage passage = new()
            {
                Name = "",
                Text = "This is a test passage."
            };

            Assert.Throws<Exception>(() => passage.ToTwine1HTML());
        }
    }
}