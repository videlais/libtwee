using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace libtwee
{
    public partial class Twine1HTML
    {
       /// <summary>
       /// Compiles a story into Twine 1 HTML. Partial support for <see href="https://github.com/iftechfoundation/twine-specs/blob/master/twine-1-htmloutput-doc.md">Twine 1 HTML Output</see>.
       /// </summary>
       /// <param name="story">Story object to compile</param>
       /// <param name="engine">Engine code to compile</param>
       /// <param name="header">Header HTML</param>
       /// <returns>String containing Twine 1 HTML</returns>
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