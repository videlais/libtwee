using System.Text.Json;

namespace libtwee
{
    public partial class StoryFormat
    {
        /// <summary>
        /// Parses a story format from a string.
        /// </summary>
        /// <param name="storyFormatData">The story format data to parse.</param>
        /// <returns>The parsed story format.</returns>
        public static StoryFormat Parse(string storyFormatData)
        {
            // Create a new StoryFormat object
            StoryFormat format = new();

            // A story format is a JSONP object with a wrapper function.
            // The function name is window.storyFormat().

            // Does the story format data start with "window.storyFormat("?
            if (storyFormatData.StartsWith("window.storyFormat("))
            {
                // Yes, it does. Remove the "window.storyFormat(" from the start of the string.
                storyFormatData = storyFormatData[19..];
            }
            else
            {
                // No, it doesn't. This is an error.
                throw new Exception("ERROR: The story format data does not start with 'window.storyFormat('.");
            }

            // Does the story format data end with ");"?
            if (storyFormatData.EndsWith(");"))
            {
                // Yes, it does. Remove the ");" from the end of the string.
                storyFormatData = storyFormatData[0..^2];
            }
            else
            {
                // No, it doesn't. This is an error.
                throw new Exception("ERROR: The story format data does not end with ');'.");
            }

            // Try to deserialize the JSON data into the StoryFormat object.
            try
            {
                // Deserialize the JSON data into the StoryFormat object.
                format = JsonSerializer.Deserialize<StoryFormat>(storyFormatData) ?? throw new Exception("ERROR: The story format data is not valid JSON.");
            }
            catch (Exception ex)
            {
                // If the deserialization fails, throw an error.
                throw new Exception("ERROR: The story format data is not valid JSON.", ex);
            }

            // Return the StoryFormat object
            return format;
        }
    }
}