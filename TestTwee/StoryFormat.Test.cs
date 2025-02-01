using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class StoryFormatTests
    {
        private StoryFormat? _storyFormat;

        [SetUp]
        public void SetUp()
        {
            _storyFormat = new StoryFormat();
        }

        [Test]
        public void TestInitializeStoryFormat()
        {
            Assert.That(_storyFormat, Is.Not.Null);
        }

        [Test]
        public void TestDefaultValues()
        {
            Assert.Multiple(() =>
            {
                Assert.That(_storyFormat?.Name, Is.EqualTo("Untitled Story Format"));
                Assert.That(_storyFormat?.Version, Is.EqualTo("0.0.0"));
                Assert.That(_storyFormat?.Author, Is.EqualTo(""));
                Assert.That(_storyFormat?.Description, Is.EqualTo(""));
                Assert.That(_storyFormat?.Image, Is.EqualTo(""));
                Assert.That(_storyFormat?.Url, Is.EqualTo(""));
                Assert.That(_storyFormat?.License, Is.EqualTo(""));
                Assert.That(_storyFormat?.Proofing, Is.False);
                Assert.That(_storyFormat?.Source, Is.EqualTo(""));
            });
        }

        [Test]
        public void TestSetName()
        {
            if (_storyFormat != null)
            {
                _storyFormat.Name = "NewName";
            }
        }

        [Test]
        public void TestSetVersion()
        {
            if (_storyFormat != null)
            {
                _storyFormat.Version = "2.0";
                Assert.That(_storyFormat.Version, Is.EqualTo("2.0"));
            }
        }

        [Test]
        public void TestSetAuthor()
        {
            if (_storyFormat != null)
            {
                _storyFormat.Author = "NewAuthor";
            }
            Assert.That(_storyFormat?.Author, Is.EqualTo("NewAuthor"));
        }

        [Test]
        public void TestSetDescription()
        {
            if (_storyFormat != null)
            {
                _storyFormat.Description = "NewDescription";
                Assert.That(_storyFormat.Description, Is.EqualTo("NewDescription"));
            }
        }

        [Test]
        public void TestSetImage()
        {
            if (_storyFormat != null)
            {
                _storyFormat.Image = "NewImage";
                Assert.That(_storyFormat.Image, Is.EqualTo("NewImage"));
            }
        }

        [Test]
        public void TestSetUrl()
        {
            if (_storyFormat != null)
            {
                _storyFormat.Url = "NewUrl";
                Assert.That(_storyFormat.Url, Is.EqualTo("NewUrl"));
            }
        }

        [Test]
        public void TestSetLicense()
        {
            if (_storyFormat != null)
            {
                _storyFormat.License = "NewLicense";
                Assert.That(_storyFormat.License, Is.EqualTo("NewLicense"));
            }
        }

        [Test]
        public void TestSetProofing()
        {
            if (_storyFormat != null)
            {
                _storyFormat.Proofing = true;
            }
            Assert.That(_storyFormat?.Proofing, Is.True);
        }

        [Test]
        public void TestSetSource()
        {
            if (_storyFormat != null)
            {
                _storyFormat.Source = "NewSource";
            }
            Assert.That(_storyFormat?.Source, Is.EqualTo("NewSource"));
        }

        [Test]
        public void TestFromJson()
        {
            var json = "{\"name\":\"NewName\",\"version\":\"2.0\",\"author\":\"NewAuthor\",\"description\":\"NewDescription\",\"image\":\"NewImage\",\"url\":\"NewUrl\",\"license\":\"NewLicense\",\"proofing\":true,\"source\":\"NewSource\"}";
            var storyFormat = StoryFormat.FromJson(json);
            Assert.Multiple(() =>
            {
                Assert.That(storyFormat.Name, Is.EqualTo("NewName"));
                Assert.That(storyFormat.Version, Is.EqualTo("2.0"));
                Assert.That(storyFormat.Author, Is.EqualTo("NewAuthor"));
                Assert.That(storyFormat.Description, Is.EqualTo("NewDescription"));
                Assert.That(storyFormat.Image, Is.EqualTo("NewImage"));
                Assert.That(storyFormat.Url, Is.EqualTo("NewUrl"));
                Assert.That(storyFormat.License, Is.EqualTo("NewLicense"));
                Assert.That(storyFormat.Proofing, Is.True);
                Assert.That(storyFormat.Source, Is.EqualTo("NewSource"));
            });
        }

        [Test]
        public void TestToJson()
        {
            if (_storyFormat != null)
            {
                _storyFormat.Name = "NewName";
                _storyFormat.Version = "2.0";
                _storyFormat.Author = "NewAuthor";
                _storyFormat.Description = "NewDescription";
                _storyFormat.Image = "NewImage";
                _storyFormat.Url = "NewUrl";
                _storyFormat.License = "NewLicense";
                _storyFormat.Proofing = true;
                _storyFormat.Source = "NewSource";
            }

            var json = _storyFormat?.ToJson();
            Assert.That(json, Is.EqualTo("{\"name\":\"NewName\",\"version\":\"2.0\",\"author\":\"NewAuthor\",\"description\":\"NewDescription\",\"image\":\"NewImage\",\"url\":\"NewUrl\",\"license\":\"NewLicense\",\"proofing\":true,\"source\":\"NewSource\"}"));
        }

        [Test]
        public void TestWrite()
        {
            if (_storyFormat != null)
            {
                _storyFormat.Name = "NewName";
                _storyFormat.Version = "2.0";
                _storyFormat.Author = "NewAuthor";
                _storyFormat.Description = "NewDescription";
                _storyFormat.Image = "NewImage";
                _storyFormat.Url = "NewUrl";
                _storyFormat.License = "NewLicense";
                _storyFormat.Proofing = true;
                _storyFormat.Source = "NewSource";
            }

            var result = _storyFormat?.Write();
            Assert.That(result, Is.EqualTo("window.storyFormat({\"name\":\"NewName\",\"version\":\"2.0\",\"author\":\"NewAuthor\",\"description\":\"NewDescription\",\"image\":\"NewImage\",\"url\":\"NewUrl\",\"license\":\"NewLicense\",\"proofing\":true,\"source\":\"NewSource\"});"));
        }

    }
}