using NUnit.Framework;
using System;
using System.Collections.Generic;
using libtwee;

namespace TestTwee
{
    [TestFixture]
    public class ParseTwine2JSONTest
    {

        [Test]
        public void TestParseTwine2JSON_InvalidDocument()
        {
            string json = "invalid json";
            Assert.Throws<FormatException>(() => Twine2JSON.Parse(json));
        }

        [Test]
        public void TestParseTwine2JSON_ValidDocument_NoPassages()
        {
            string json = @"
            {
                ""name"": ""Example"",
                ""ifid"": ""D674C58C-DEFA-4F70-B7A2-27742230C0FC"",
                ""format"": ""Snowman"",
                ""format-version"": ""3.0.2"",
                ""start"": ""My Starting Passage"",
                ""tag-colors"": {
                    ""bar"": ""Green"",
                    ""foo"": ""red"",
                    ""qaz"": ""blue""
                },
                ""zoom"": 0.25,
                ""creator"": ""Twine"",
                ""creator-version"": ""2.8"",
                ""style"": """",
                ""script"": """"
            }
            ";
            Story story = Twine2JSON.Parse(json);
            Assert.Multiple(() =>
            {
                Assert.That(story.Name, Is.EqualTo("Example"));
                Assert.That(story.IFID, Is.EqualTo("D674C58C-DEFA-4F70-B7A2-27742230C0FC"));
                Assert.That(story.Format, Is.EqualTo("Snowman"));
                Assert.That(story.FormatVersion, Is.EqualTo("3.0.2"));
                Assert.That(story.Start, Is.EqualTo("My Starting Passage"));
                Assert.That(story.Zoom, Is.EqualTo(0.25));
                Assert.That(story.Creator, Is.EqualTo("Twine"));
                Assert.That(story.CreatorVersion, Is.EqualTo("2.8"));
                Assert.That(story.StoryStylesheets[0].ToString(), Is.EqualTo(""));
                Assert.That(story.StoryScripts[0].ToString(), Is.EqualTo(""));
            });
        }

