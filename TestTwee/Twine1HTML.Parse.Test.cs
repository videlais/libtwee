using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class ParseTwine1HTMLTests
    {
        [Test]
        public void TestParse_InvalidHTML()
        {
            string html = "";
            Assert.Throws<MissingHTMLElementException>(() => Twine1HTML.Parse(html));
        }

        [Test]
        public void TestParse_ValidHTML()
        {
            string html = "<div id=\"storeArea\"><div tiddler=\"Start\" tags=\"\" created=\"202306020121\" modifier=\"twee\" twine-position=\"10,10\">[[One passage]]</div></div>";
            Story story = Twine1HTML.Parse(html);
            Assert.Multiple(() =>
            {
                Assert.That(story.Passages, Has.Count.EqualTo(1));
                Assert.That(story.Passages[0].Name, Is.EqualTo("Start"));
                Assert.That(story.Passages[0].Text, Is.EqualTo("[[One passage]]"));
            });
        }

        [Test]
        public void TestParse_MissingStoreArea()
        {
            string html = "<div id=\"storeArea\"></div>";
            Assert.Throws<MissingHTMLElementException>(() => Twine1HTML.Parse(html));
        }

        [Test]
        public void TestParse_MissingTiddler()
        {
            string html = "<div id=\"storeArea\"><div></div></div>";
            Assert.Throws<MissingHTMLElementException>(() => Twine1HTML.Parse(html));
        }

        [Test]
        public void TestParse_VerifyPassage()
        {
            string html = "<div id=\"storeArea\"><div tiddler=\"Start\" tags=\"\" created=\"202306020121\" modifier=\"twee\" twine-position=\"10,10\" modified=\"202306020121\">[[One passage]]</div></div>";
            Story story = Twine1HTML.Parse(html);
            Passage passage = story.Passages[0];
            Assert.Multiple(() =>
            {
                Assert.That(passage.Name, Is.EqualTo("Start"));
                Assert.That(passage.Tags, Has.Count.EqualTo(0));
                Assert.That(passage.Text, Is.EqualTo("[[One passage]]"));
                Assert.That(passage.Metadata["created"], Is.EqualTo("202306020121"));
                Assert.That(passage.Metadata["modifier"], Is.EqualTo("twee"));
                Assert.That(passage.Metadata["position"], Is.EqualTo("10,10"));
                Assert.That(passage.Metadata["modified"], Is.EqualTo("202306020121"));
            });
        }

        [Test]
        public void Parse_Verify_CSS()
        {
            string html = "<style id=\"StoryCSS\">body { background-color: black; }</style><div id=\"storeArea\"><div tiddler=\"Start\" tags=\"\" created=\"202306020121\" modifier=\"twee\" twine-position=\"10,10\" modified=\"202306020121\">[[One passage]]</div></div>";
            Story story = Twine1HTML.Parse(html);
            Assert.That(story.StoryStylesheets[0], Is.EqualTo("body { background-color: black; }"));
        }

    }
}