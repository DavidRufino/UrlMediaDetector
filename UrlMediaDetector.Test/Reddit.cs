using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;

namespace UrlMediaDetector.Test;

internal class Reddit
{
    [TestCase("reddit.com/r/law/comments/1jh3qd3/just_in_elon_musk_announces_he_is_launching_a/")]
    [TestCase("https://www.reddit.com/r/law/comments/1jh3qd3/just_in_elon_musk_announces_he_is_launching_a/")]
    [TestCase("https://old.reddit.com/r/law/comments/1jh3qd3/just_in_elon_musk_announces_he_is_launching_a/")]
    [TestCase("https://reddit.com/r/law/comments/1jh3qd3/just_in_elon_musk_announces_he_is_launching_a/")]
    [TestCase("https://www.reddit.com/r/BeAmazed/comments/1jgt0f1/conversation_pits_were_a_popular_home_feature/#lightbox")]
    [TestCase("https://www.reddit.com/r/maybemaybemaybe/comments/1jh7hv7/maybe_maybe_maybe/")]
    [TestCase("https://preview.redd.it/blah.jpg?s=39fiuoe298rhiw")]
    [TestCase("old.reddit.com/r/law/comments/1jh3qd3/just_in_elon_musk_announces_he_is_launching_a/")]
    public async Task TestRegexPattern(string input)
    {
        var reddit = new UrlMediaDetector.Services.Reddit();

        // Find the first matching pattern using regex
        var patternMatch = reddit.Patterns.FirstOrDefault(p => Regex.IsMatch(input, p.Pattern));

        // Debug output
        TestContext.WriteLine($"Input: {input}");
        TestContext.WriteLine($"ServiceName: {patternMatch?.ToString() ?? null}");
        //TestContext.WriteLine($"Regex Match: {matched} (Expected: {expectedMatch})");

        Assert.AreEqual(ServiceNameEnum.Reddit, patternMatch?.ServiceName ?? null);
    }
}