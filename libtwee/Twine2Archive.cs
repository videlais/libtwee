using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace libtwee
{
    public class Twine2Archive
    {
        public List<Story> Stories { get; set; } = [];

        public string CreateHTML() 
        {
            // Create initial string.
            string output = "";

            // Loop through each story.
            // For each, call its ToTwine2HTML method.
            // Add two newlines between each story.
            foreach (Story story in Stories)
            {
                // Add the story's HTML to the output.
                output += story.ToTwine2HTML() + "\n\n";
            }

            // Return the string.
            return output;
        }

        public static Twine2Archive Parse(string html) 
        {
            // Create a new Twine2Archive object.
            Twine2Archive archive = new();

            // Create a new HTML document
            HtmlDocument doc = new();

            // Load the HTML into the document
            doc.LoadHtml(html);

           // Find all instances, if any, of the <tw-storydata> element.
           // If there are none, throw an exception.
           HtmlNodeCollection entries = doc.DocumentNode.SelectNodes("//tw-storydata") ?? throw new Exception("ERROR: The document does not contain a <tw-storydata> element.");

            // Loop through each <tw-storydata> element.
            foreach (HtmlNode entry in entries)
            {
                // Parse the <tw-storydata> element.
                Story story = Twine2HTML.Parse(entry.OuterHtml);
                
                // Add the Story object to the Twine2Archive object.
                archive.Stories.Add(story);
            }
            
            // Return the Twine2Archive object.
            return archive;
        }
    }
}