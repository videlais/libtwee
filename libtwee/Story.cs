using System.Text.Json;

namespace libtwee
{
    /// <summary>
    /// Represents a Twine story containing passages, metadata, and format information.
    /// A story can be serialized to and from Twee, Twine 1 HTML, Twine 2 HTML, and JSON formats.
    /// </summary>
    public class Story
    {
        /// <summary>
        /// The name of the story.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Interactive Fiction Identifier (IFID) for the story in uppercase UUID format.
        /// </summary>
        public string IFID { get; set; }

        /// <summary>
        /// The name of the starting passage.
        /// </summary>
        public string Start { get; set; }

        /// <summary>
        /// The story format (e.g., "Harlowe", "SugarCube", "Snowman").
        /// </summary>
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// The version of the story format.
        /// </summary>
        public string FormatVersion { get; set; } = string.Empty;

        /// <summary>
        /// The zoom level of the story (1.0 is 100%).
        /// </summary>
        public float Zoom { get; set; }

        /// <summary>
        /// The list of passages in the story.
        /// </summary>
        public List<Passage> Passages { get; }

        /// <summary>
        /// The name of the program that created the story.
        /// </summary>
        public string Creator { get; set; } = string.Empty;

        /// <summary>
        /// The version of the program that created the story.
        /// </summary>
        public string CreatorVersion { get; set; } = string.Empty;

        /// <summary>
        /// A dictionary mapping tag names to color values.
        /// </summary>
        public Dictionary<string, string> TagColors { get; set; }

        /// <summary>
        /// The number of passages in the story.
        /// </summary>
        public int Count => Passages.Count;

        private readonly JsonSerializerOptions JSONSerializePrettyPrint = new() { WriteIndented = true };

        /// <summary>
        /// A list of story-level CSS stylesheets.
        /// </summary>
        public List<string> StoryStylesheets { get; set; }

        /// <summary>
        /// A list of story-level JavaScript scripts.
        /// </summary>
        public List<string> StoryScripts { get; set; }

        /// <summary>
        /// Constructor for the Story class.
        /// </summary>
        public Story()
        {
            Passages = [];
            TagColors = new();
            Name = "Untitled";
            IFID = "";
            Start = "";
            Format = "";
            FormatVersion = "";
            Zoom = 1.0f;
            Creator = "";
            CreatorVersion = "";
            StoryStylesheets = [];
            StoryScripts = [];
        }

        /// <summary>
        /// Adds a passage to the story.
        /// </summary>
        /// <param name="p">Passage to add to the story.</param>
        /// <returns>Number of passages in story</returns>
        public int AddPassage(Passage p)
        {
            // Check if the passage already exists in the story.
            if (Passages.Exists(entry => entry.Name == p.Name))
            {
                // Overwrite the existing passage with the new passage.
                Passages[Passages.FindIndex(entry => entry.Name == p.Name)] = p;

                // Return the number of passages in the story.
                return Passages.Count;
            }

            // If the incoming passage is named 'StoryData', 
            //  update the story's metadata based on specific keys.
            if (p.Name == "StoryData")
            {
                // StoryData is assumed to contain JSON.
                var metadata = new Dictionary<string, object>();

                // Try to deserialize the JSON into a dictionary.
                try
                {
                    metadata = JsonSerializer.Deserialize<Dictionary<string, object>>(p.Text);
                }
                catch (JsonException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }

                if (metadata != null)
                {
                    // Does the StoryData contain 'ifid'?
                    if (metadata.TryGetValue("ifid", out object? ifidValue))
                    {
                        IFID = ifidValue?.ToString() ?? string.Empty;
                    }

                    // Does the StoryData contain 'start'?
                    if (metadata.TryGetValue("start", out object? startValue))
                    {
                        Start = startValue?.ToString() ?? string.Empty;
                    }

                    // Does the StoryData contain 'format'?
                    if (metadata.TryGetValue("format", out object? formatValue))
                    {
                        Format = formatValue?.ToString() ?? string.Empty;
                    }

                    // Does the StoryData contain 'format-version'?
                    if (metadata.TryGetValue("format-version", out object? formatVersionValue))
                    {
                        FormatVersion = formatVersionValue?.ToString() ?? string.Empty;
                    }

                    // Does the StoryData contain 'zoom'?
                    if (metadata.TryGetValue("zoom", out object? zoomValue) && zoomValue is JsonElement element && element.TryGetDouble(out double zoom))
                    {
                        // Convert the zoom value to a float.
                        Zoom = (float)zoom;
                    }

                    // Does the StoryData contain 'tag-colors'?
                    if (metadata.TryGetValue("tag-colors", out object? tagColorsValue))
                    {
                        // Convert the tag-colors value to a dictionary.
                        var deserializedTagColors = JsonSerializer.Deserialize<Dictionary<string, string>>(tagColorsValue?.ToString() ?? string.Empty);

                        // Check if the deserialized tag colors are not null.
                        if (deserializedTagColors != null)
                        {
                            TagColors = deserializedTagColors;
                        }
                    }
                }

                return Passages.Count;
            }

            // If the incoming passage is named 'StoryTitle',
            //  update the story's name based on the passage's text.
            if (p.Name == "StoryTitle")
            {
                Name = p.Text;
            }

            // If the incoming passage is named 'Start',
            //  check if Start was already set.
            if (p.Name == "Start")
            {
                // Set internal start based on Start.
                /**
                * Four possible scenarios:
                * 1. StoryData has already been encountered, and we will never get here.
                * 2. StoryData exists and will be encountered after Start.
                * 3. StoryData does not exist.
                * 4. Start is the first and only passage.
                */
                if (string.IsNullOrEmpty(Start))
                {
                    Start = p.Name;
                }
            }

            // Add the passage to the story.
            Passages.Add(p);

            // Return the number of passages in the story.
            return Passages.Count;
        }

