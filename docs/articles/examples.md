# Examples

This page contains practical examples demonstrating how to use libtwee for common tasks.

## Basic Story Creation

### Creating a Story from Scratch

```csharp
using libtwee;

// Create a new story
var story = new Story
{
    Name = "My Interactive Fiction",
    IFID = Babel.GenerateTwineIFID(),
    Format = "Harlowe",
    FormatVersion = "3.3.5"
};

// Add passages
story.AddPassage(new Passage("Start", "Welcome to the adventure!\n\n[[Begin->Forest]]"));
story.AddPassage(new Passage("Forest", "You are in a dark forest.\n\n[[Go deeper->Cave]]\n[[Turn back->Start]]"));
story.AddPassage(new Passage("Cave", "You discovered a mysterious cave!\n\nThe End."));

// Set the starting passage
story.Start = "Start";

// Convert to HTML
string html = story.ToTwine2HTML();
File.WriteAllText("my-story.html", html);
```

## File Format Conversions

### Twee to HTML

```csharp
// Read Twee file
string tweeContent = File.ReadAllText("story.twee");

// Parse and convert
Story story = Twee.Parse(tweeContent);
string html = story.ToTwine2HTML();

// Save HTML
File.WriteAllText("story.html", html);
```

### Twee to JSON

```csharp
// Convert Twee to Twine 2 JSON format
string tweeContent = File.ReadAllText("story.twee");
Story story = Twee.Parse(tweeContent);
string json = story.ToTwine2JSON();

File.WriteAllText("story.json", json);
```

### HTML to JSON

```csharp
// Parse existing Twine 2 HTML
string htmlContent = File.ReadAllText("story.html");
Story story = Twine2HTML.Parse(htmlContent);

// Convert to JSON
string json = story.ToTwine2JSON();
File.WriteAllText("converted-story.json", json);
```

## Working with Story Data

### Extracting Story Information

```csharp
string tweeContent = @"
:: StoryTitle
The Lost Temple

:: StoryData
{
  ""ifid"": ""12345678-1234-5678-9ABC-123456789ABC"",
  ""format"": ""Harlowe"",
  ""format-version"": ""3.3.5"",
  ""start"": ""Entrance""
}

:: Entrance
You stand before an ancient temple.
[[Enter->Hall]]

:: Hall  
The hall is dimly lit by torches.
[[Explore->Chamber]]
[[Leave->Entrance]]

:: Chamber
You found a treasure chest!
";

Story story = Twee.Parse(tweeContent);

Console.WriteLine($"Title: {story.Name}");
Console.WriteLine($"IFID: {story.IFID}");
Console.WriteLine($"Format: {story.Format} {story.FormatVersion}");
Console.WriteLine($"Starting passage: {story.Start}");
Console.WriteLine($"Total passages: {story.Count}");

// List all passages
foreach (var passage in story.Passages)
{
    Console.WriteLine($"- {passage.Name}: {passage.Text.Length} characters");
}
```

### Adding Metadata

```csharp
var story = new Story();

// Set basic metadata
story.Name = "Adventure Game";
story.IFID = Babel.GenerateTwineIFID();
story.Creator = "Twine";
story.CreatorVersion = "2.8.1";

// Add tag colors
story.TagColors["important"] = "red";
story.TagColors["optional"] = "blue";

// Add stylesheets
story.StoryStylesheets.Add("body { background-color: #333; color: white; }");

// Add scripts
story.StoryScripts.Add("console.log('Story loaded');");
```

## Advanced Passage Manipulation

### Finding and Modifying Passages

```csharp
Story story = Twee.Parse(File.ReadAllText("story.twee"));

// Find a specific passage
var startPassage = story.GetPassage("Start");
if (startPassage != null)
{
    // Modify passage content
    startPassage.Text += "\n\n[[New option->Secret]]";
    
    // Add tags
    startPassage.Tags.Add("modified");
}

// Add a new passage
var secretPassage = new Passage("Secret", "You found a secret area!");
secretPassage.Tags.Add("hidden");
story.AddPassage(secretPassage);
```