        [Test]
        public void TestParseTwine2JSON_ValidDocument_EmptyPassages()
        {
            string json = @"
            {
                ""name"": ""Example"",
                ""ifid"": ""D674C58C-DEFA-4F70-B7A2-27742230C0FC"",
                ""format"": ""Snowman"",
                ""format-version"": ""3.0.2"",
                ""start"": ""My Starting Passage"",
                ""tag-colors"": {
                    ""bar"": ""Green"",
                    ""foo"": ""red"",
                    ""qaz"": ""blue""
                },
                ""zoom"": 0.25,
                ""creator"": ""Twine"",
                ""creator-version"": ""2.8"",
                ""style"": """",
                ""script"": """",
                ""passages"": []
            }
            ";
            Story story = Twine2JSON.Parse(json);
            Assert.That(story.Passages, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestParseTwine2JSON_ValidDocument_InvalidPassage()
        {
            string json = @"
            {
                ""name"": ""Example"",
                ""ifid"": ""D674C58C-DEFA-4F70-B7A2-27742230C0FC"",
                ""format"": ""Snowman"",
                ""format-version"": ""3.0.2"",
                ""start"": ""My Starting Passage"",
                ""tag-colors"": {
                    ""bar"": ""Green"",
                    ""foo"": ""red"",
                    ""qaz"": ""blue""
                },
                ""zoom"": 0.25,
                ""creator"": ""Twine"",
                ""creator-version"": ""2.8"",
                ""style"": """",
                ""script"": """",
                ""passages"": ""invalid""
            }
            ";
            Story story = Twine2JSON.Parse(json);
            Assert.That(story.Passages, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestParseTwine2JSON_ValidDocument_SinglePassage()
        {
            string json = @"
            {
                ""name"": ""Example"",
                ""ifid"": ""D674C58C-DEFA-4F70-B7A2-27742230C0FC"",
                ""format"": ""Snowman"",
                ""format-version"": ""3.0.2"",
                ""start"": ""My Starting Passage"",
                ""tag-colors"": {
                    ""bar"": ""Green"",
                    ""foo"": ""red"",
                    ""qaz"": ""blue""
                },
                ""zoom"": 0.25,
                ""creator"": ""Twine"",
                ""creator-version"": ""2.8"",
                ""style"": """",
                ""script"": """",
                ""passages"": [
                    {
                        ""name"": ""My Starting Passage"",
                        ""tags"": [""foo"", ""bar""],
                        ""text"": ""This is the content of the passage.""
                    }
                ]
            }
            ";
            Story story = Twine2JSON.Parse(json);

            Assert.Multiple(() =>
            {
                Assert.That(story.Passages, Has.Count.EqualTo(1));
                Assert.That(story.Passages[0].Name, Is.EqualTo("My Starting Passage"));
                Assert.That(story.Passages[0].Tags, Has.Count.EqualTo(2));
                var tagsList = new List<string>(story.Passages[0].Tags);
                Assert.That(tagsList[0], Is.EqualTo("foo"));
                Assert.That(tagsList[1], Is.EqualTo("bar"));
                Assert.That(story.Passages[0].Text, Is.EqualTo("This is the content of the passage."));
            });
        }

        [Test]
        public void TestParseTwine2JSON_Passages_Null()
        {
            string json = @"
            {
                ""name"": ""Example"",
                ""ifid"": ""D674C58C-DEFA-4F70-B7A2-27742230C0FC"",
                ""format"": ""Snowman"",
                ""format-version"": ""3.0.2"",
                ""start"": ""My Starting Passage"",
                ""tag-colors"": {
                    ""bar"": ""Green"",
                    ""foo"": ""red"",
                    ""qaz"": ""blue""
                },
                ""zoom"": 0.25,
                ""creator"": ""Twine"",
                ""creator-version"": ""2.8"",
                ""style"": """",
                ""script"": """",
                ""passages"": null
            }
            ";
            Story story = Twine2JSON.Parse(json);
            Assert.That(story.Passages, Has.Count.EqualTo(0));
        }

        [Test]
        public void ParseTwine2JSON_MinimumValid_OnlyName()
        {
            string json = @"
            {
                ""name"": ""Example""
            }
            ";
            Story story = Twine2JSON.Parse(json);
            Assert.That(story.Name, Is.EqualTo("Example"));
        }

        [Test]
        public void ParseTwine2JSON_Minimum_MissingName()
        {
            string json = @"{}";
            Story story = Twine2JSON.Parse(json);
            Assert.That(story.Name, Is.EqualTo("Untitled"));
        }

        [Test]
        public void ParseTwine2JSON_MissingTagColors()
        {
            string json = @"
            {
                ""name"": ""Example"",
                ""ifid"": ""D674C58C-DEFA-4F70-B7A2-27742230C0FC"",
                ""format"": ""Snowman"",
                ""format-version"": ""3.0.2"",
                ""start"": ""My Starting Passage"",
                ""zoom"": 0.25,
                ""creator"": ""Twine"",
                ""creator-version"": ""2.8"",
                ""style"": """",
                ""script"": """"
            }
            ";
            Story story = Twine2JSON.Parse(json);
            Assert.That(story.TagColors, Has.Count.EqualTo(0));
        }

        [Test]
        public void ParseTwine2JSON_EmptyTagColors()
        {
            string json = @"
            {
                ""name"": ""Example"",
                ""ifid"": ""D674C58C-DEFA-4F70-B7A2-27742230C0FC"",
                ""format"": ""Snowman"",
                ""format-version"": ""3.0.2"",
                ""start"": ""My Starting Passage"",
                ""tag-colors"": {},
                ""zoom"": 0.25,
                ""creator"": ""Twine"",
                ""creator-version"": ""2.8"",
                ""style"": """",
                ""script"": """"
            }
            ";
            Story story = Twine2JSON.Parse(json);
            Assert.That(story.TagColors, Has.Count.EqualTo(0));
        }

        [Test]
        public void ParseTwine2JSON_TagColors_StringNotCollection()
        {
            string json = @"
            {
                ""name"": ""Example"",
                ""ifid"": ""D674C58C-DEFA-4F70-B7A2-27742230C0FC"",
                ""format"": ""Snowman"",
                ""format-version"": ""3.0.2"",
                ""start"": ""My Starting Passage"",
                ""tag-colors"": ""invalid"",
                ""zoom"": 0.25,
                ""creator"": ""Twine"",
                ""creator-version"": ""2.8"",
                ""style"": """",
                ""script"": """"
            }
            ";
            Story story = Twine2JSON.Parse(json);
            Assert.That(story.TagColors, Has.Count.EqualTo(0));
        }

        [Test]
        public void TestParseTwine2JSON_MissingName()
        {
            string json = @"
            {
                ""ifid"": ""D674C58C-DEFA-4F70-B7A2-27742230C0FC""
            }
            ";
            Story story = Twine2JSON.Parse(json);
            Assert.That(story.Name, Is.EqualTo("Untitled"));
        }

        [Test]
        public void TestParseTwine2JSON_NullName()
        {
            string json = @"
            {
                ""name"": null,
                ""ifid"": ""D674C58C-DEFA-4F70-B7A2-27742230C0FC""
            }
            ";
            Story story = Twine2JSON.Parse(json);
            Assert.That(story.Name, Is.EqualTo("Untitled"));
        }

        [Test]
        public void TestParseTwine2JSON_InvalidPassagesData()
        {
            string json = @"
            {
                ""name"": ""Test Story"",
                ""passages"": ""invalid data""
            }
            ";
            Story story = Twine2JSON.Parse(json);
            Assert.Multiple(() =>
            {
                Assert.That(story.Name, Is.EqualTo("Test Story"));
                Assert.That(story.Passages, Has.Count.EqualTo(0));
            });
        }

        [Test]
        public void TestParseTwine2JSON_NullPassages()
        {
            string json = @"
            {
                ""name"": ""Test Story"",
                ""passages"": null
            }
            ";
            Story story = Twine2JSON.Parse(json);
            Assert.Multiple(() =>
            {
                Assert.That(story.Name, Is.EqualTo("Test Story"));
                Assert.That(story.Passages, Has.Count.EqualTo(0));
            });
        }

        [Test]
        public void TestParseTwine2JSON_InvalidZoomValue()
        {
            string json = @"
            {
                ""name"": ""Test Story"",
                ""zoom"": ""invalid""
            }
            ";
            // This should throw an exception due to invalid zoom type
            Assert.Throws<InvalidOperationException>(() => Twine2JSON.Parse(json));
        }
    }
}