        /// <summary>
        /// Removes a passage from the story by its name.
        /// </summary>
        /// <param name="name">Name of the passage to remove</param>
        public void RemovePassageByName(string name)
        {
            Passages.RemoveAll(p => p.Name == name);
        }

        /// <summary>
        /// Returns all passages that contain the specified tag.
        /// </summary>
        /// <param name="tag">The tag to search for.</param>
        /// <returns>A list of passages containing the specified tag.</returns>
        public List<Passage> GetPassagesByTag(string tag)
        {
            List<Passage> results = Passages.FindAll(
                delegate (Passage p)
                {
                    return p.Tags.Contains(tag);
                }
            );

            return results;
        }

        /// <summary>
        /// Returns the first passage with the specified name.
        /// </summary>
        /// <param name="name">The name of the passage to find.</param>
        /// <returns>The passage with the specified name, or <c>null</c> if not found.</returns>
        public Passage GetPassageByName(string name)
        {
            return Passages.Find(
                delegate (Passage p)
                {
                    return p.Name == name;
                }
            )!;
        }

        /// <summary>
        /// Returns a JSON representation of the story following the <see href="https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-jsonoutput-doc.md">Twine 2 JSON Output Specification</see>.
        /// </summary>
        /// <returns>String containing JSON representation of story.</returns>
        // https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-jsonoutput-doc.md
        public string ToJson()
        {
            string combinedStylesheets = string.Join("\n", StoryStylesheets);

            // Find all passages with the tag 'stylesheet'.
            List<Passage> stylesheets = GetPassagesByTag("stylesheet");

            // Combine all the stylesheets into a single string.
            combinedStylesheets += string.Join("\n", stylesheets);

            // Combine all the scripts into a single string.
            string combinedScripts = string.Join("\n", StoryScripts);

            // Find all passages with the tag 'script'.
            List<Passage> scripts = GetPassagesByTag("script");

            // Combine all the scripts into a single string.
            combinedScripts += string.Join("\n", scripts);

            // Create a dictionary to store the story data.
            Dictionary<string, object> data = new()
            {
                { "name", Name },
                { "ifid", IFID },
                { "start", Start },
                { "format", Format },
                { "format-version", FormatVersion },
                { "creator", Creator },
                { "creator-version", CreatorVersion },
                { "zoom", Zoom },
                { "tag-colors", TagColors },
                { "style", combinedStylesheets },
                { "script", combinedScripts },
                { "passages", Passages }
            };

            return JsonSerializer.Serialize(data, JSONSerializePrettyPrint);
        }

        /// <summary>
        /// Returns a Twine 1 HTML representation of the story following the <see href="https://github.com/iftechfoundation/twine-specs/blob/master/twine-1-htmloutput-doc.md">Twine 1 HTML Output Specification</see>.
        /// </summary>
        /// <returns>String containing Twine 1 HTML representation of story.</returns>
        public string ToTwine1HTML()
        {
            // For each passage, generate a Twine 1 HTML string.
            string html = $"<div id=\"storeArea\" data-size=\"{Passages.Count}\">";

            // Append each passage to the Twine 1 HTML string.
            foreach (Passage p in Passages)
            {
                // Add the passage to the Twine 1 HTML string.
                html += p.ToTwine1HTML();
            }

            // Close the storeArea div.
            html += "</div>";

            // Return the Twine 1 HTML string.
            return html;
        }

