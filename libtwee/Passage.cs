using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace libtwee
{
    /// <summary>
    /// Class <c>Passage</c> represents a passage in a Twine story.
    /// <list type="bullet">
    ///     <item>
    ///         <term>Name</term>
    ///         <description>The name of every passage in a story should be unique.</description>
    ///     </item>
    ///     <item>
    ///         <term>Tags</term>
    ///         <description>Every passage can contain none, one, or many tags. Tags should be unique per passage.</description>
    ///     </item>
    ///     <item>
    ///         <term>Metadata</term>
    ///         <description>Passage can contain extra metadata. (Currently, only Twee format expresses metadata.)</description>
    ///     </item>
    ///     <item>
    ///         <term>Text</term>
    ///         <description>The text of a passage.</description>
    ///     </item>
    /// </list>
    /// Passages can be converted to Twee, JSON, and HTML formats. However, not every format supports all data.
    /// </summary>
    public class Passage
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("tags")]
        public HashSet<string> Tags { get; set; }
        [JsonPropertyName("metadata")]
        public Dictionary<string, object> Metadata { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }

        /// <summary>
        /// Constructor for the <c>Passage</c> class.
        /// </summary>
        public Passage()
        {
            Tags = [];
            Metadata = [];
            Name = "Untitled Passage";
            Text = "";
        }

        /// <summary>
        /// Constructor for the <c>Passage</c> class.
        /// </summary>
        public Passage(string name, string text)
        {
            Tags = [];
            Metadata = [];
            Name = name;
            Text = text;
        }

        /// <summary>
        /// Adds a tag to the passage.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns><c>true</c> if the tag was added, <c>false</c> if the tag already exists.</returns>
        public bool AddTag(string tag)
        {
            return Tags.Add(tag);
        }

        /// <summary>
        /// Checks if the passage has a tag.
        /// </summary>
        /// <param name="tag">Tag to check for.</param>
        /// <returns><c>true</c> if the passage contains the tag; <c>false</c> otherwise.</returns>
        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }

        /// <summary>
        /// Removes a tag from the passage.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns><c>true</c> if the tag was removed, <c>false</c> if the tag does not exist.</returns>
        public bool RemoveTag(string tag)
        {
            return Tags.Remove(tag);
        }

        /// <summary>
        /// Adds metadata to the passage.
        /// </summary>
        /// <param name="key">The key to add.</param>
        /// <param name="value">Value to add based on key.</param>
        /// <returns><c>true</c> if the metadata was added, <c>false</c> if the key could not be added.</returns>
        public bool AddMetadata(string key, object value)
        {
            return Metadata.TryAdd(key, value);
        }

        /// <summary>
        /// Checks if the passage has metadata.
        /// </summary>
        /// <param name="key">Key to check for.</param>
        /// <returns><c>true</c> if the passage contains the key; <c>false</c> otherwise.</returns>
        public bool RemoveMetadata(string key)
        {
            return Metadata.Remove(key);
        }

        /// <summary>
        /// Sets metadata for the passage.
        /// </summary>
        /// <param name="key">Key to reference.</param>
        /// <param name="value">New value to update.</param>
        public void SetMetadata(string key, object value)
        {
            Metadata[key] = value;
        }

        /// <summary>
        /// Gets metadata for the passage.
        /// </summary>
        /// <param name="key">Key to reference.</param>
        /// <returns>Value of the key.</returns>
        public object GetMetadata(string key)
        {
            return Metadata[key];
        }

        /// <summary>
        /// Checks if the passage has metadata.
        /// </summary>
        /// <param name="key">Key to check for.</param>
        /// <returns><c>true</c> if the passage contains the key; <c>false</c> otherwise.</returns>
        public bool HasMetadata(string key)
        {
            return Metadata.ContainsKey(key);
        }

        /// <summary>
        /// Returns a Twee representation of the passage following the <see href="https://github.com/iftechfoundation/twine-specs/blob/master/twee-3-specification.md">Twee 3 Specification</see>.
        /// </summary>
        /// <returns>String containing Twee representation of passage.</returns>
        /// <exception cref="EmptyPassageNameException">Throws error if passage name is empty or null</exception>
        public string ToTwee()
        {
            // If Name is empty, throw an exception.
            if (string.IsNullOrEmpty(Name))
            {
                throw new EmptyPassageNameException();
            }

            // Add the passage name.
            string twee = $":: {Name}";

            // Add tags.
            if (Tags.Count > 0)
            {
                twee += $" [{string.Join(" ", Tags)}]";
            }

            // Add metadata.
            if (Metadata.Count > 0)
            {
                // Add a space after the tags.
                twee += " ";
                // Convert Dictionary into JSON.
                twee += JsonSerializer.Serialize(Metadata);
            }

            // Add the passage text.
            twee += $"\n{Text}";

            // Return the twee representation.
            return twee;
        }

        /// <summary>
        /// Returns a JSON representation of the passage following the <see href="https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-jsonoutput-doc.md">Twine 2 JSON Output Specification</see>.
        /// </summary>
        /// <returns>String containing JSON representation of passage</returns>
        /// <exception cref="EmptyPassageNameException">Throws exception if passage name is empty</exception>
        public string ToJson()
        {
            // If Name is empty, throw an exception.
            if (string.IsNullOrEmpty(Name))
            {
                throw new EmptyPassageNameException();
            }

            // Create a dictionary to store the passage data.
            Dictionary<string, object> data = new()
            {
                // Add the passage name.
                { "name", Name },
                // Add the tags.
                { "tags", Tags },
                // Add the metadata.
                { "metadata", Metadata },
                // Add the passage text.
                { "text", Text }
            };

            // Convert the dictionary into JSON.
            return JsonSerializer.Serialize(data);
        }

        /// <summary>
        /// Returns Twine 2 HTML representation of the passage following the <see href="https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-htmloutput-spec.md">Twine 2 HTML Output Specification</see>.
        /// </summary>
        /// <param name="pid">Passage ID</param>
        /// <returns>String containing HTML representation of passage</returns>
        public string ToTwine2HTML(int pid = 1)
        {
            // If Name is empty, throw an exception.
            if (string.IsNullOrEmpty(Name))
            {
                throw new Exception("Passage name cannot be empty.");
            }

            // Add the passage name and PID.
            string html = $"<tw-passagedata pid=\"{pid}\" name=\"{Name}\"";

            // Add tags.
            html += $" tags=\"{string.Join(" ", Tags)}\"";

            // Does "position" exist within the metadata?
            if (Metadata.TryGetValue("position", out object? pValue))
            {
                // Add the position attribute.
                html += $" position=\"{pValue}\"";
            }

            // Does "size" exist within the metadata?
            if (Metadata.TryGetValue("size", out object? sValue))
            {
                // Add the size attribute.
                html += $" size=\"{sValue}\"";
            }

            // Add the passage text and close the element.
            html += $">{Text}</tw-passagedata>";

            // Return the HTML representation.
            return html;
        }

        /// <summary>
        /// Returns Twine 1 HTML representation of the passage following the <see href="https://github.com/iftechfoundation/twine-specs/blob/master/twine-1-htmloutput-doc.md">Twine 1 HTML Output Specification</see>.
        /// </summary>
        /// <returns>String containing HTML representation of passage</returns>
        /// <exception cref="EmptyPassageNameException">Throws exception if passage name is empty</exception>
        public string ToTwine1HTML()
        {
            // If Name is empty, throw an exception.
            if (string.IsNullOrEmpty(Name))
            {
                throw new EmptyPassageNameException();
            }

            // Add the passage name.
            string html = $"<div tiddler=\"{Name}\"";

            // Add the tags.
            html += $" tags=\"{string.Join(" ", Tags)}\"";

            // Add the modifier tool.
            html += " modifier=\"extwee\"";

            // Does "position" exist within the metadata?
            if (Metadata.TryGetValue("position", out object? pValue))
            {
                // Add the position attribute.
                html += $" twine-position=\"{pValue}\"";
            }
            else
            {
                // Add a default position.
                html += " twine-position=\"10,10\"";
            }

            // Add the passage text.
            html += $">{Text}</div>";

            // Return the HTML representation.
            return html;
        }
    }
}