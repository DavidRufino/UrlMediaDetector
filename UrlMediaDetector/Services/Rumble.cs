using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Helpers;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Models.Contexts;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Rumble : BaseChannel
{
    private const string RUMBLE_ENDPOINT = "https://rumble.com/api/Media/oembed.json?url={0}";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Rumble
        new(@"^rumble\.com\/[a-zA-Z0-9.\-]+\.html(\?.*)?", ServiceNameEnum.Rumble, MediaEnum.WebRecorded),
        new(@"^rumble\.com\/(embed)\/[a-zA-Z0-9.\-]+(\/)?", ServiceNameEnum.Rumble, MediaEnum.WebRecorded),
    ];

    public Rumble()
    {
        this.ServiceName = ServiceNameEnum.Rumble;
        this.HomePage = "https://www.rumble.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        string rumbleEndPoint = string.Format(RUMBLE_ENDPOINT, pageUrl);

        // Retrieve the JsonTypeInfo for the RumbleModel type from the source generation context
        // This type information is used to efficiently handle serialization and deserialization without reflection.
        JsonTypeInfo<RumbleModel> typeInfo = JsonContext.Default.RumbleModel;
        var rumbleResult = await HttpFetcherHelper.Instance.GetJsonFromEndpoint<RumbleModel>(rumbleEndPoint, typeInfo);
        if (rumbleResult is not null)
        {
            media.Title = rumbleResult.Title;

            // Regular expression pattern to extract the value of the "src" attribute.
            string rumblePattern = @"<iframe\s+src=""(.*?)""";

            // Attempt to match the pattern in the HTML string.
            Match rumbleMatch = Regex.Match(rumbleResult.Html, rumblePattern);

            // Check if the pattern was found in the HTML string.
            if (rumbleMatch.Success)
            {
                // Get the value of the "src" attribute from the first match group.
                string srcValue = rumbleMatch.Groups[1].Value;
                media.SanitizedUrl = string.Format(srcValue);
            }
            else return null;
        }
        else return null;

        return await base.OnReplace(media);
    }
}
