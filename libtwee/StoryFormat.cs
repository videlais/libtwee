using System.Text.Json.Serialization;
using System.Text.Json;

namespace libtwee
{
    /// <summary>
    /// A class representing a story format in Twine.
    /// </summary>
    public partial class StoryFormat
    {
        /// <summary>
        /// The name of the story format. Omitting the name results in an "Untitled Story Format".
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The version of the story format in semantic version format (x.y.z).
        /// </summary>
        [JsonPropertyName("version")]
        public string Version { get; set; }

        /// <summary>
        /// The author of the story format.
        /// </summary>
        [JsonPropertyName("author")]
        public string Author { get; set; }

        /// <summary>
        /// A description of the story format.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// The filename of an image (ideally SVG) served from the same directory as the format.js file.
        /// </summary>
        [JsonPropertyName("image")]
        public string Image { get; set; }

        /// <summary>
        /// The URL of the directory containing the format.js file.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// The name of the license under which the story format is released.
        /// </summary>
        [JsonPropertyName("license")]
        public string License { get; set; }

        /// <summary>
        /// Whether the story format is a "proofing" format. Defaults to <c>false</c>.
        /// </summary>
        [JsonPropertyName("proofing")]
        public bool Proofing { get; set; }

        /// <summary>
        /// The full HTML source of the story format, including the placeholders {{STORY_NAME}} and {{STORY_DATA}}.
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; }

        /// <summary>
        /// Constructor for StoryFormat.
        /// </summary>
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

        /// <summary>
        /// Deserializes a JSON string to a StoryFormat object.
        /// </summary>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>A StoryFormat object.</returns>
        public static StoryFormat FromJson(string json)
        {
            var result = JsonSerializer.Deserialize<StoryFormat>(json) ?? throw new InvalidOperationException("Deserialization resulted in a null StoryFormat object.");
            return result;
        }

        /// <summary>
        /// Serializes the StoryFormat object to a JSON string.
        /// </summary>
        /// <returns>A JSON string.</returns>
        public string ToJson()
        {
            // Create string result
            string result;

            // Try to serialize the object
            try
            {
                result = JsonSerializer.Serialize(this);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("ERROR: Unable to serialize StoryFormat: " + ex.Message);
            }

            // Return the result
            return result;
        }

        /// <summary>
        /// Writes the StoryFormat object to a string in a format suitable for inclusion in a Twine story.
        /// </summary>
        /// <returns>A string in a format suitable for inclusion in a Twine story.</returns>
        public string Write()
        {
            // Wrap the JSON in a function.
            string result = $"window.storyFormat({this.ToJson()});";

            // Return the result
            return result;
        }
    }
}