        /// <summary>
        /// Returns a Twee representation of the story following the <see href="https://github.com/iftechfoundation/twine-specs/blob/master/twee-3-specification.md">Twee 3 Specification</see>.
        /// </summary>
        /// <returns>String containing Twee representation of story.</returns>
        public string ToTwee()
        {
            // For human readability, start with StoryData passage.
            string twee = ":: StoryData\n";

            /**
             * StoryData Passage
             *
             * See: Twee 3 Specification
             * (https://github.com/iftechfoundation/twine-specs/blob/master/twee-3-specification.md#storydata) 
             * 
             * ifid: (string) Required. Maps to <tw-storydata ifid>.
             * format: (string) Optional. Maps to <tw-storydata format>.
             * format-version: (string) Optional. Maps to <tw-storydata format-version>.
             * start: (string) Optional. Maps to <tw-passagedata name> of the node whose pid matches <tw-storydata startnode>.
             * tag-colors: (object of tag(string):color(string) pairs) Optional. Pairs map to <tw-tag> nodes as <tw-tag name>:<tw-tag color>.
             * zoom: (decimal) Optional. Maps to <tw-storydata zoom>.
             */
            Dictionary<string, object> data = [];

            // Verify the IFID is in the proper format.
            if (Babel.IsValidTwineIFID(IFID) == false)
            {
                // Generate a warning message.
                Console.WriteLine("WARN: IFID is not in the proper format. Generating a new IFID.");

                // Generate a new IFID based on uppercase'd GUIDv4.
                IFID = Guid.NewGuid().ToString().ToUpper();
            }

            // Add the IFID to the StoryData passage.
            data.Add("ifid", IFID);

            // Does Format have a value?
            if (!string.IsNullOrEmpty(Format))
            {
                data.Add("format", Format);
            }

            // Does FormatVersion have a value?
            if (!string.IsNullOrEmpty(FormatVersion))
            {
                data.Add("format-version", FormatVersion);
            }

            // Does Start have a value?
            if (!string.IsNullOrEmpty(Start))
            {
                data.Add("start", Start);
            }

            // Does TagColors have a value?
            if (TagColors.Count > 0)
            {
                data.Add("tag-colors", TagColors);
            }

            // Does Zoom have a value?
            if (Zoom >= 1.0f)
            {
                data.Add("zoom", Zoom);
            }

            // Add the StoryData passage to the twee representation.
            // Serialize the data dictionary into JSON using pretty print.
            twee += JsonSerializer.Serialize(data, JSONSerializePrettyPrint);

            // Add two newlines for separation.
            twee += "\n\n";

            // Does Name have a value?
            if (!string.IsNullOrEmpty(Name))
            {
                // Add StoryTitle based on Name.
                twee += $":: StoryTitle\n{Name}\n\n";
            }

            // Add each passage to the twee representation.
            foreach (Passage p in Passages)
            {
                // Add the passage to the twee representation.
                twee += p.ToTwee();

                // Add two newlines for separation.
                twee += "\n\n";
            }

            // Return the twee representation.
            return twee;
        }

