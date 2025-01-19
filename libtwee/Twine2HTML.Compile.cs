using System;
using System.Collections.Generic;

namespace libtwee
{
    public partial class Twine2HTML
    {
        /**
         * <summary>
         *   Compile a story and story format and return HTML code.
         *
         *  (https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-htmloutput-spec.md)
         * </summary>
         * 
         * @param story Story object.
         * @param storyFormat Story format object.
         * @throws Exception If the story format source is empty.
         * @throws Exception If the story IFID is not a valid uppercase UUIDv4.
         * @throws Exception If the story name is empty.
         * @return string HTML code of story + story format.
         */
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