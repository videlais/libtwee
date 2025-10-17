# File Formats Guide

libtwee supports multiple file formats used in the Twine ecosystem. This guide explains each format and how to work with them.

## Supported Formats

| Format | Input | Output | Description |
|--------|--------|--------|-------------|
| Twee 3 | ✅ | ✅ | Latest specification for Twine source files |
| Twine 2 HTML | ✅ | ✅ | Compiled playable story format |
| Twine 2 JSON | ✅ | ✅ | JSON export format for stories |
| Twine 2 Archive | ✅ | ✅ | Collection format for multiple stories |
| Twine 1 HTML | ✅ | 🟡 | Legacy HTML format (partial output support) |

## Twee 3 Format

Twee 3 is the primary source format for Twine stories. It's a plain text format that's human-readable and version control friendly.

### Basic Structure

```twee
:: StoryTitle
My Adventure Story

:: StoryData
{
  "ifid": "12345678-1234-5678-9ABC-123456789ABC",
  "format": "Harlowe",
  "format-version": "3.3.5",
  "start": "Introduction"
}

:: Introduction
Welcome to my adventure story!

This is where your journey begins.

[[Start the adventure->Forest]]

:: Forest [location outdoor]
You are standing in a dense forest.

The trees tower above you, their leaves rustling in the wind.

* [[Go deeper into the forest->DeepForest]]
* [[Follow the path->Path]]
* [[Turn back->Introduction]]

:: DeepForest [location outdoor hidden]
You venture deeper into the mysterious forest...
```

### Passage Headers

Passages in Twee format follow this pattern:
```
:: PassageName [tag1 tag2] {"position":"100,200"}
```

- **PassageName**: Unique identifier for the passage
- **[tags]**: Optional space-separated tags
- **{"metadata"}**: Optional JSON metadata (position, etc.)

### Story Metadata

The `StoryData` passage contains JSON configuration:

```json
{
  "ifid": "Interactive Fiction ID",
  "format": "Story format name",
  "format-version": "Format version",
  "start": "Starting passage name",
  "tag-colors": {
    "important": "red",
    "optional": "blue"
  },
  "zoom": 1.0
}
```

### Working with Twee

```csharp
// Parse Twee content
string tweeContent = File.ReadAllText("story.twee");
Story story = Twee.Parse(tweeContent);

// Create Twee from Story object
string tweeOutput = story.ToTwee();
File.WriteAllText("output.twee", tweeOutput);
```

## Twine 2 HTML Format

The compiled HTML format contains both the story data and the story format engine, creating a standalone playable file.

### Structure

Twine 2 HTML files contain:
- HTML document structure
- Story data in a `<tw-storydata>` element
- Story format JavaScript
- Passages as `<tw-passagedata>` elements

### Example HTML Structure

```html
<!DOCTYPE html>
<html>
<head>
    <title>My Story</title>
</head>
<body>
    <tw-storydata name="My Story" startnode="1" 
                  ifid="12345678-1234-5678-9ABC-123456789ABC"
                  format="Harlowe" format-version="3.3.5">
        <tw-passagedata pid="1" name="Start" tags="">
            Welcome to the story!
            
            [[Continue->Next]]
        </tw-passagedata>
        <tw-passagedata pid="2" name="Next" tags="">
            This is the next passage.
        </tw-passagedata>
    </tw-storydata>
    
    <!-- Story format JavaScript would be here -->
</body>
</html>
```

### Working with Twine 2 HTML

```csharp
// Parse HTML file
string htmlContent = File.ReadAllText("story.html");
Story story = Twine2HTML.Parse(htmlContent);

// Compile story to HTML
string html = story.ToTwine2HTML();
File.WriteAllText("compiled.html", html);
```

## Twine 2 JSON Format

JSON format provides a structured data representation of stories, useful for tool integration and data processing.

### JSON Structure

```json
{
  "name": "My Adventure Story",
  "ifid": "12345678-1234-5678-9ABC-123456789ABC",
  "format": "Harlowe",
  "format-version": "3.3.5",
  "start": "Introduction",
  "passages": [
    {
      "name": "Introduction",
      "tags": ["beginning"],
      "text": "Welcome to my adventure!\n\n[[Start->Forest]]",
      "pid": 1
    },
    {
      "name": "Forest",
      "tags": ["location", "outdoor"],
      "text": "You are in a forest.\n\n[[Continue->End]]",
      "pid": 2
    }
  ],
  "creator": "Twine",
  "creator-version": "2.8.1"
}
```

