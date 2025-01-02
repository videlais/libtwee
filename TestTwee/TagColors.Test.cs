using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class TagColorsTests
    {
        private TagColors _tagColors;

        [SetUp]
        public void SetUp()
        {
            _tagColors = new TagColors();
        }

        [Test]
        public void Constructor_CreatesEmptyDictionary()
        {
            Assert.That(_tagColors, Is.Not.Null);
            Assert.That(_tagColors.Count, Is.EqualTo(0));
        }

        [Test]
        public void AddTag_AddsTag()
        {
            _tagColors.AddTag("tag", "color");
            Assert.That(_tagColors.HasTag("tag"));
        }

        [Test]
        public void HasTag_ReturnsFalse_WhenTagIsNotPresent()
        {
            Assert.That(_tagColors.HasTag("tag"), Is.False);
        }

        [Test]
        public void HasTag_ReturnsTrue_WhenTagIsPresent()
        {
            _tagColors.AddTag("tag", "color");
            Assert.That(_tagColors.HasTag("tag"));
        }

        [Test]
        public void GetColor_ReturnsNull_WhenTagIsNotPresent()
        {
            Assert.That(_tagColors.GetColor("tag"), Is.Null);
        }

        [Test]
        public void GetColor_ReturnsColor_WhenTagIsPresent()
        {
            _tagColors.AddTag("tag", "color");
            Assert.That(_tagColors.GetColor("tag"), Is.EqualTo("color"));
        }

        [Test]
        public void RemoveTag_RemovesTag()
        {
            _tagColors.AddTag("tag", "color");
            _tagColors.RemoveTag("tag");
            Assert.That(_tagColors.HasTag("tag"), Is.False);
        }

        [Test]
        public void Count_ReturnsNumberOfTags()
        {
            _tagColors.AddTag("tag", "color");
            Assert.That(_tagColors.Count, Is.EqualTo(1));
        }

        [Test]
        public void Count_ReturnsZero_WhenNoTags()
        {
            Assert.That(_tagColors.Count, Is.EqualTo(0));
        }

        [Test]
        public void Clear_RemovesAllTags()
        {
            _tagColors.AddTag("tag", "color");
            _tagColors.Clear();
            Assert.That(_tagColors.Count, Is.EqualTo(0));
        }

        [Test]
        public void ToTwine2HTML_ReturnsEmptyString_WhenNoTags()
        {
            Assert.That(_tagColors.ToTwine2HTML(), Is.EqualTo(""));
        }

        [Test]
        public void ToTwine2HTML_ReturnsTagColors()
        {
            _tagColors.AddTag("tag", "color");
            Assert.That(_tagColors.ToTwine2HTML(), Is.EqualTo("<tw-tag name=\"tag\" color=\"color\" />\n"));
        }

        [Test]
        public void ToTwine2HTML_ReturnsMultipleTagColors()
        {
            _tagColors.AddTag("tag1", "color1");
            _tagColors.AddTag("tag2", "color2");
            Assert.That(_tagColors.ToTwine2HTML(), Is.EqualTo("<tw-tag name=\"tag1\" color=\"color1\" />\n<tw-tag name=\"tag2\" color=\"color2\" />\n"));
        }

        [Test]
        public void ToString_ReturnsEmptyDictionary_WhenNoTags()
        {
            Assert.That(_tagColors.ToString(), Is.EqualTo("{}"));
        }

        [Test]
        public void ToString_ReturnsTagColors()
        {
            _tagColors.AddTag("tag", "color");
            Assert.That(_tagColors.ToString(), Is.EqualTo("{\"tag\":\"color\"}"));
        }
    }
}