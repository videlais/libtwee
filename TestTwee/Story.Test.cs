using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using NUnit.Framework;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class StoryTest
    {
        private Story _story;

        [SetUp]
        public void SetUp()
        {
            _story = new Story();
        }

        [Test]
        public void TestAddPassage()
        {
            Passage passage = new ("TestPassage", "This is a test passage.");
            
            int count = _story.AddPassage(passage);
            
            Assert.Multiple(() =>
            {
                Assert.That(count, Is.EqualTo(1));
                Assert.That(_story.Count, Is.EqualTo(1));
                Assert.That(passage, Is.EqualTo(_story.GetPassageByName("TestPassage")));
            });

        }

        [Test]
        public void TestAddPassage_Multiple()
        {
            Passage passage = new ("TestPassage", "This is a test passage.");
            Passage passage1 = new ("TestPassage1", "This is a test passage.");
            
            _story.AddPassage(passage);
            _story.AddPassage(passage1);
            
            Assert.That(_story.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestAddDuplicatePassage()
        {
            Passage passage = new ("TestPassage", "This is a test passage.");
            _story.AddPassage(passage);
            _story.AddPassage(passage);
            Assert.That(_story.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestRemovePassageByName()
        {
            Passage passage = new ("TestPassage", "This is a test passage.");
            _story.AddPassage(passage);
            _story.RemovePassageByName("TestPassage");
            Assert.That(_story.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestGetPassagesByTag()
        {
            Passage passage1 = new("Passage1", "This is a test passage.");
            passage1.AddTag("tag1");
            
            Passage passage2 = new("Passage2", "This is another test passage.");
            passage2.AddTag("tag2");
            
            _story.AddPassage(passage1);
            _story.AddPassage(passage2);
            
            List<Passage> passages = _story.GetPassagesByTag("tag1");
            
            Assert.That(passages.Count, Is.EqualTo(1));

        }

        [Test]
        public void TestStoryDataPassage()
        {
            _story.IFID = "12345";
            _story.Start = "StartPassage";
            _story.Format = "Twine";
            _story.FormatVersion = "2.0";
            _story.Zoom = 2;
            Assert.Multiple(() =>
            {
                Assert.That(_story.IFID, Is.EqualTo("12345"));
                Assert.That(_story.Start, Is.EqualTo("StartPassage"));
                Assert.That(_story.Format, Is.EqualTo("Twine"));
                Assert.That(_story.FormatVersion, Is.EqualTo("2.0"));
                Assert.That(_story.Zoom, Is.EqualTo(2));
            });

        }

        [Test]
        public void TestStartPassage()
        {
           _story.Start = "Start";
            Assert.That(_story.Start, Is.EqualTo("Start"));
        }

        [Test]
        public void TestCreatorPassage()
        {
            _story.Creator = "Author";
            Assert.That(_story.Creator, Is.EqualTo("Author"));
        }

        [Test]
        public void TestCreatorVersionPassage()
        {
            _story.CreatorVersion = "1.0";
            Assert.That(_story.CreatorVersion, Is.EqualTo("1.0"));
        }

        [Test]
        public void TestFormatPassage()
        {
            _story.Format = "Twine";
            Assert.That(_story.Format, Is.EqualTo("Twine"));
        }

        [Test]
        public void TestFormatVersionPassage()
        {
            _story.FormatVersion = "2.0";
            Assert.That(_story.FormatVersion, Is.EqualTo("2.0"));
        }

        [Test]
        public void TestZoomPassage()
        {
            _story.Zoom = 2;
            Assert.That(_story.Zoom, Is.EqualTo(2));
        }

        [Test]
        public void TestToTwine1HTML_NoPassages() {
            string html = _story.ToTwine1HTML();
            Assert.That(html, Is.EqualTo("<div id=\"storeArea\" data-size=\"0\"></div>"));
        }

        [Test]
        public void TestToTwine1HTML_OnePassage() {
            Passage passage = new ("TestPassage", "This is a test passage.");
            
            _story.AddPassage(passage);
            
            string html = _story.ToTwine1HTML();
            
            Assert.That(html, Is.EqualTo("<div id=\"storeArea\" data-size=\"1\"><div tiddler=\"TestPassage\" tags=\"\" modifier=\"extwee\" twine-position=\"10,10\">This is a test passage.</div></div>"));
        }

        [Test]
        public void TestToTwine1HTML_TwoPassages() {
            Passage passage1 = new ("TestPassage1", "This is a test passage.");
            Passage passage2 = new ("TestPassage2", "This is another test passage.");
            
            _story.AddPassage(passage1);
            _story.AddPassage(passage2);
           
            string html = _story.ToTwine1HTML();
            
            Assert.That(html, Is.EqualTo("<div id=\"storeArea\" data-size=\"2\"><div tiddler=\"TestPassage1\" tags=\"\" modifier=\"extwee\" twine-position=\"10,10\">This is a test passage.</div><div tiddler=\"TestPassage2\" tags=\"\" modifier=\"extwee\" twine-position=\"10,10\">This is another test passage.</div></div>"));
        }

        [Test]
        public void TestToTwee_Default() {
            Passage passage = new ("TestPassage", "This is a test passage.");
            // Add a passage to the story.
            _story.AddPassage(passage);
            // Add a name to the story.
            _story.Name = "TestStory";
            // Add a start passage to the story.
            _story.Start = "TestPassage";
            // Set a test value for the IFID.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";


            string expectedTwee = $":: StoryData\n" +
                                 "{\n" +
                                    "  \"ifid\": \"A77C26C0-C331-11EF-A8FA-0800200C9A66\",\n" +
                                    "  \"start\": \"TestPassage\",\n" +
                                    "  \"zoom\": 1\n" +
                                 "}" +
                                 "\n\n" +
                                 ":: StoryTitle\n" +
                                 "TestStory\n\n" +
                                 ":: TestPassage\n" +
                                 "This is a test passage.\n\n";
            Assert.That(_story.ToTwee(), Is.EqualTo(expectedTwee));
        }

        [Test]
        public void TestToTwee_NoIFID() {
            // Add a passage to the story.
            Passage passage = new ("TestPassage", "This is a test passage.");
            _story.AddPassage(passage);
            // Add a name to the story.
            _story.Name = "TestStory";
            // Add a start passage to the story.
            _story.Start = "TestPassage";

            // Generate Twee output, which will automatically generate an IFID.
            _story.ToTwee();

            // Assert that an IFID was generated.
            Assert.That(_story.IFID, Is.Not.Null);
        }

        [Test]
        public void TestToTwee_NoStart() {
            // Add a passage to the story.
            Passage passage = new ("TestPassage", "This is a test passage.");
            _story.AddPassage(passage);
            // Add a name to the story.
            _story.Name = "TestStory";
            // Set a test value for the IFID.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";

            // Generate Twee output, which will automatically set the start passage.
            _story.ToTwee();

            // Assert that the start passage was set to the first passage.
            Assert.That(_story.Start, Is.EqualTo(""));
        }

        [Test]
        public void TestToTwee_NoName() {
            // Add a passage to the story.
            Passage passage = new ("TestPassage", "This is a test passage.");
            _story.AddPassage(passage);
            // Add a start passage to the story.
            _story.Start = "TestPassage";
            // Set a test value for the IFID.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";

            // Generate Twee output, which will automatically set the name.
            _story.ToTwee();

            // Assert that the name was set to "Untitled".
            Assert.That(_story.Name, Is.EqualTo("Untitled"));
        }

        [Test]
        public void TestToTwee_NoPassages() {
            // Add a name to the story.
            _story.Name = "TestStory";
            // Add a start passage to the story.
            _story.Start = "TestPassage";
            // Set a test value for the IFID.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";

            string expectedTwee = $":: StoryData\n" +
                                 "{\n" +
                                    "  \"ifid\": \"A77C26C0-C331-11EF-A8FA-0800200C9A66\",\n" +
                                    "  \"start\": \"TestPassage\",\n" +
                                    "  \"zoom\": 1\n" +
                                 "}" +
                                 "\n\n" +
                                 ":: StoryTitle\n" +
                                 "TestStory\n\n";
            Assert.That(_story.ToTwee(), Is.EqualTo(expectedTwee));
        }

        [Test]
        public void TestToTwee_WithFormat() {
            // Add a passage to the story.
            Passage passage = new ("TestPassage", "This is a test passage.");
            _story.AddPassage(passage);
            // Add a name to the story.
            _story.Name = "TestStory";
            // Add a start passage to the story.
            _story.Start = "TestPassage";
            // Set a test value for the IFID.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";
            // Set the format of the story.
            _story.Format = "Twine";

            string expectedTwee = $":: StoryData\n" +
                                 "{\n" +
                                    "  \"ifid\": \"A77C26C0-C331-11EF-A8FA-0800200C9A66\",\n" +
                                    "  \"format\": \"Twine\",\n" +
                                    "  \"start\": \"TestPassage\",\n" +
                                    "  \"zoom\": 1\n" +
                                 "}" +
                                 "\n\n" +
                                 ":: StoryTitle\n" +
                                 "TestStory\n\n" +
                                 ":: TestPassage\n" +
                                 "This is a test passage.\n\n";
            Assert.That(_story.ToTwee(), Is.EqualTo(expectedTwee));
        }

        [Test]
        public void TestToTwee_WithFormatVersion() {
            // Add a passage to the story.
            Passage passage = new ("TestPassage", "This is a test passage.");
            _story.AddPassage(passage);
            // Add a name to the story.
            _story.Name = "TestStory";
            // Add a start passage to the story.
            _story.Start = "TestPassage";
            // Set a test value for the IFID.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";
            // Set the format of the story.
            _story.Format = "Twine";
            // Set the format version of the story.
            _story.FormatVersion = "2.0";

            string expectedTwee = $":: StoryData\n" +
                                 "{\n" +
                                    "  \"ifid\": \"A77C26C0-C331-11EF-A8FA-0800200C9A66\",\n" +
                                    "  \"format\": \"Twine\",\n" +
                                    "  \"format-version\": \"2.0\",\n" +
                                    "  \"start\": \"TestPassage\",\n" +
                                    "  \"zoom\": 1\n" +
                                 "}" +
                                 "\n\n" +
                                 ":: StoryTitle\n" +
                                 "TestStory\n\n" +
                                 ":: TestPassage\n" +
                                 "This is a test passage.\n\n";
            Assert.That(_story.ToTwee(), Is.EqualTo(expectedTwee));
        }

        [Test]
        public void TestToTwee_WithTagColors() {
            // Add a passage to the story.
            Passage passage = new ("TestPassage", "This is a test passage.");
            _story.AddPassage(passage);
            // Add a name to the story.
            _story.Name = "TestStory";
            // Add a start passage to the story.
            _story.Start = "TestPassage";
            // Set a test value for the IFID.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";
            // Set the tag colors of the story.
            _story.TagColors.AddTag("tag1", "#FFFFFF");
            _story.TagColors.AddTag("tag2", "#000000");

            string expectedTwee = $":: StoryData\n" +
                                 "{\n" +
                                    "  \"ifid\": \"A77C26C0-C331-11EF-A8FA-0800200C9A66\",\n" +
                                    "  \"start\": \"TestPassage\",\n" +
                                    "  \"tag-colors\": {\n" +
                                    "    \"tag1\": \"#FFFFFF\",\n" +
                                    "    \"tag2\": \"#000000\"\n" +
                                    "  },\n" +
                                    "  \"zoom\": 1\n" +
                                 "}" +
                                 "\n\n" +
                                 ":: StoryTitle\n" +
                                 "TestStory\n\n" +
                                 ":: TestPassage\n" +
                                 "This is a test passage.\n\n";
            Assert.That(_story.ToTwee(), Is.EqualTo(expectedTwee));
        }

        [Test]
        public void TestToTwine2HTML_Default() {
            // Set IFID to a default value for testing.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";
            string html = _story.ToTwine2HTML();
            Assert.That(html, Is.EqualTo("<tw-storydata name=\"Untitled\" ifid=\"A77C26C0-C331-11EF-A8FA-0800200C9A66\" startnode=\"0\" zoom=\"1\" options hidden>\n</tw-storydata>"));
        }

        [Test]
        public void TestToTwine2HTML_WithStart() {
            // Set IFID to a default value for testing.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";
            _story.Start = "TestPassage";
            // Create a passage to add to the story.
            Passage passage = new ("TestPassage", "This is a test passage.");
            _story.AddPassage(passage);
            string html = _story.ToTwine2HTML();
            Assert.That(html, Is.EqualTo("<tw-storydata name=\"Untitled\" ifid=\"A77C26C0-C331-11EF-A8FA-0800200C9A66\" startnode=\"1\" zoom=\"1\" options hidden>\n<tw-passagedata pid=\"1\" name=\"TestPassage\" tags=\"\">This is a test passage.</tw-passagedata></tw-storydata>"));
        }

        [Test]
        public void TestToTwine2HTML_WithStartAndName() {
            // Set IFID to a default value for testing.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";
            _story.Start = "TestPassage";
            _story.Name = "TestStory";
            // Create a passage to add to the story.
            Passage passage = new ("TestPassage", "This is a test passage.");
            _story.AddPassage(passage);
            string html = _story.ToTwine2HTML();
            Assert.That(html, Is.EqualTo("<tw-storydata name=\"TestStory\" ifid=\"A77C26C0-C331-11EF-A8FA-0800200C9A66\" startnode=\"1\" zoom=\"1\" options hidden>\n<tw-passagedata pid=\"1\" name=\"TestPassage\" tags=\"\">This is a test passage.</tw-passagedata></tw-storydata>"));
        }

        [Test]
        public void TestToTwine2HTML_WithStartAndNameAndZoom() {
            // Set IFID to a default value for testing.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";
            _story.Start = "TestPassage";
            _story.Name = "TestStory";
            _story.Zoom = 2;
            // Create a passage to add to the story.
            Passage passage = new ("TestPassage", "This is a test passage.");
            _story.AddPassage(passage);
            string html = _story.ToTwine2HTML();
            Assert.That(html, Is.EqualTo("<tw-storydata name=\"TestStory\" ifid=\"A77C26C0-C331-11EF-A8FA-0800200C9A66\" startnode=\"1\" zoom=\"2\" options hidden>\n<tw-passagedata pid=\"1\" name=\"TestPassage\" tags=\"\">This is a test passage.</tw-passagedata></tw-storydata>"));
        }

        [Test]
        public void TestToTwine2HTML_WithStartAndNameAndZoomAndPassageTag() {
            // Set IFID to a default value for testing.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";
            _story.Start = "TestPassage";
            _story.Name = "TestStory";
            _story.Zoom = 2;
            // Create a passage to add to the story.
            Passage passage = new ("TestPassage", "This is a test passage.");
            passage.AddTag("tag1");
            _story.AddPassage(passage);
            string html = _story.ToTwine2HTML();
            Assert.That(html, Is.EqualTo("<tw-storydata name=\"TestStory\" ifid=\"A77C26C0-C331-11EF-A8FA-0800200C9A66\" startnode=\"1\" zoom=\"2\" options hidden>\n<tw-passagedata pid=\"1\" name=\"TestPassage\" tags=\"tag1\">This is a test passage.</tw-passagedata></tw-storydata>"));
        }

        [Test]
        public void TestToTwine2HTML_WithStartAndNameAndZoomAndPassageTagAndTagColors() {
            // Set IFID to a default value for testing.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";
            _story.Start = "TestPassage";
            _story.Name = "TestStory";
            _story.Zoom = 2;
            // Create a passage to add to the story.
            Passage passage = new ("TestPassage", "This is a test passage.");
            passage.AddTag("tag1");
            _story.AddPassage(passage);
            // Set the tag colors of the story.
            _story.TagColors.AddTag("tag1", "#FFFFFF");
            string html = _story.ToTwine2HTML();
            Assert.That(html, Is.EqualTo("<tw-storydata name=\"TestStory\" ifid=\"A77C26C0-C331-11EF-A8FA-0800200C9A66\" startnode=\"1\" zoom=\"2\" options hidden>\n<tw-tag name=\"tag1\" color=\"#FFFFFF\" />\n<tw-passagedata pid=\"1\" name=\"TestPassage\" tags=\"tag1\">This is a test passage.</tw-passagedata></tw-storydata>"));
        }

        [Test]
        public void TestToTwine2HTML_WithStartAndNameAndZoomAndPassageTagAndTagColorsAndCreator() {
            // Set IFID to a default value for testing.
            _story.IFID = "A77C26C0-C331-11EF-A8FA-0800200C9A66";
            _story.Start = "TestPassage";
            _story.Name = "TestStory";
            _story.Zoom = 2;
            // Create a passage to add to the story.
            Passage passage = new ("TestPassage", "This is a test passage.");
            passage.AddTag("tag1");
            _story.AddPassage(passage);
            // Set the tag colors of the story.
            _story.TagColors.AddTag("tag1", "#FFFFFF");
            // Set the creator of the story.
            _story.Creator = "Author";
            string html = _story.ToTwine2HTML();
            Assert.That(html, Is.EqualTo("<tw-storydata name=\"TestStory\" ifid=\"A77C26C0-C331-11EF-A8FA-0800200C9A66\" startnode=\"1\" creator=\"Author\" zoom=\"2\" options hidden>\n<tw-tag name=\"tag1\" color=\"#FFFFFF\" />\n<tw-passagedata pid=\"1\" name=\"TestPassage\" tags=\"tag1\">This is a test passage.</tw-passagedata></tw-storydata>"));
        }
    }
}