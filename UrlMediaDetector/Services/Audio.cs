using System.Diagnostics;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Audio : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Audio
        new(@"(http|https).*[\w]+\.(aa|aax|aac|aiff|flac|m4a|m4b|m4p|mp3|mpc|mpp|ogg|oga|wav|wma|wv)(?=[\?\&]?.*$)$", ServiceNameEnum.Audio, MediaEnum.Audio),
        new(@"^[a-zA-Z]{1}:(\\|\/)(?:[^\\/:*?' <>|\r\n]+\\)*[^\\/:*?'<>|\r\n].*\.(aa|aax|aac|aiff|flac|m4a|m4b|m4p|mp3|mpc|mpp|ogg|oga|wav|wma|wv)", ServiceNameEnum.Audio, MediaEnum.Audio),
    ];

    public Audio()
    {
        this.ServiceName = ServiceNameEnum.Audio;
        this.HomePage = null;
        this.UseUrlWithoutSchemeAndSubdomain = false;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia? media)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        if (pageUrl.ToLower().StartsWith("open-pip:"))
        {
            Debug.WriteLine("contains open-pip");
            //media.SanitizedUrl = new string[] { pageUrl };
        }
        else
        {
            Debug.WriteLine("contains playmedia");
            // for custom scheme: open-pip, and without
            pageUrl = pageUrl.Replace("playmedia:", "");

            media.SanitizedUrl = pageUrl;
        }

        return await base.OnReplace(media);
    }
}
