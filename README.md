<a name="readme-top"></a>

## Table of Contents

<ol>
  <li><a href="#story-compilation">Story Compilation</a></li>
  <li><a href="#format-support">Format Support</a></li>
  <li><a href="#support-functionality">Support Functionality</a></li>
  <li><a href="#license">License</a></li>
</ol>


# libtwee
libtwee is a C# library for creating and parsing current and historic Twine file formats

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
| [ Twine 1 HTML (2006 - 2015) ]( https://github.com/iftechfoundation/twine-specs/blob/master/twine-1-htmloutput-doc.md )          | Yes             | Partial support. |
| [ Twine 1 TWS (2009 - 2015) ]( https://github.com/iftechfoundation/twine-specs/blob/master/twine-1-twsoutput.md )                | Not supported   | Not supported.   |
| [ Twine 2 HTML (2015 - Present) ]( https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-htmloutput-spec.md )      | Yes             | Yes              |
| [ Twine 2 Archive HTML (2015 - Present) ]( https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-archive-spec.md ) | Yes             | Yes              |
| [ Twee 3 (2021 - Present) ]( https://github.com/iftechfoundation/twine-specs/blob/master/twee-3-specification.md )               | Yes             | Yes              |
| [ Twine 2 JSON (2023 - Present) ]( https://github.com/iftechfoundation/twine-specs/blob/master/twine-2-jsonoutput-doc.md )       | Yes             | Yes              |

**Note:** Round-trip translations can present problems because of required fields and properties per format. Some metadata may be added or removed based on the specification being followed.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Support Functionality

Multiple Twine formats support using [an IFID](https://ifdb.org/help-ifid) to identify one work from another.

The Babel class contains methods for verifying and generating a Twine-compatible IFID.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## License

Distributed under the MIT License. See `LICENSE` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>
