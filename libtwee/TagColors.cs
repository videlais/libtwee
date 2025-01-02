using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace libtwee
{
    /**
     * In Twine 2, passage tags are guaranteed to be unique per story. 
     * This class enforces that guarantee by using a dictionary to store tags.
     */
    public class TagColors
    {
        private readonly Dictionary<string, string> _tagColors = [];
        public int Count => _tagColors.Count;

        public void AddTag(string tag, string color)
        {
            // If the tag already exists, update the color.
            _tagColors[tag] = color;
        }

        public bool HasTag(string tag)
        {
            return _tagColors.ContainsKey(tag);
        }

        public string? GetColor(string tag)
        {
            if (_tagColors.TryGetValue(tag, out string? color))
            {
                return color;
            }
            return null;
        }

        public bool RemoveTag(string tag)
        {
            return _tagColors.Remove(tag);
        }

        public void Clear()
        {
            _tagColors.Clear();
        }

        /**
         * Returns a string of Twine 2 HTML that represents the tag colors.
         */
        public string ToTwine2HTML()
        {
            string html = "";
            
            foreach (KeyValuePair<string, string> tagColor in _tagColors)
            {
                html += $"<tw-tag name=\"{tagColor.Key}\" color=\"{tagColor.Value}\" />\n";
            }
            
            return html;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(_tagColors);
        }
    }
}