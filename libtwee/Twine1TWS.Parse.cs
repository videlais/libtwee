using System;
using System.Collections.Generic;
using Razorvine.Pickle;

namespace libtwee
{
    public partial class Twine1TWS 
    {
        /**
         * <summary>
         *  From 2006 to 2009, Twine 1 supported a format called TWS.
         *  
         * TWS is a Python pickle file that contains a dictionary of story data.
         * </summary>
         */
        public static Story Parse(byte[] contentAsBytes) {
            // Create a new story object.
            Story story = new();

            // Unpickle the TWS file.
            Unpickler unpickler = new();

            // Create default dictionary for story data.
            Dictionary<string, object> storyData = [];

            // Try to unpickle the content.
            try {
                storyData = (Dictionary<string, object>) unpickler.loads(contentAsBytes);
            } catch (Exception e) {
                throw new Exception("ERROR: The document is not a valid TWS file. " + e.Message);
            }

            // DEBUG - Print the story data.
            Console.WriteLine(storyData);

            // Write to the Console the data found.
            foreach (KeyValuePair<string, object> entry in storyData) {
                Console.WriteLine(entry.Key + ": " + entry.Value);
            }

            // Return Story.
            return story;
        }
    }
}