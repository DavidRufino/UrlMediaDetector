using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;

namespace UrlMediaDetector.Test;

internal class YoutubeChannel
{
    [TestCase("https://www.youtube.com/shorts/ZXMRHeIY8GE")]
    [TestCase("youtube.com/shorts/ZXMRHeIY8GE")]
    [TestCase("youtube.com/watch?v=xVMsAgHy_IY")]
    [TestCase("https://www.youtube.com/watch?v=xVMsAgHy_IY")]
    public async Task TestRegexPattern(string input)
    {
        var youtubeChannel = new UrlMediaDetector.Services.YoutubeChannel();

        // Find the first matching pattern using regex
        var patternMatch = youtubeChannel.Patterns.FirstOrDefault(p => Regex.IsMatch(input, p.Pattern));

        // Debug output
        TestContext.WriteLine($"Input: {input}");
        TestContext.WriteLine($"ServiceName: {patternMatch?.ToString() ?? null}");
        //TestContext.WriteLine($"Regex Match: {matched} (Expected: {expectedMatch})");

        Assert.AreEqual(ServiceNameEnum.Youtube, patternMatch?.ServiceName ?? null);
    }
}