### Working with JSON

```csharp
// Parse JSON file
string jsonContent = File.ReadAllText("story.json");
Story story = Twine2JSON.Parse(jsonContent);

// Export to JSON
string json = story.ToTwine2JSON();
File.WriteAllText("exported.json", json);
```

## Twine 2 Archive Format

Archive format allows bundling multiple stories into a single HTML file for sharing story collections.

### Archive Structure

```html
<tw-library>
    <tw-story name="Story 1" ifid="...">
        <!-- Story 1 data -->
    </tw-story>
    <tw-story name="Story 2" ifid="...">
        <!-- Story 2 data -->
    </tw-story>
</tw-library>
```

### Working with Archives

```csharp
// Parse archive
string archiveContent = File.ReadAllText("collection.html");
var stories = Twine2Archive.Parse(archiveContent);

// Create archive from multiple stories
var storyList = new List<Story> { story1, story2, story3 };
string archive = Twine2Archive.Create(storyList);
File.WriteAllText("collection.html", archive);
```

## Legacy Twine 1 HTML

libtwee provides parsing support for legacy Twine 1 HTML files, allowing migration to newer formats.

### Differences from Twine 2

- Uses `<div>` elements instead of custom elements
- Different data attribute names
- Simpler structure
- Limited metadata support

### Migration Example

```csharp
// Parse legacy Twine 1 HTML
string twine1Html = File.ReadAllText("legacy-story.html");
Story story = Twine1HTML.Parse(twine1Html);

// Convert to modern Twine 2 format
string twine2Html = story.ToTwine2HTML();
File.WriteAllText("modernized-story.html", twine2Html);
```

## Format Conversion Examples

### Complete Conversion Workflow

```csharp
public class StoryConverter
{
    public static void ConvertBetweenFormats(string inputFile, string outputFile)
    {
        Story story = null;
        string extension = Path.GetExtension(inputFile).ToLower();
        
        // Parse input based on file extension
        string content = File.ReadAllText(inputFile);
        
        switch (extension)
        {
            case ".twee":
                story = Twee.Parse(content);
                break;
            case ".html":
                // Try Twine 2 first, fall back to Twine 1
                try
                {
                    story = Twine2HTML.Parse(content);
                }
                catch
                {
                    story = Twine1HTML.Parse(content);
                }
                break;
            case ".json":
                story = Twine2JSON.Parse(content);
                break;
            default:
                throw new ArgumentException($"Unsupported input format: {extension}");
        }
        
        // Generate output based on desired format
        string outputExtension = Path.GetExtension(outputFile).ToLower();
        string output;
        
        switch (outputExtension)
        {
            case ".twee":
                output = story.ToTwee();
                break;
            case ".html":
                output = story.ToTwine2HTML();
                break;
            case ".json":
                output = story.ToTwine2JSON();
                break;
            default:
                throw new ArgumentException($"Unsupported output format: {outputExtension}");
        }
        
        File.WriteAllText(outputFile, output);
        Console.WriteLine($"Converted {inputFile} to {outputFile}");
    }
}

// Usage examples
StoryConverter.ConvertBetweenFormats("story.twee", "story.html");
StoryConverter.ConvertBetweenFormats("story.html", "story.json");
StoryConverter.ConvertBetweenFormats("legacy.html", "modern.twee");
```

## Best Practices

### Format Selection

- **Use Twee 3** for source control and human editing
- **Use HTML** for distribution and playing
- **Use JSON** for tool integration and data processing
- **Use Archives** for story collections

### File Organization

```
project/
├── src/
│   ├── story.twee          # Source files
│   └── chapters/
│       ├── intro.twee
│       └── adventure.twee
├── build/
│   ├── story.html          # Compiled output
│   └── story.json          # Data export
└── assets/
    ├── images/
    └── styles/
```

### Version Control

Twee format is ideal for version control:

```bash
# Track source files
git add src/*.twee

# Ignore build artifacts
echo "build/" >> .gitignore
echo "*.html" >> .gitignore
```