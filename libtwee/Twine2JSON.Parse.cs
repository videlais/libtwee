using System;
using System.Collections.Generic;
using System.Text.Json;

namespace libtwee
{
    public partial class Twine2JSON
    {
        /**
         * <summary>
         *   Parse a Twine 2 JSON document and return a Story object.
         *
         *  libtwee considers all input as a partial story. 
         *  It will only enforce required elements when producing output.
         */
         public static Story Parse(string json) {
            // Create a new story object.
            Story story = new();

            // Create a new dictionary to store the parsed JSON data.
            Dictionary<string, object> data = [];

            try {
                // Parse the JSON into a dictionary.
                data = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? [];
            } catch (JsonException e) {
                throw new FormatException("ERROR: Invalid JSON format.", e);
            }

            /**
            {
                "name": "Example",
                "ifid": "D674C58C-DEFA-4F70-B7A2-27742230C0FC",
                "format": "Snowman",
                "format-version": "3.0.2",
                "start": "My Starting Passage",
                "tag-colors": {
                    "bar": "Green",
                    "foo": "red",
                    "qaz": "blue"
                },
                "zoom": 0.25,
                "creator": "Twine",
                "creator-version": "2.8",
                "style": "",
                "script": ""
            }
            */

            // Parse the story metadata.
            // name: (string) Required. The name of the story.
            if(data.TryGetValue("name", out object? nameValue)) {
                story.Name = nameValue?.ToString() ?? "Untitled";
            } else {
                // Produce warning if name is missing.
                Console.WriteLine("WARNING: name is required. Ignoring data.");
            }

            // ifid: (string) Optional. An IFID is a sequence of between 8 and 63 characters, each of which shall be a digit, a capital letter or a hyphen that uniquely identify a story (see Treaty of Babel). Maps to <tw-storydata ifid>.
            story.IFID = data.TryGetValue("ifid", out object? ifidValue) ? ifidValue?.ToString() ?? "" : "";

            // format: (string) Optional. The format of the story. Maps to <tw-storydata format>.
            story.Format = data.TryGetValue("format", out object? formatValue) ? formatValue?.ToString() ?? "" : "";

            // format-version: (string) Optional. The version of the format. Maps to <tw-storydata format-version>.
            story.FormatVersion = data.TryGetValue("format-version", out object? formatVersionValue) ? formatVersionValue?.ToString() ?? "" : "";

            // start: (string) Optional. The name of the starting passage. Maps to <tw-passagedata name="Start">.
            story.Start = data.TryGetValue("start", out object? startValue) ? startValue?.ToString() ?? "" : "";

            // Test if the tag-colors key exists in the data dictionary.
            if(data.TryGetValue("tag-colors", out object? tagColorsValue)) {
                // Create empty KeyValuePair collection to store the tag-colors.
                Dictionary<string, string> tagColors = [];

                if (tagColorsValue is JsonElement tagColorsElement && tagColorsElement.ValueKind == JsonValueKind.Object)
                {
                    foreach (JsonProperty item in tagColorsElement.EnumerateObject())
                    {
                        // Add the tag-colors to the story object.
                        story.TagColors.Add(item.Name, item.Value.GetString() ?? "");
                    }
                }
                else
                {
                    // If tag-colors is not a collection, produce a warning and skip data.
                    Console.WriteLine("WARNING: tag-colors is not a collection. Ignoring data.");
                }
            }

            // zoom: (float) Optional. The zoom level of the story. Maps to <tw-storydata zoom>.
            if(data.TryGetValue("zoom", out object? zoomValue) && zoomValue is JsonElement zoomElement && zoomElement.TryGetSingle(out float zoom)) {
                story.Zoom = zoom;
            }

            // creator: (string) Optional. The name of the creator. Maps to <tw-storydata creator>.
            story.Creator = data.TryGetValue("creator", out object? creatorValue) ? creatorValue?.ToString() ?? "" : "";

            // creator-version: (string) Optional. The version of the creator. Maps to <tw-storydata creator-version>.
            story.CreatorVersion = data.TryGetValue("creator-version", out object? creatorVersionValue) ? creatorVersionValue?.ToString() ?? "" : "";

            // style: (string) Optional. The style of the story. Maps to <style>.
            story.StoryStylesheets.Add(data.TryGetValue("style", out object? styleValue) ? styleValue?.ToString() ?? "" : "");

            // script: (string) Optional. The script of the story. Maps to <script>.
            story.StoryScripts.Add(data.TryGetValue("script", out object? scriptValue) ? scriptValue?.ToString() ?? "" : "");

            /**
            "passages": [
                {
                "name": "My Starting Passage",
                "tags": ["tag1", "tag2"],
                "metadata": {
                    "position":"600,400",
                    "size":"100,200"
                },
                "text": "Double-click this passage to edit it."
                }
            ]
            */

            // Parse the story passages.
            if (data.TryGetValue("passages", out object? passagesValue)) {
                // Convert passagesValue to a list of dictionaries.
                if(passagesValue != null) {
                    // Create a new list to store the passages.
                    List<Dictionary<string, object>> passages = [];

                    try {
                        passages = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(passagesValue.ToString() ?? string.Empty) ?? [];
                    } catch (JsonException e) {
                        // Produce warning if passagesValue is not a list of dictionaries.
                        Console.WriteLine("WARNING: passages is not a valid collection of passage data. JSON Exception: " + e.Message);
                    }

                    // Loop through the passages.
                    foreach (Dictionary<string, object> passage in passages) {
                        // Create a new passage object.
                        Passage newPassage = new()
                        {
                            // name: (string) Required. The name of the passage.
                            Name = passage.TryGetValue("name", out object? pNameValue) ? pNameValue?.ToString() ?? "" : ""
                        };
                        // tags: (string[]) Optional. The tags of the passage.
                        if (passage.TryGetValue("tags", out object? tagsValue)) {
                            // Convert tagsValue to a list of strings.
                            List<string> tags = tagsValue != null ? JsonSerializer.Deserialize<List<string>>(tagsValue.ToString() ?? string.Empty) ?? [] : [];
                            // Loop through the tags.
                            foreach (string tag in tags) {
                                // Add the tag to the passage object.
                                newPassage.Tags.Add(tag);
                            }
                        }
                        // metadata: (Dictionary<string, object>) Optional. The metadata of the passage.
                        if (passage.TryGetValue("metadata", out object? metadataValue) && metadataValue is Dictionary<string, object> metadata) {
                            // position: (string) Optional. The position of the passage.
                            //newPassage.Metadata.Position = metadata.TryGetValue("position", out object? positionValue) ? positionValue?.ToString() ?? "" : "";
                            // size: (string) Optional. The size of the passage.
                            //newPassage.Metadata.Size = metadata.TryGetValue("size", out object? sizeValue) ? sizeValue?.ToString() ?? "" : "";
                        }
                        // text: (string) Optional. The text of the passage.
                        newPassage.Text = passage.TryGetValue("text", out object? textValue) ? textValue?.ToString() ?? "" : "";
                        // Add the passage to the story object.
                        story.Passages.Add(newPassage);
                    }
                } else {
                    // Produce warning if passages is missing.
                    Console.WriteLine("WARNING: No passages found.");
                }
            }

            // Return the story object.
            return story;
         }
    }
}