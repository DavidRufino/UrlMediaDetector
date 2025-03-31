# Url Media Detector .NET 8

## Overview

Url Media Detector is a .NET 8 open-source library designed to detect and process media URLs from various services. It provides an extensible architecture that allows developers to add support for new platforms by implementing service classes in the `Services` folder.

## Features

- Detects and processes URLs from different media services
- Additional services can be added

## Usage

### Adding a New Media Service

To add a new service, create a class in the `Services` folder that inherits from `BaseChannel`. Below is an example implementation for YouTube:

```csharp
namespace UrlMediaDetector.Services;

public class YoutubeChannel : BaseChannel
{
    private string YOUTUBEAPI_VIDEO = "";
    private string YOUTUBE_CHAT = "https://www.youtube.com/live_chat?is_popout=1&v={0}";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        new(@"^(youtube\.com|youtu\.be)\/.*", ServiceNameEnum.Youtube, MediaEnum.WebRecorded)
    ];

    public YoutubeChannel()
    {
        this.ServiceName = ServiceNameEnum.Youtube;
        this.HomePage = "https://www.youtube.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        // Processing logic here
        return await base.OnReplace(media);
    }
}
```

### Running Unit Tests

Unit tests for services should be placed in the `UrlMediaDetector.Test` project. Here’s an example of how you can create a test for `YoutubeChannel`:

```csharp
namespace UrlMediaDetector.Test;

public class YoutubeChannelTests
{
    [TestCase("https://www.youtube.com/watch?v=xVMsAgHy_IY")]
    public void YoutubeUrl_ShouldBeDetectedCorrectly()
    {
        var youtubeChannel = new UrlMediaDetector.Services.YoutubeChannel();

        // Find the first matching pattern using regex
        var patternMatch = youtubeChannel.Patterns.FirstOrDefault(p => Regex.IsMatch(input, p.Pattern));

        // Debug output
        TestContext.WriteLine($"Input: {input}");
        TestContext.WriteLine($"ServiceName: {patternMatch?.ToString() ?? null}");

        Assert.AreEqual(ServiceNameEnum.Youtube, patternMatch?.ServiceName ?? null);
    }
}
```

## Contributing

Contributions are welcome! Please fork the repository, create a feature branch, and submit a pull request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
