# libtwee Documentation

Welcome to the libtwee documentation! This library provides comprehensive support for parsing and compiling current and historic Twine file formats.

## What is libtwee?

libtwee is a C# library designed to work with the Twine ecosystem, supporting:

- **Twee 3** - The latest specification for Twine story source files
- **Twine 2 HTML** - Compiled story output format
- **Twine 2 JSON** - JSON export format for stories
- **Twine 1 HTML** - Legacy HTML format support
- **Twine 2 Archive** - Collection format for multiple stories

## Quick Navigation

### [Getting Started](articles/getting-started.md)

Learn how to install and start using libtwee in your projects.

### [Examples](articles/examples.md)

Practical examples showing common usage patterns.

### [API Reference](articles/api-reference.md)

Complete API documentation for all classes and methods.

### [File Format Guide](articles/file-formats.md)

Understanding the different Twine file formats supported by libtwee.

## Key Features

- **Parse Twee Files** - Convert Twee source code into structured Story objects
- **Compile to HTML** - Generate playable Twine stories in HTML format
- **Format Conversion** - Convert between different Twine file formats
- **IFID Management** - Generate and validate Interactive Fiction IDs
- **High Test Coverage** - 90.5% branch coverage ensuring reliability

## Installation

```bash
dotnet add package Videlais.libtwee
```

## Quick Example

```csharp
using libtwee;

// Parse a Twee story
var story = Twee.Parse(tweeContent);

// Convert to HTML
var html = story.ToTwine2HTML();

// Generate IFID
var ifid = Babel.GenerateTwineIFID();
```