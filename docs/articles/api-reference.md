# API Reference

This document provides a comprehensive reference for the libtwee API.

## Core Classes

### Story Class

The `Story` class represents a complete Twine story with all its passages and metadata.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | The title of the story |
| `IFID` | `string` | Interactive Fiction ID (UUID format) |
| `Start` | `string` | Name of the starting passage |
| `Format` | `string` | Story format name (e.g., "Harlowe") |
| `FormatVersion` | `string` | Version of the story format |
| `Zoom` | `float` | Zoom level for the editor |
| `Passages` | `List<Passage>` | Collection of all passages |
| `Creator` | `string` | Creator application name |
| `CreatorVersion` | `string` | Creator application version |
| `TagColors` | `Dictionary<string, string>` | Tag color mappings |
| `Count` | `int` | Number of passages (read-only) |
| `StoryStylesheets` | `List<string>` | Story-level stylesheets |
| `StoryScripts` | `List<string>` | Story-level scripts |

#### Methods

```csharp
// Constructor
Story()

// Passage management
void AddPassage(Passage passage)
Passage? GetPassage(string name)
bool RemovePassage(string name)

// Format conversion
string ToTwee()
string ToTwine2HTML()
string ToTwine2JSON()
```

#### Example Usage

```csharp
var story = new Story
{
    Name = "My Adventure",
    IFID = Babel.GenerateTwineIFID(),
    Format = "Harlowe",
    FormatVersion = "3.3.5"
};

story.AddPassage(new Passage("Start", "Welcome to the adventure!"));
string html = story.ToTwine2HTML();
```

### Passage Class

The `Passage` class represents a single passage within a story.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | Unique passage identifier |
| `Text` | `string` | Passage content/text |
| `Tags` | `List<string>` | Associated tags |
| `Pid` | `int` | Passage ID number |
| `Position` | `Point?` | Editor position (optional) |

#### Methods

```csharp
// Constructors
Passage()
Passage(string name, string text)
Passage(string name, string text, List<string> tags)

// Tag management
void AddTag(string tag)
bool RemoveTag(string tag)
bool HasTag(string tag)
```

#### Example Usage

```csharp
var passage = new Passage("Forest", "You are in a dark forest.");
passage.AddTag("location");
passage.AddTag("outdoor");

story.AddPassage(passage);
```

## Static Classes

### Twee Class

Static class for parsing and creating Twee format files.

#### Methods

```csharp
// Parse Twee content into a Story object
static Story Parse(string twee)

// Create Twee content from a Story object  
static string Create(Story story)
```

#### Example Usage

```csharp
string tweeContent = File.ReadAllText("story.twee");
Story story = Twee.Parse(tweeContent);

string output = Twee.Create(story);
File.WriteAllText("output.twee", output);
```

### Babel Class

Static class for IFID (Interactive Fiction ID) generation and validation.

#### Methods

```csharp
// Generate a new Twine-compatible IFID
static string GenerateTwineIFID()

// Validate an IFID format
static bool IsValidTwineIFID(string ifid)
```

#### Example Usage

```csharp
// Generate new IFID
string newIFID = Babel.GenerateTwineIFID();
// Output: "A1B2C3D4-E5F6-7890-ABCD-EFGHIJ123456"

// Validate existing IFID
bool isValid = Babel.IsValidTwineIFID("12345678-1234-5678-9ABC-123456789ABC");
// Output: true
```

### Twine2HTML Class

Static class for parsing and compiling Twine 2 HTML format.

#### Methods

```csharp
// Parse Twine 2 HTML into a Story object
static Story Parse(string html)

// Compile Story to Twine 2 HTML
static string Compile(Story story)
```

#### Example Usage

```csharp
string htmlContent = File.ReadAllText("story.html");
Story story = Twine2HTML.Parse(htmlContent);

string compiled = Twine2HTML.Compile(story);
File.WriteAllText("compiled.html", compiled);
```

### Twine2JSON Class

Static class for parsing and creating Twine 2 JSON format.

#### Methods

```csharp
// Parse Twine 2 JSON into a Story object
static Story Parse(string json)

// Create JSON from a Story object
static string Create(Story story)
```

#### Example Usage

```csharp
string jsonContent = File.ReadAllText("story.json");
Story story = Twine2JSON.Parse(jsonContent);

string json = Twine2JSON.Create(story);
File.WriteAllText("output.json", json);
```

### Twine1HTML Class

Static class for parsing legacy Twine 1 HTML format.

#### Methods

```csharp
// Parse Twine 1 HTML into a Story object
static Story Parse(string html)
```

#### Example Usage

```csharp
string legacyHtml = File.ReadAllText("legacy-story.html");
Story story = Twine1HTML.Parse(legacyHtml);

// Convert to modern format
string modernHtml = story.ToTwine2HTML();
```

### Twine2Archive Class

Static class for working with Twine 2 Archive format (story collections).

#### Methods

```csharp
// Parse archive into multiple Story objects
static List<Story> Parse(string archive)

// Create archive from multiple stories
static string Create(List<Story> stories)
```

#### Example Usage

```csharp
string archiveContent = File.ReadAllText("collection.html");
List<Story> stories = Twine2Archive.Parse(archiveContent);

string newArchive = Twine2Archive.Create(stories);
File.WriteAllText("new-collection.html", newArchive);
```

### StoryFormat Class

Static class for working with story format metadata and validation.

#### Methods

```csharp
// Parse story format definition
static StoryFormat Parse(string formatData)

// Validate format compatibility
static bool IsCompatible(string format, string version)
```

## Exception Classes

### EmptyPassageNameException

Thrown when attempting to create a passage with an empty or null name.

```csharp
public class EmptyPassageNameException : Exception
{
    public EmptyPassageNameException(string message) : base(message) { }
}
```

### MissingHTMLElementException

Thrown when required HTML elements are missing during parsing.

```csharp
public class MissingHTMLElementException : Exception
{
    public MissingHTMLElementException(string message) : base(message) { }
}
```

## Usage Patterns

### Error Handling

```csharp
try
{
    Story story = Twee.Parse(content);
    string html = story.ToTwine2HTML();
}
catch (EmptyPassageNameException ex)
{
    Console.WriteLine($"Invalid passage: {ex.Message}");
}
catch (MissingHTMLElementException ex)
{
    Console.WriteLine($"Malformed HTML: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"General error: {ex.Message}");
}
```

### Format Detection

```csharp
public static Story ParseAnyFormat(string content)
{
    // Try different formats in order of likelihood
    try
    {
        if (content.TrimStart().StartsWith("::"))
            return Twee.Parse(content);
        else if (content.Contains("<tw-storydata"))
            return Twine2HTML.Parse(content);
        else if (content.TrimStart().StartsWith("{"))
            return Twine2JSON.Parse(content);
        else
            return Twine1HTML.Parse(content);
    }
    catch (Exception ex)
    {
        throw new ArgumentException($"Unable to parse content: {ex.Message}");
    }
}
```

### Batch Processing

```csharp
public static void ConvertDirectory(string inputDir, string outputDir)
{
    foreach (string file in Directory.GetFiles(inputDir, "*.twee"))
    {
        try
        {
            string content = File.ReadAllText(file);
            Story story = Twee.Parse(content);
            
            string outputFile = Path.ChangeExtension(
                Path.Combine(outputDir, Path.GetFileName(file)), 
                ".html"
            );
            
            File.WriteAllText(outputFile, story.ToTwine2HTML());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing {file}: {ex.Message}");
        }
    }
}
```