using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace libtwee 
{
    public class StoryFormat 
    {
        // name: (string) Optional. The name of the story format. (Omitting the name will lead to an Untitled Story Format.)
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        // version: (string) Required, and semantic version-style formatting (x.y.z, e.g., 1.2.1) of the version is also required.
        [JsonPropertyName("version")]
        public string Version { get; set; }
        
        // author: (string) Optional.
        [JsonPropertyName("author")]
        public string Author { get; set; }
        
        // description: (string) Optional.
        [JsonPropertyName("description")]
        public string Description { get; set; }
        // image: (string) Optional. The filename of an image (ideally SVG) served from the same directory as the format.js file.
        [JsonPropertyName("image")]
        public string Image { get; set; }
        
        // url: (string) Optional. The URL of the directory containing the format.js file.
        [JsonPropertyName("url")]
        public string Url { get; set; }
        
        // license: (string) Optional. The name of the license under which the story format is released.
        [JsonPropertyName("license")]
        public string License { get; set; }
        
        // proofing: (boolean) Optional (defaults to false). True if the story format is a "proofing" format. The distinction is relevant only in the Twine 2 UI.
        [JsonPropertyName("proofing")]
        public bool Proofing { get; set; }
        
        // source: (string) Required. An adequately escaped string containing the full HTML output of the story format, including the two placeholders {{STORY_NAME}} and {{STORY_DATA}}. (The placeholders are not themselves required.)
        [JsonPropertyName("source")]
        public string Source { get; set; }

        public StoryFormat()
        {
            Name = "Untitled Story Format";
            Version = "0.0.0";
            Author = "";
            Description = "";
            Image = "";
            Url = "";
            License = "";
            Proofing = false;
            Source = "";
        }

        public static StoryFormat FromJson(string json)
        {
            var result = JsonSerializer.Deserialize<StoryFormat>(json) ?? throw new InvalidOperationException("Deserialization resulted in a null StoryFormat object.");
            return result;
        }
    }
}