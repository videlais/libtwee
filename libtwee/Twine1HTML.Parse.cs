using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace libtwee
{
    public partial class Twine1HTML
    {
        /**
         * <summary>
         *   Parse a Twine 1 HTML document and return a Story object.
         *
         *  libtwee considers all input as a partial story. 
         *  It will only enforce required elements when producing output.
         *  (https://github.com/iftechfoundation/twine-specs/blob/master/twine-1-htmloutput-doc.md)
         * </summary>
         * 
         * @param html Twine 2 HTML.
         * @return A Story object.
         */
        public static Story Parse(string html) {
            // Create a new story object
            Story story = new();

            // Create a new HTML document
            HtmlDocument doc = new();

            // Load the HTML into the document
            doc.LoadHtml(html);

            // Does the document contain an element id="storeArea" or id="store-area"?
            var storeAreaNode = (doc.GetElementbyId("storeArea") ?? doc.GetElementbyId("store-area")) ?? throw new Exception("ERROR: The document Twine 1 HTML elements");

            // Find every element with the 'tiddler' attribute.
            var tiddlerNodes = storeAreaNode.SelectNodes("//*[@tiddler]") ?? throw new Exception("ERROR: The document does not contain any tiddler nodes.");

            /**
                <div 
                    tiddler="Start" 
                    tags="" 
                    created="202306020121" 
                    modifier="twee" 
                    modified="202306020121"
                    twine-position="10,10">[[One passage]]</div>
            */

            // Parse each tiddler node
            foreach (var tiddlerNode in tiddlerNodes) {
                // Create a new passage object
                Passage passage = new()
                {
                    // Parse the tiddler node
                    // tiddler: (string) Required. The name of the passage.
                    Name = tiddlerNode.GetAttributeValue("tiddler", string.Empty),

                    // tags: (string) Optional. A space-separated list of tags.
                    Tags = [.. tiddlerNode.GetAttributeValue("tags", string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries)],

                    // Parse the content of the tiddler node
                    // content: (string) Required. The content of the passage.
                    Text = tiddlerNode.InnerHtml
                };

                // Twine 1 passages may have additional attributes not supported in other formats:
                // - modifier: (string) Optional. Name of the tool that last edited the passage. Generally, for versions of Twine 1, this value will be "twee". Twee compilers may place their own name (e.g. "tweego" for Tweego).
                // - created: (string) Optional. The date and time the passage was created. The format is "YYYYMMDDHHMM".
                // - modified: (string) Optional. The date and time the passage was last modified. The format is "YYYYMMDDHHMM".
                // - twine-position: (string) Required. Comma-separated X and Y coordinates of the passage within Twine 1.
                // These are included as incoming metadata.

                // Does this element have the 'created' attribute?
                if (tiddlerNode.Attributes["created"] != null) {
                    // Add the 'created' attribute to the metadata
                    passage.Metadata.Add("created", tiddlerNode.GetAttributeValue("created", string.Empty));
                }

                // Does this element have the 'modifier' attribute?
                if (tiddlerNode.Attributes["modifier"] != null) {
                    // Add the 'modifier' attribute to the metadata
                    passage.Metadata.Add("modifier", tiddlerNode.GetAttributeValue("modifier", string.Empty));
                }

                // Does this element have the 'modified' attribute?
                if (tiddlerNode.Attributes["modified"] != null) {
                    // Add the 'modified' attribute to the metadata
                    passage.Metadata.Add("modified", tiddlerNode.GetAttributeValue("modified", string.Empty));
                }

                // Does this element have the 'twine-position' attribute?
                if (tiddlerNode.Attributes["twine-position"] != null) {
                    // Add the 'twine-position' attribute to the metadata
                    passage.Metadata.Add("position", tiddlerNode.GetAttributeValue("twine-position", string.Empty));
                }


                // Add the passage to the story
                story.Passages.Add(passage);
            }

            // The story stylesheet may be found in an element using the id attribute value of "storyCSS" or "story-style", depending on the story format.
            // Find the story stylesheet
            var storyCSSNode = doc.GetElementbyId("storyCSS") ?? doc.GetElementbyId("story-style");

            // Does the document contain a story stylesheet?
            if (storyCSSNode != null) {
                // Add the story stylesheet to the story
                story.StoryStylesheets.Add(storyCSSNode.InnerHtml);
            }

            // Return the story
            return story;
        }
    }
}