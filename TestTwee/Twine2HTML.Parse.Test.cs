using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class ParseTwine2HTMLTest
    {
        [Test]
        public void TestParseTwine2HTML()
        {
            string html = @"
                <tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                </tw-storydata>
            ";
            Story story = Twine2HTML.Parse(html);
            Assert.Multiple(() =>
            {
                Assert.That(story.Name, Is.EqualTo("DocumentationExample"));
                Assert.That(story.IFID, Is.EqualTo("6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC"));
                Assert.That(story.Creator, Is.EqualTo("Twine"));
                Assert.That(story.CreatorVersion, Is.EqualTo("2.3.3"));
                Assert.That(story.Zoom, Is.EqualTo(1));
            });
        }

        [Test]
        public void TestParseTwine2HTMLMissingStoryData()
        {
            string html = @"
                <note>
                    ...
                </note>
            ";
            Assert.That(() => Twine2HTML.Parse(html), Throws.Exception.With.Message.EqualTo("The document does not contain a <tw-storydata> element."));
        }

        [Test]
        public void TestParseTwine2HTML_Name()
        {
            string html = @"
                <tw-storydata
                    name='DocumentationExample'
                </tw-storydata>
            ";
            Assert.That(Twine2HTML.Parse(html).Name, Is.EqualTo("DocumentationExample"));
        }

        [Test]
        public void TestParseTwine2HTML_IFID()
        {
            string html = @"
                <tw-storydata
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                </tw-storydata>
            ";
            Assert.That(Twine2HTML.Parse(html).IFID, Is.EqualTo("6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC"));
        }

        [Test]
        public void TestParseTwine2HTML_Creator()
        {
            string html = @"
                <tw-storydata
                    creator='Twine'
                </tw-storydata>
            ";
            Assert.That(Twine2HTML.Parse(html).Creator, Is.EqualTo("Twine"));
        }

        [Test]
        public void TestParseTwine2HTML_CreatorVersion()
        {
            string html = @"
                <tw-storydata
                    creator-version='2.3.3'
                </tw-storydata>
            ";
            Assert.That(Twine2HTML.Parse(html).CreatorVersion, Is.EqualTo("2.3.3"));
        }

        [Test]
        public void TestParseTwine2HTML_Zoom()
        {
            string html = @"
                <tw-storydata
                    zoom='1'
                </tw-storydata>
            ";
            Assert.That(Twine2HTML.Parse(html).Zoom, Is.EqualTo(1));
        }

        [Test]
        public void TestParseTwine2HTML_Format()
        {
            string html = @"
                <tw-storydata
                    format='Harlowe'
                </tw-storydata>
            ";
            Assert.That(Twine2HTML.Parse(html).Format, Is.EqualTo("Harlowe"));
        }

        [Test]
        public void TestParseTwine2HTML_FormatVersion()
        {
            string html = @"
                <tw-storydata
                    format-version='3.0.2'
                </tw-storydata>
            ";
            Assert.That(Twine2HTML.Parse(html).FormatVersion, Is.EqualTo("3.0.2"));
        }

        [Test]
        public void TestParseTwine2HTML_StartNode_MissingPassageData()
        {
            string html = @"
                <tw-storydata
                    startnode='1'
                </tw-storydata>
            ";
            Assert.That(Twine2HTML.Parse(html).Start, Is.EqualTo(""));
        }

        [Test]
        public void TestParseTwine2HTML_StartNodeEmpty()
        {
            string html = @"
                <tw-storydata
                    startnode=''
                </tw-storydata>
            ";
            Assert.That(Twine2HTML.Parse(html).Start, Is.EqualTo(""));
        }

        [Test]
        public void TestParseTwine2HTML_StartNode()
        {
            string html = @"
                <tw-storydata
                    startnode='1'
                </tw-storydata>
                <tw-passagedata
                    pid='1'
                    name='Start'
                </tw-passagedata>
            ";
            Assert.That(Twine2HTML.Parse(html).Start, Is.EqualTo("Start"));
        }

        [Test]
        public void TestParseTwine2HTML_StartNode_MissingName()
        {
            string html = @"
                <tw-storydata
                    startnode='1'
                </tw-storydata>
                <tw-passagedata
                    pid='1'
                </tw-passagedata>
            ";
            Assert.That(Twine2HTML.Parse(html).Start, Is.EqualTo(""));
        }

        [Test]
        public void TestParseTwine2HTML_StartNode_MissingPID()
        {
            string html = @"
                <tw-storydata
                    startnode='1'
                </tw-storydata>
                <tw-passagedata
                    name='Start'
                </tw-passagedata>
            ";
            Assert.That(Twine2HTML.Parse(html).Start, Is.EqualTo(""));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'>
                </tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            Assert.That(story.Passages, Has.Count.EqualTo(1));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_Name()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'>
                </tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            Assert.That(story.Passages[0].Name, Is.EqualTo("Start"));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_Tags()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'
                    tags='tag1 tag2 tag3'>
                </tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            Assert.That(story.Passages[0].Tags, Is.EqualTo(new string[] { "tag1", "tag2", "tag3" }));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_TagsEmpty()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'
                    tags=''>
                </tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            Assert.That(story.Passages[0].Tags, Is.EqualTo(new string[] { }));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_TagsMissing()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'>
                </tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            Assert.That(story.Passages[0].Tags, Is.EqualTo(new string[] { }));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_Text()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'>Text</tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            Assert.That(story.Passages[0].Text, Is.EqualTo("Text"));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_TextEmpty()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'></tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            Assert.That(story.Passages[0].Text, Is.EqualTo(""));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_TextMissing()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'></tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            Assert.That(story.Passages[0].Text, Is.EqualTo(""));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_Size()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'
                    size='100,100'>
                </tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            Assert.That(story.Passages[0].Metadata["size"], Is.EqualTo("100,100"));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_SizeEmpty()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'
                    size=''>
                </tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            Assert.That(story.Passages[0].Metadata["size"], Is.EqualTo(""));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_SizeMissing()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'>
                </tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            // Assert that 'size' is not in the metadata
            Assert.That(story.Passages[0].Metadata.ContainsKey("size"), Is.False);
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_Position()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'
                    position='100,100'>
                </tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            Assert.That(story.Passages[0].Metadata["position"], Is.EqualTo("100,100"));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_PositionEmpty()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'
                    position=''>
                </tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            Assert.That(story.Passages[0].Metadata["position"], Is.EqualTo(""));
        }

        [Test]
        public void TestParseTwine2HTML_ParsePassageData_PositionMissing()
        {
            string html = @"<tw-storydata
                    name='DocumentationExample'
                    startnode='1'
                    creator='Twine'
                    creator-version='2.3.3'
                    ifid='6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC'
                    zoom='1'
                    format='Harlowe'
                    format-version='3.0.2'>
                <tw-passagedata
                    pid='1'
                    name='Start'>
                </tw-passagedata>
            </tw-storydata>";
            Story story = Twine2HTML.Parse(html);
            // Assert that 'position' is not in the metadata
            Assert.That(story.Passages[0].Metadata.ContainsKey("position"), Is.False);
        }

    }
}