        /// <summary>
        /// Returns a Twine 2 HTML representation of the story following the <see href="https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-htmloutput-spec.md">Twine 2 HTML Output Specification</see>.
        /// </summary>
        /// <returns>String containing Twine 2 HTML representation of story.</returns>
        public string ToTwine2HTML()
        {
            // Twine 2 HTML starts with a <tw-storydata> element.
            // See: Twine 2 HTML Output

            // name: (string) Required. The name of the story.
            //
            // Maps to <tw-storydata name>.
            //
            string html = $"<tw-storydata name=\"{Name}\"";

            // ifid: (string) Required. The IFID of the story.
            // ifid: (string) Required. 
            //   An IFID is a sequence of between 8 and 63 characters, 
            //   each of which shall be a digit, a capital letter or a
            //    hyphen that uniquely identify a story (see Treaty of Babel).
            //
            // Maps to <tw-storydata ifid>.
            //
            // Check if IFID is valid.
            // Verify the IFID is in the proper format.
            if (Babel.IsValidTwineIFID(IFID) == false)
            {
                // Generate a warning message.
                Console.WriteLine("WARN: IFID is not in the proper format. Generating a new IFID.");

                // Generate a new IFID based on uppercase'd GUIDv4.
                IFID = Guid.NewGuid().ToString().ToUpper();
            }

            html += $" ifid=\"{IFID}\"";

            // Passage Identification (PID) counter.
            // (Twine 2 starts with 1, so we mirror that.)
            int PIDcounter = 1;

            // Set initial PID value.
            int startPID = 1;

            // We have to do a bit of nonsense here.
            // Twine 2 HTML cares about PID values.
            // We need to set the PID value for the Start passage.

            // For each passage, increment the PID counter until the Start passage is found.
            foreach (Passage p in Passages)
            {
                // If the passage is named 'Start', set the PID value.
                if (p.Name == Start)
                {
                    startPID = PIDcounter;
                }

                // Increment the PID counter.
                PIDcounter++;
            }

            // Check for the case of no passages.
            if (Passages.Count == 0)
            {
                // Set the startPID to 0.
                startPID = 0;
            }

            /**
             * Multiple possible scenarios:
             * 1. No passages. (StartPID is 0.)
             * 2. Start is the first or only passage. (StartPID is 1.)
             * 3. Starting passage is not the first passage. (StartPID is > 1.)
             */

            // startnode: (integer) Optional. The PID of the starting passage.
            html += $" startnode=\"{startPID}\"";

            // creator: (string) Optional. The name of the program that created the story.
            // Maps to <tw-storydata creator>.

            // Check if Creator has a value.
            if (!string.IsNullOrEmpty(Creator))
            {
                html += $" creator=\"{Creator}\"";
            }

            // creator-version: (string) Optional. The version of the program that created the story.
            // Maps to <tw-storydata creator-version>.

            // Check if CreatorVersion has a value.
            if (!string.IsNullOrEmpty(CreatorVersion))
            {
                html += $" creator-version=\"{CreatorVersion}\"";
            }

            // zoom: (decimal) Optional. The zoom level of the story.
            // Maps to <tw-storydata zoom>.

            // Does Zoom have a value?
            if (Zoom >= 1.0f)
            {
                html += $" zoom=\"{Zoom}\"";
            }

            // format: (string) Optional. The format of the story.
            // Maps to <tw-storydata format>.

            // Check if Format has a value.
            if (!string.IsNullOrEmpty(Format))
            {
                html += $" format=\"{Format}\"";
            }

            // format-version: (string) Optional. The version of the format of the story.
            // Maps to <tw-storydata format-version>.

            // Check if FormatVersion has a value.
            if (!string.IsNullOrEmpty(FormatVersion))
            {
                html += $" format-version=\"{FormatVersion}\"";
            }

            // Add the default attributes.
            html += " options hidden>\n";

            // Export the story's stylesheets.
            // <style id="twine-user-stylesheet" type="text/twine-css">body {font-size: 1.5em;}</style>
            foreach (string stylesheet in StoryStylesheets)
            {
                html += $"<style role=\"stylesheet\" id=\"twine-user-stylesheet\" type=\"text/twine-css\">{stylesheet}</style>\n";
            }

            // Look for all passages with the tag 'stylesheet'.
            List<Passage> stylesheets = GetPassagesByTag("stylesheet");

            // If there are stylesheets, add them to the Twine 2 HTML string.
            if (stylesheets.Count > 0)
            {
                // For each stylesheet, add a <style> element.
                foreach (Passage p in stylesheets)
                {
                    // Add the stylesheet to the Twine 2 HTML string.
                    html += $"<style role=\"stylesheet\" id=\"twine-user-stylesheet\" type=\"text/twine-css\">{p.Text}</style>\n";
                }
            }

            // Export the story's scripts.
            // <script id="twine-user-script" type="text/twine-javascript">alert('Hello, world!');</script>
            foreach (string script in StoryScripts)
            {
                html += $"<script role=\"script\" id=\"twine-user-script\" type=\"text/twine-javascript\">{script}</script>\n";
            }

            // Look for all passages with the tag 'script'.
            List<Passage> scripts = GetPassagesByTag("script");

            // If there are scripts, add them to the Twine 2 HTML string.
            if (scripts.Count > 0)
            {
                // For each script, add a <script> element.
                foreach (Passage p in scripts)
                {
                    // Add the script to the Twine 2 HTML string.
                    html += $"<script role=\"script\" id=\"twine-user-script\" type=\"text/twine-javascript\">{p.Text}</script>\n";
                }
            }

            // Add the tag-colors.
            if (TagColors.Count > 0)
            {
                foreach (KeyValuePair<string, string> tagColor in TagColors)
                {
                    html += $"<tw-tag name=\"{tagColor.Key}\" color=\"{tagColor.Value}\" />\n";
                }
            }

            // Reset the PID counter.
            PIDcounter = 1;

            // For each passage, generate a Twine 2 HTML string.
            foreach (Passage p in Passages)
            {
                // Add the passage to the Twine 2 HTML string.
                html += p.ToTwine2HTML(PIDcounter);

                // Increment the PID counter.
                PIDcounter++;
            }

            // Close the tw-storydata element.
            html += "</tw-storydata>";

            return html;
        }

    }
}