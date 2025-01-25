using System;
using System.Collections.Generic;

namespace libtwee
{
    /// <summary>
    /// Class <c>Twine2HTML</c> provides methods to compile a story and story format into HTML code based on <see href="https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-htmloutput-spec.md">Twine 2 HTML Output Specification</see>. 
    /// </summary>
    public partial class Twine2HTML
    {
        /// <summary>
        /// Compiles a story and story format into HTML code.
        /// </summary>
        /// <param name="story">The story to compile.</param>
        /// <param name="storyFormat">The story format to use.</param>
        /// <returns>The compiled HTML code.</returns>
        /// <exception cref="Exception">Thrown when the story format source is empty or the story IFID is not a valid UUIDv4.</exception>
        public static string Compile(Story story, StoryFormat storyFormat)
        {
            // Check if the source of the storyFormat is not empty.
            if (string.IsNullOrEmpty(storyFormat.Source))
            {
                throw new Exception("ERROR: The source of the story format is empty.");
            }

            // Verify the story contains an IFID in UUIDv4 format using uppercase letters.
            if (!Babel.IsValidTwineIFID(story.IFID))
            {
                throw new Exception("ERROR: The story IFID is not a valid.");
            }

            // Create string data HTML from the story.
            string storyData = story.ToTwine2HTML();

            // Create a copy of the story format source with the {{STORY_DATA}} placeholder replaced with the story data.
            string storyFormatSource = storyFormat.Source.Replace("""{{STORY_DATA}}""", storyData);

            // In the new copy, replace {{STORY_NAME}} with the story name.
            storyFormatSource = storyFormatSource.Replace("""{{STORY_NAME}}""", story.Name);

            // Return the new copy with the story name and story data replaced.
            return storyFormatSource;
        }
    }
}