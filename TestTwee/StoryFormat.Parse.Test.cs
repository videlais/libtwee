using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class StoryFormatParseTests
    {
        [Test]
        public void TestParse_Invalid()
        {
            string json = "";
            Assert.Throws<Exception>(() => StoryFormat.Parse(json));
        }

        [Test]
        public void TestParseValidJson()
        {
            string json = """window.storyFormat({"name":"Test Format","version":"1.0.0","author":"Test Author","description":"Test Description","image":"test.png","url":"http://example.com","license":"MIT","proofing":true,"source":"<html>{{STORY_DATA}}</html>"});""";
            var format = StoryFormat.Parse(json);
            Assert.Multiple(() =>
            {
                Assert.That(format.Name, Is.EqualTo("Test Format"));
                Assert.That(format.Version, Is.EqualTo("1.0.0"));
                Assert.That(format.Author, Is.EqualTo("Test Author"));
                Assert.That(format.Description, Is.EqualTo("Test Description"));
                Assert.That(format.Image, Is.EqualTo("test.png"));
                Assert.That(format.Url, Is.EqualTo("http://example.com"));
                Assert.That(format.License, Is.EqualTo("MIT"));
                Assert.That(format.Proofing, Is.True);
                Assert.That(format.Source, Is.EqualTo("<html>{{STORY_DATA}}</html>"));
            });
        }

        [Test]
        public void TestParseInvalidJson()
        {
            string json = """window.storyFormat({"name":"Test Format","version":"1.0.0","author":"Test Author","description":"Test Description","image":"test.png","url":"http://example.com","license":"MIT","proofing":true,"source":"<html>{{STORY_DATA}}</html>");""";
            Assert.Throws<Exception>(() => StoryFormat.Parse(json));
        }

        [Test]
        public void TestParseMissingFunctionBeginning()
        {
            string json = """{"name":"Test Format","version":"1.0.0","author":"Test Author","description":"Test Description","image":"test.png","url":"http://example.com","license":"MIT","proofing":true,"source":"<html>{{STORY_DATA}}</html>"});""";
            Assert.Throws<Exception>(() => StoryFormat.Parse(json));
        }

        [Test]
        public void TestParseMissingFunctionEnd()
        {
            string json = """window.storyFormat({"name":"Test Format","version":"1.0.0","author":"Test Author","description":"Test Description","image":"test.png","url":"http://example.com","license":"MIT","proofing":true,"source":"<html>{{STORY_DATA}}</html>"}""";
            Assert.Throws<Exception>(() => StoryFormat.Parse(json));
        }
    }
}