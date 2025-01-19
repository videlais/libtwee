using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace libtwee 
{
    public partial class Twee
    {
        /**
         * <summary>
         *   Create a Twee document from a Story object.
         */
        public static string Create(Story story) 
        {
            // Create a new StringBuilder to store the output.
            StringBuilder output = new();

            // Search for the StoryData passage.
            var storyDataPassage = story.GetPassageByName("StoryData");

            // Output the StoryData passage, creating it if it doesn't exist.
            if(storyDataPassage == null) {
                // Create a new StoryData passage.
                // Create a Dictionary to store the StoryData properties.
                Dictionary<string, object> storyData = [];

                // ifid: (string) Required. Maps to <tw-storydata ifid>.
                // Does the story have a valid IFID?
                if(Babel.IsValidTwineIFID(story.IFID)) {
                    // Add the IFID to the StoryData properties.
                    storyData["ifid"] = story.IFID;
                } else {
                    // Generate a new IFID and add it to the StoryData properties.
                    storyData["ifid"] = Babel.GenerateTwineIFID();
                }

                // format: (string) Optional. Maps to <tw-storydata format>.
                if(!string.IsNullOrEmpty(story.Format)) {
                    // Add the format to the StoryData properties.
                    storyData["format"] = story.Format;
                }

                // format-version: (string) Optional. Maps to <tw-storydata format-version>.
                if(!string.IsNullOrEmpty(story.FormatVersion)) {
                    // Add the format version to the StoryData properties.
                    storyData["format-version"] = story.FormatVersion;
                }

                // start: (string) Optional. Maps to <tw-passagedata name> of the node whose pid matches <tw-storydata startnode>.
                if(!string.IsNullOrEmpty(story.Start)) {
                    // Add the start node to the StoryData properties.
                    storyData["start"] = story.Start;
                }

                // tag-colors: (object of tag(string):color(string) pairs) Optional. Pairs map to <tw-tag> nodes as <tw-tag name>:<tw-tag color>.
                if(story.TagColors != null) {
                    // Add the tag colors to the StoryData properties.
                    storyData["tag-colors"] = story.TagColors;
                }

                // zoom: (decimal) Optional. Maps to <tw-storydata zoom>.
                if(story.Zoom != 0) {
                    // Add the zoom to the StoryData properties.
                    storyData["zoom"] = story.Zoom;
                }

                // Add the StoryData properties to the output.
                output.Append(":: StoryData\n");
                output.Append(JsonSerializer.Serialize(storyData, new JsonSerializerOptions { WriteIndented = true }));

            } 
            else 
            {
                // Output the existing StoryData passage.
                output.Append(storyDataPassage.ToTwee());
                // Append two newlines.
                output.Append("\n\n");
            }

            // Output the Story passages, skipping the StoryData passage.
            foreach(Passage passage in story.Passages) {
                if(passage.Name != "StoryData") {
                    // Append passage to the output.
                    output.Append(passage.ToTwee());
                    // Append two newlines.
                    output.Append("\n\n");
                }
            }

            // Return the output as a string.
            return output.ToString();
        }
    }
}