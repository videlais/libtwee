<a name="readme-top"></a>

# libtwee

[![.NET Build](https://github.com/videlais/libtwee/actions/workflows/dotnet.yml/badge.svg)](https://github.com/videlais/libtwee/actions/workflows/dotnet.yml)
[![NuGet Version](https://img.shields.io/nuget/v/Videlais.libtwee)](https://www.nuget.org/packages/Videlais.libtwee/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Videlais.libtwee)](https://www.nuget.org/packages/Videlais.libtwee/)
[![Code Coverage](https://img.shields.io/badge/Coverage-90.5%25-brightgreen)](https://github.com/videlais/libtwee/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)

A C# library for creating and parsing current and historic Twine file formats.

## Table of Contents

<ol>
  <li><a href="#installation">Installation</a></li>
  <li><a href="#quick-start">Quick Start</a></li>
  <li><a href="#story-compilation">Story Compilation</a></li>
  <li><a href="#format-support">Format Support</a></li>
  <li><a href="#support-functionality">Support Functionality</a></li>
  <li><a href="#contributing">Contributing</a></li>
  <li><a href="#license">License</a></li>
</ol>

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

## Quick Start

```csharp
using libtwee;

// Parse a Twee 3 file
var story = Twee.Parse(tweeContent);

// Convert to Twine 2 HTML
var html = story.ToTwine2HTML();

// Generate a new IFID
var ifid = Babel.GenerateTwineIFID();
```

## Documentation

📚 **[Complete Documentation](docs/index.md)** - Comprehensive guides and API reference

- **[Getting Started Guide](docs/articles/getting-started.md)** - Installation and basic usage
- **[Examples](docs/articles/examples.md)** - Practical code examples  
- **[API Reference](docs/articles/api-reference.md)** - Complete API documentation
- **[File Formats Guide](docs/articles/file-formats.md)** - Understanding Twine formats

### Building Documentation Locally

```bash
# Install DocFX (if not already installed)
dotnet tool install -g docfx

# Build documentation
./build-docs.sh

# Or manually:
cd docs && docfx docfx.json
```

## Requirements

- .NET 9.0 or higher

## Story Compilation

The process of *story compilation* converts human-readable content, what is generally called a *story*, into a playable format such as HyperText Markup Language (HTML). The result is then presented as links or other visual, interactive elements for another party to interact with to see its content.

The term *compilation* is used because different parts of code are put together in a specific arrangement to enable later play. As part of Twine-compatible HTML, this means combining JavaScript code (generally a "story format") with story HTML data.

Playable formats are the following and require external story formats[^1] to enable play:

- Twine 2 HTML
- Twine 1 HTML

More human-readable formats include:

- Twee 3[^2]
- Twine 2 JSON[^3]

From 2009 to 2015, Twine 1 supported a now [historical format named TWS](https://github.com/iftechfoundation/twine-specs/blob/master/twine-1-twsoutput.md). libtwee does not support this.

Twine 2 supports exporting a collection of stories (known as a *library*) in the [Twine 2 Archive HTML format](https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-archive-spec.md).

[^1]: Story formats can be found in the [Story Format Archive (SFA)](https://github.com/videlais/story-formats-archive).

[^2]: Twee exists in three versions. The first existed between 2006 to 2009 and was part of Twine 1. The move to Twine 2 in 2009 dropped support and the story compilation tools [Twee2](https://dan-q.github.io/twee2/) and [Tweego](https://www.motoslave.net/tweego/) adopted their own extensions and modifications. Beginning in 2017, work was done to unite the different projects. This resulted in [Twee 3](https://github.com/iftechfoundation/twine-specs/blob/master/twee-3-specification.md) in March 2021.

[^3]: In October 2023, JavaScript Object Notation (JSON) was added as a supported community format for story compilation tools.

## Format Support

| Format                                                                                                                           | Input           | Output           |
|----------------------------------------------------------------------------------------------------------------------------------|-----------------|------------------|
| [Twine 1 HTML (2006 - 2015)]( https://github.com/iftechfoundation/twine-specs/blob/master/twine-1-htmloutput-doc.md )          | Yes             | Partial support. |
| [Twine 1 TWS (2009 - 2015)]( https://github.com/iftechfoundation/twine-specs/blob/master/twine-1-twsoutput.md )                | Not supported   | Not supported.   |
| [Twine 2 HTML (2015 - Present)]( https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-htmloutput-spec.md )      | Yes             | Yes              |
| [Twine 2 Archive HTML (2015 - Present)]( https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-archive-spec.md ) | Yes             | Yes              |
| [Twee 3 (2021 - Present)]( https://github.com/iftechfoundation/twine-specs/blob/master/twee-3-specification.md )               | Yes             | Yes              |
| [Twine 2 JSON (2023 - Present)]( https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-jsonoutput-doc.md )       | Yes             | Yes              |

**Note:** Round-trip translations can present problems because of required fields and properties per format. Some metadata may be added or removed based on the specification being followed.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Support Functionality

Multiple Twine formats support using [an IFID](https://ifdb.org/help-ifid) to identify one work from another.

The `Babel` class contains methods for verifying and generating a Twine-compatible IFID:

```csharp
// Generate a new IFID
string ifid = Babel.GenerateIFID();

// Validate an existing IFID
bool isValid = Babel.ValidateIFID("12345678-1234-1234-1234-123456789ABC");
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

### Development Setup

1. Clone the repository
2. Ensure you have .NET 9.0 SDK installed
3. Run `dotnet restore` to install dependencies
4. Run `dotnet build` to build the project
5. Run `dotnet test` to run the test suite

### Code Coverage

This project maintains high code coverage standards. Run the following to generate a coverage report:

```bash
./coverage.sh
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## License

Distributed under the MIT License. See `LICENSE` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
