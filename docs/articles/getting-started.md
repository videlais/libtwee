# Getting Started with libtwee

This guide will help you get up and running with libtwee quickly.

## Installation

### NuGet Package Manager
```bash
Install-Package Videlais.libtwee
```

### .NET CLI
```bash
dotnet add package Videlais.libtwee
```

### Package Reference
```xml
<PackageReference Include="Videlais.libtwee" Version="1.0.0" />
```

## Requirements

- .NET 9.0 or higher
- For HTML parsing: HtmlAgilityPack (included automatically)

## Basic Usage

### 1. Import the Namespace

```csharp
using libtwee;
```

### 2. Parse a Twee Story

```csharp
string tweeContent = @"
:: Start
This is the beginning of your story.

[[Continue->Next]]

:: Next
This is the next passage.

[[Go back->Start]]
";

// Parse the Twee content into a Story object
Story story = Twee.Parse(tweeContent);

Console.WriteLine($"Story has {story.Count} passages");
Console.WriteLine($"Starting passage: {story.Start}");
```

### 3. Generate an IFID

```csharp
// Generate a new Interactive Fiction ID
string ifid = Babel.GenerateTwineIFID();
Console.WriteLine($"New IFID: {ifid}");

// Validate an existing IFID
bool isValid = Babel.IsValidTwineIFID("12345678-1234-1234-1234-123456789ABC");
Console.WriteLine($"IFID is valid: {isValid}");
```

### 4. Compile to HTML

```csharp
// Convert the story to Twine 2 HTML format
string html = story.ToTwine2HTML();

// Save to file
File.WriteAllText("story.html", html);
```

### 5. Work with Passages

```csharp
// Add a new passage
var newPassage = new Passage("End", "Thanks for playing!");
story.AddPassage(newPassage);

// Find a passage
var startPassage = story.GetPassage("Start");
if (startPassage != null)
{
    Console.WriteLine($"Start passage content: {startPassage.Text}");
}

// List all passages
foreach (var passage in story.Passages)
{
    Console.WriteLine($"Passage: {passage.Name}");
}
```

## Complete Example

Here's a complete example that demonstrates the main functionality:

```csharp
using libtwee;

class Program
{
    static void Main()
    {
        // Create a simple Twee story
        string tweeStory = @"
:: StoryTitle
My Adventure

:: StoryData
{
  ""ifid"": ""12345678-1234-5678-9ABC-123456789ABC"",
  ""format"": ""Harlowe"",
  ""format-version"": ""3.3.5""
}

:: Start
You are standing in a forest clearing.

What do you want to do?

* [[Look around->Forest]]
* [[Go north->Cave]]

:: Forest
The forest is dense and mysterious. You see a path leading north.

[[Follow the path->Cave]]
[[Go back->Start]]

:: Cave
You enter a dark cave. You can hear water dripping.

[[Exit the cave->Start]]
";

        try
        {
            // Parse the story
            Story story = Twee.Parse(tweeStory);
            
            Console.WriteLine($"Parsed story: {story.Name}");
            Console.WriteLine($"Format: {story.Format} {story.FormatVersion}");
            Console.WriteLine($"IFID: {story.IFID}");
            Console.WriteLine($"Passages: {story.Count}");
            
            // Generate HTML
            string html = story.ToTwine2HTML();
            
            // Save to file
            File.WriteAllText("my-adventure.html", html);
            Console.WriteLine("Story saved to my-adventure.html");
            
            // Convert to JSON
            string json = story.ToTwine2JSON();
            File.WriteAllText("my-adventure.json", json);
            Console.WriteLine("Story saved to my-adventure.json");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
```

## Next Steps

- Explore more [Examples](examples.md) for advanced usage patterns
- Learn about [File Formats](file-formats.md) supported by libtwee