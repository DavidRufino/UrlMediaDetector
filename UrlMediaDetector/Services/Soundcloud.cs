using System.Net;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Soundcloud : BaseChannel
{
    /// <summary>
    /// https://developers.soundcloud.com/docs/api/guide
    /// </summary>
    private string SOUNDCLOUND_API = "";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        //  Soundcloud
        new(@"^(w\.)?(soundcloud\.com)\/(?!people\/(directory)|search|signin|notifications(?!\/)|terms-of-use(?!\/)|discover(?!\/)|messages(?!\/)|stream(?!\/)|jobs(?!\/)|upload(?!\/)|imprint(?!\/)|charts\/top|pages\/(privacy|cookies|contact)|you\/(library|likes|sets|albums|stations|following|history)|$).*", ServiceNameEnum.Soundcloud, MediaEnum.WebRecorded),
    ];

    public Soundcloud()
    {
        this.ServiceName = ServiceNameEnum.Soundcloud;
        this.HomePage = "https://soundcloud.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        media.SanitizedUrl = string.Format(SOUNDCLOUND_API, WebUtility.UrlEncode(pageUrl));

        return await base.OnReplace(media);
    }
}
