using System;
using System.Collections.Generic;

namespace libtwee 
{
    public partial class Twee
    {
        /**
         * <summary>
         *   Parse a Twee document and return a Story object.
         *
         *  libtwee considers all input as a partial story. 
         *  It will only enforce required elements when producing output.
         */
        public static Story Parse(string twee) {
            // Create a new story object
            Story story = new();

            // Convert the string into an array of UTF-8 bytes.
            // ASCII conversion introduces issues around escape characters.
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(twee);

            // Look for the first (or any) instance of a colon.
            int delimiterIndex = Array.IndexOf(bytes, (byte)':');

            // Was at one colon found?
            if (delimiterIndex == -1) {
                throw new Exception("ERROR: The document does not contain any passages.");
            }

            // Is there more content after the colon?
            if (delimiterIndex + 1 >= bytes.Length) {
                throw new Exception("ERROR: The document does not contain any passages.");
            }

            // Check if the next character is also a colon.
            if (bytes[delimiterIndex + 1] == (byte)':') {
                
            }

            // Return the story object.
            return story;
        } 

    }

}