using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace libtwee
{
    public partial class Twine2HTML
    {
        /**
         * <summary>
         *   Parse a Twine 2 HTML document and return a Story object.
         *
         *  libtwee considers all input as a partial story. 
         *  It will only enforce required elements when producing output.
         *  (https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-htmloutput-spec.md)
         * </summary>
         * 
         * @param html Twine 2 HTML.
         * @return A Story object.
         */
        public static Story Parse(string html)
        {
            // Create a new story object
            Story story = new();

            // Create a new HTML document
            HtmlDocument doc = new();

            // Load the HTML into the document
            doc.LoadHtml(html);

            // Does the document contain a <tw-storydata> element?
            var storyDataNode = doc.DocumentNode.SelectSingleNode("//tw-storydata") ?? throw new Exception("ERROR: The document does not contain a <tw-storydata> element.");

            /**
                <tw-storydata
                    name="DocumentationExample"
                    startnode="1"
                    creator="Twine"
                    creator-version="2.3.3"
                    ifid="6D509890-1CA5-49DF-BFB6-5CA35B8DE2AC"
                    zoom="1"
                    format="Harlowe"
                    format-version="3.0.2">
                    ...
                </tw-storydata>
            */

            // Parse the <tw-storydata> element
            // name: (string) Required. The name of the story.
            story.Name = storyDataNode.GetAttributeValue("name", string.Empty);

            // ifid: (string) Required. An IFID is a sequence of between 8 and 63 characters, 
            //  each of which shall be a digit, a capital letter or a hyphen 
            //  that uniquely identify a story (see Treaty of Babel).
            story.IFID = storyDataNode.GetAttributeValue("ifid", string.Empty);

            // creator: (string) Optional. The name of program used to create the file.
            story.Creator = storyDataNode.GetAttributeValue("creator", string.Empty);
            // creator-version: (string) Optional. The version of the program used to create the file.
            story.CreatorVersion = storyDataNode.GetAttributeValue("creator-version", string.Empty);

            // zoom: (string) Optional. The decimal level of zoom (i.e. 1.0 is 100% and 1.2 would be 120% zoom level).
            story.Zoom = int.Parse(storyDataNode.GetAttributeValue("zoom", "1"));

            // format: (string) Optional. The story format used to create the story.
            story.Format = storyDataNode.GetAttributeValue("format", string.Empty);
            // format-version: (string) Optional. The version of the story format used to create the story.
            story.FormatVersion = storyDataNode.GetAttributeValue("format-version", string.Empty);

            // startnode: (string) Optional. The PID of the passage that the story should start at.
            // We record the name of the start node, not the PID.
            // The PID is subject to change during editing with Twine 2.
            string startNodePID = storyDataNode.GetAttributeValue("startnode", string.Empty);

            // Look for every <tw-passagedata> element in the document
            var passageNodes = doc.DocumentNode.SelectNodes("//tw-passagedata");

            /**
                <tw-passagedata
                    pid="1"
                    name="Start"
                    tags="tag1 tag2"
                    position="102,99"
                    size="100,100">
                    Some content
                </tw-passagedata>
            */

            if (passageNodes != null)
            {
                /**
                    Each passage is represented as a <tw-passagedata> element with its metadata stored as its attributes.
                    - pid: (string) Required. The Passage ID (PID). (Note: This is subject to change during editing with Twine 2.)
                    - name: (string) Required. The name of the passage.
                    - tags: (string) Optional. Any tags for the passage separated by spaces.
                    - position: (string) Optional. Comma-separated X and Y position of the upper-left of the passage when viewed within the Twine 2 editor.
                    - size: (string) Optional. Comma-separated width and height of the passage when viewed within the Twine 2 editor.
                */

                foreach (var node in passageNodes)
                {
                    // Check if current PID is the start node.
                    if (node.GetAttributeValue("pid", string.Empty) == startNodePID)
                    {
                        // Record the name of the start node.
                        story.Start = node.GetAttributeValue("name", string.Empty);
                    }

                    // Prepare the metadata dictionary.
                    Dictionary<string, object> metadata = [];

                    // Get the value or null if it doesn't exist.
                    string size = node.GetAttributeValue("size", null);

                    // If the size exists, add it to the metadata dictionary.
                    if (size != null)
                    {
                        // Add the size metadata.
                        metadata.Add("size", size);
                    }

                    // Get the value or null if it doesn't exist.
                    string position = node.GetAttributeValue("position", null);

                    // If the position exists, add it to the metadata dictionary.
                    if (position != null)
                    {
                        // Add the position metadata.
                        metadata.Add("position", position);
                    }

                    // Create a new passage object.
                    Passage passage = new()
                    {
                        Name = node.GetAttributeValue("name", "Untitled Passage"),
                        Tags = [.. node.GetAttributeValue("tags", string.Empty).Split([' '], StringSplitOptions.RemoveEmptyEntries)],
                        Metadata = metadata,
                        Text = node.InnerText
                    };

                    // Add the passage to the story.
                    story.Passages.Add(passage);
                }
            }

            return story;
        }
    }
}