using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace libtwee
{
    public partial class Twine1HTML
    {
        /**
         * <summary>
         *   Compile a Twine 1 story + header.html (Twine 1 story format) and return HTML code.
         *  Adheres to only partial support for all options. https://github.com/iftechfoundation/twine-specs/blob/master/twine-1-htmloutput-doc.md
         * </summary>
         */
        public static string Compile(Story story, string engine = "", string header = "") 
        {
            // Replace the "VERSION" with story.creator in the header HTML.
            header = header.Replace("VERSION", story.Creator);

            // Replace the "TIME" with new Date().ToISOString() in the header HTML.
            header = header.Replace("TIME", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));

            // Replace the ENGINE with `engine.js` code in the header.
            header = header.Replace("ENGINE", engine);

            // Replace "STORY_SIZE" with number of passages in current story in the header.
            header = header.Replace("STORY_SIZE", story.Passages.Count.ToString());

            // Replace "STORY" with Twine 1 HTML in the header.
            header = header.Replace("STORY", story.ToTwine1HTML());

            // Replace START_AT with '' in the header.
            header = header.Replace("START_AT", "");

            // Return the header.
            return header;
        }
    }
}