### Working with Passage Tags

```csharp
string tweeWithTags = @"
:: Start [beginning important]
This is the start of the story.

:: Forest [location outdoor]
You are in a forest.

:: Cave [location indoor dark]
A dark cave entrance.
";

Story story = Twee.Parse(tweeWithTags);

// Find passages with specific tags
var locationPassages = story.Passages
    .Where(p => p.Tags.Contains("location"))
    .ToList();

Console.WriteLine("Location passages:");
foreach (var passage in locationPassages)
{
    Console.WriteLine($"- {passage.Name}: {string.Join(", ", passage.Tags)}");
}

// Find outdoor locations
var outdoorPassages = story.Passages
    .Where(p => p.Tags.Contains("outdoor"))
    .ToList();
```

## IFID Management

### Generate and Validate IFIDs

```csharp
// Generate a new IFID
string newIFID = Babel.GenerateTwineIFID();
Console.WriteLine($"Generated IFID: {newIFID}");

// Validate IFIDs
string[] testIFIDs = {
    "12345678-1234-5678-9ABC-123456789ABC", // Valid
    "invalid-ifid",                         // Invalid
    "12345678-1234-5678-9ABC-123456789abc", // Valid (mixed case)
    ""                                      // Invalid (empty)
};

foreach (string ifid in testIFIDs)
{
    bool isValid = Babel.IsValidTwineIFID(ifid);
    Console.WriteLine($"IFID '{ifid}': {(isValid ? "Valid" : "Invalid")}");
}
```

### Ensuring Story Has Valid IFID

```csharp
Story story = Twee.Parse(tweeContent);

// Check if story has a valid IFID
if (string.IsNullOrEmpty(story.IFID) || !Babel.IsValidTwineIFID(story.IFID))
{
    // Generate a new IFID if missing or invalid
    story.IFID = Babel.GenerateTwineIFID();
    Console.WriteLine($"Assigned new IFID: {story.IFID}");
}
else
{
    Console.WriteLine($"Story has valid IFID: {story.IFID}");
}
```

## Error Handling

### Robust Parsing with Error Handling

```csharp
public static Story SafeParse(string content, string filename)
{
    try
    {
        Story story = Twee.Parse(content);
        
        // Validate the parsed story
        if (story.Count == 0)
        {
            throw new InvalidOperationException("Story contains no passages");
        }
        
        if (string.IsNullOrEmpty(story.Start))
        {
            // Set first passage as start if not specified
            story.Start = story.Passages.First().Name;
            Console.WriteLine($"Warning: No start passage specified, using '{story.Start}'");
        }
        
        return story;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error parsing {filename}: {ex.Message}");
        throw;
    }
}

// Usage
try
{
    string content = File.ReadAllText("story.twee");
    Story story = SafeParse(content, "story.twee");
    Console.WriteLine("Story parsed successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to process story: {ex.Message}");
}
```

## Batch Processing

### Converting Multiple Files

```csharp
public static void ConvertTweeToHTML(string inputDirectory, string outputDirectory)
{
    var tweeFiles = Directory.GetFiles(inputDirectory, "*.twee");
    
    foreach (string tweeFile in tweeFiles)
    {
        try
        {
            string content = File.ReadAllText(tweeFile);
            Story story = Twee.Parse(content);
            
            string filename = Path.GetFileNameWithoutExtension(tweeFile);
            string outputPath = Path.Combine(outputDirectory, $"{filename}.html");
            
            string html = story.ToTwine2HTML();
            File.WriteAllText(outputPath, html);
            
            Console.WriteLine($"Converted: {tweeFile} -> {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error converting {tweeFile}: {ex.Message}");
        }
    }
}

// Usage
ConvertTweeToHTML("./twee-stories", "./html-stories");
```