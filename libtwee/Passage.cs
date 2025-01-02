using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace libtwee
{
    public class Passage
    {
        public string Name { get; set; }
        public HashSet<string> Tags { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
        public string Text { get; set; }

        /**
         * Default constructor.
         * Initializes the passage with empty values.
         */
        public Passage()
        {
            Tags = [];
            Metadata = [];
            Name = "Untitled Passage";
            Text = "";
        }

        public Passage(string name, string text)
        {
            Tags = [];
            Metadata = [];
            Name = name;
            Text = text;
        }

        public void AddTag(string tag)
        {
            Tags.Add(tag);   
        }

        public bool HasTag(string tag)
        {
            return Tags.Contains(tag);
        }

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
        }

        public void AddMetadata(string key, object value)
        {
            Metadata.TryAdd(key, value);
        }

        public void RemoveMetadata(string key)
        {
            Metadata.Remove(key);
        }

        public void SetMetadata(string key, object value)
        {
            Metadata[key] = value;
        }

        public object GetMetadata(string key)
        {
            return Metadata[key];
        }

        public bool HasMetadata(string key)
        {
            return Metadata.ContainsKey(key);
        }

        /**
         * Returns a Twee representation of the passage.
         */
        public string ToTwee() {
            // If Name is empty, throw an exception.
            if (string.IsNullOrEmpty(Name)) {
                throw new Exception("Passage name cannot be empty.");
            }

            // Add the passage name.
            string twee = $":: {Name}";

            // Add tags.
            if (Tags.Count > 0) {
                twee += $" [{string.Join(" ", Tags)}]";
            }

            // Add metadata.
            if (Metadata.Count > 0) {
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

        /**
         * Returns a JSON representation of the passage.
         */
        public string ToJson() {
            // If Name is empty, throw an exception.
            if (string.IsNullOrEmpty(Name)) {
                throw new Exception("Passage name cannot be empty.");
            }

            // Create a dictionary to store the passage data.
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                // Add the passage name.
                { "name", Name }
            };

            // Add tags.
            data.Add("tags", Tags);

            // Add metadata.
            data.Add("metadata", Metadata);

            // Add the passage text.
            data.Add("text", Text);

            // Convert the dictionary into JSON.
            return JsonSerializer.Serialize(data);
        }

        /**
         * Returns a Twine 2 HTML representation of the passage.
         */
        public string ToTwine2HTML(int pid = 1) {
            // If Name is empty, throw an exception.
            if (string.IsNullOrEmpty(Name)) {
                throw new Exception("Passage name cannot be empty.");
            }

            // Add the passage name and PID.
            string html = $"<tw-passagedata pid=\"{pid}\" name=\"{Name}\"";

            // Add tags.
            html += $" tags=\"{string.Join(" ", Tags)}\"";

            // Does "position" exist within the metadata?
            if (Metadata.TryGetValue("position", out object? pValue)) {
                // Add the position attribute.
                html += $" position=\"{pValue}\"";
            }

            // Does "size" exist within the metadata?
            if (Metadata.TryGetValue("size", out object? sValue)) {
                // Add the size attribute.
                html += $" size=\"{sValue}\"";
            }

            // Add the passage text and close the element.
            html += $">{Text}</tw-passagedata>";

            // Return the HTML representation.
            return html;
         }

        /**
        * Returns Twine 1 HTML representation of the passage.
        * Example:
        * <div
        *  created="2023 06 02 012 1"
        *  modifier="extwee"
        *  twine-position="10,10">[[One passage]]</div>
        */
        public string ToTwine1HTML() {
            // If Name is empty, throw an exception.
            if (string.IsNullOrEmpty(Name)) {
                throw new Exception("Passage name cannot be empty.");
            }

            // Add the passage name.
            string html = $"<div tiddler=\"{Name}\"";

            // Add the tags.
            html += $" tags=\"{string.Join(" ", Tags)}\"";

            // Add the modifier tool.
            html += " modifier=\"extwee\"";

            // Does "position" exist within the metadata?
            if (Metadata.TryGetValue("position", out object? pValue)) {
                // Add the position attribute.
                html += $" twine-position=\"{pValue}\"";
            } else {
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