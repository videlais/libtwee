using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace libtwee 
{
    public partial class Twee
    {
        /// <summary>
        /// Parses a Twee story from a string based on <see href="https://github.com/iftechfoundation/twine-specs/blob/master/twee-3-specification.md">Twee 3 Specification</see>.
        /// </summary>
        /// <param name="twee">The Twee story to parse.</param>
        /// <returns>The parsed Twee story.</returns>
        /// <exception cref="Exception">Thrown when the Twee story is invalid.</exception>
        public static Story Parse(string twee) {
            // Create a new story object
            Story story = new();

            // Convert the string into an array of UTF-8 bytes.
            // ASCII conversion introduces issues around escape characters.
            byte[] bytes = Encoding.UTF8.GetBytes(twee);

            // Look for the first (or any) instance of a colon.
            int delimiterIndex = Array.IndexOf(bytes, (byte)':');

            // Was at least one colon found?
            if (delimiterIndex == -1) {
                throw new Exception("ERROR: The document does not contain any passages.");
            }

            // Is there more content after the colon?
            if (delimiterIndex + 1 >= bytes.Length) {
                throw new Exception("ERROR: The document does not contain any passages.");
            }

            // Check if the next character is also a colon.
            if (bytes[delimiterIndex + 1] == (byte)':') {
                // Is there more content after the second colon?
                if (delimiterIndex + 2 >= bytes.Length) {
                    throw new Exception("ERROR: The document does not contain any passages.");
                }

                // Move the position to the character after the second colon.
                int position = delimiterIndex + 2;

                // While there is content after the second colon, keep looking for passages.
                // Begin with the first passage header.
                while (position < bytes.Length) {
                    // Create a tempnName variable to store the passage name.
                    string tempName = "";

                    // Create a tempTags variable to store the passage tags.
                    string tempTags = "";

                    // Create a tempContent variable to store the passage JSON metadata.
                    string tempMetadata = "{";
                    
                    // Continue to append characters to the tempName 
                    //  variable until a '[', '{', or newline is found.
                    //
                    // Escaped characters are supported, so we need to check for them.
                    while (position < bytes.Length &&
                           bytes[position] != (byte)'[' &&
                           bytes[position] != (byte)'{' &&
                           bytes[position] != (byte)'\n'
                           ) {
                        // Append the current character to the tempName variable.
                        tempName += (char)bytes[position];
                        // Move the position to the next character.
                        position++;
                    }

                    // Trim whitespace at the beginning and end of the passage name.
                    tempName = tempName.Trim();

                    // At this point, we might four possibilities:
                    // (1) We have read to a newline and have a passage name and nothing else.
                    // (2) We have read to a '[' and have a passage name and tags.
                    // (3) We have read to a '{' and have a passage name and metadata.
                    // (4) We have read to a '[' and have a passage name, tags, and metadata.

                    // Check if the current position is '['.
                    if (position < bytes.Length && 
                        bytes[position] == (byte)'[') {
                        // Move the position to the character after the '['.
                        position++;
                        
                        // Continue to append characters to the tempTags variable until a ']' is found.
                        // Ignore escaped characters.
                        while (position < bytes.Length && 
                               bytes[position] != (byte)']') {
                            tempTags += (char)bytes[position];
                            position++;
                        }
                        
                        // Move the position to the character after the ']'.
                        position++;
                    }

                    // After tags, we might have spaces, beginning of metadata, or newline.
                    // Keep reading until we find the metadata or newline.
                    while (position < bytes.Length &&
                           bytes[position] != (byte)'{' && bytes[position]-1 != (byte)'\\' &&
                           bytes[position] != (byte)'\n'
                           ) {
                        position++;
                    }

                    // Check if the current position is '{'.
                    if (position < bytes.Length &&
                        bytes[position] == (byte)'{' && bytes[position]-1 != (byte)'\\') {
                        // Move the position to the character after the '{'.
                        position++;
                        
                        // Continue to append characters to the tempMetadata variable
                        //  until a '{', newline, or content runs out.
                        while (position < bytes.Length &&
                            bytes[position] != (byte)'{' && bytes[position]-1 != (byte)'\\' &&
                               bytes[position] != (byte)'\n'
                               ) {
                            tempMetadata += (char)bytes[position];
                            position++;
                        }
                    }

                    // After metadata, we might have spaces or newline.
                    // Keep reading until we find the newline.
                    while (position < bytes.Length && bytes[position] != (byte)'\n') {
                        position++;
                    }

                    // Move the position to the character after the newline.
                    // Is there more content after the newline in the passage header?
                    if (position < bytes.Length && position + 1 >= bytes.Length) {
                        throw new Exception("ERROR: The document contains invalid passage.");
                    }

                    // Passage content (text) begins after the newline.
                    // It continues until the next passage header or the end of the document.
                    position++;

                    // Create a tempContent variable to store the passage content.
                    string tempContent = "";

                    // Continue to append characters to the tempContent variable
                    // Append until the new passage delimiter is found or the content runs out.
                    while (position < bytes.Length) {
                        // Check if the current character is a colon.
                        if (bytes[position] == (byte)':') {
                            // Check if the next character is also a colon.
                            if (bytes[position + 1] == (byte)':') {
                                // This might be a new passage header.
                                // Break out of the internal loop.
                                break;
                            }
                        }

                        // Append the current character to the tempContent variable.
                        tempContent += (char)bytes[position];
                        // Move the position to the next character.
                        position++;
                    }

                    // Remove one or more newline characters from the end of the passage content.
                    tempContent = tempContent.TrimEnd('\n');

                    // Create a new passage object.
                    Passage passage = new()
                    {
                        // Set the passage name.
                        Name = tempName,

                        // Split the passage tags into an array.
                        Tags = [.. tempTags.Split(' ')],

                        // Parse the passage metadata.
                        // Set a default
                        Metadata = []
                    };

                    // This is a JSON object, so we need to parse it.
                    try {
                        if (!string.IsNullOrEmpty(tempMetadata))
                        {
                            var metadata = JsonSerializer.Deserialize<Dictionary<string, object>>(tempMetadata);

                            // Check if the metadata is not null.
                            if (metadata != null)
                            {
                                passage.Metadata = metadata;
                            }
                        }
                    } catch (Exception e) {
                        Console.WriteLine($"WARN: Unable to parse passage metadata. {e.Message}");
                    }

                    // Set the passage content.
                    passage.Text = tempContent;

                    // Add the passage to the story.
                    story.Passages.Add(passage);   
                }
            }

            // Return the story object.
            return story;
        } 

    }

}