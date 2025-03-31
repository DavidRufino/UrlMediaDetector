using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Vimeo : BaseChannel
{
    private string VIMEO_VIDEO = "https://player.vimeo.com/video/{0}";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Vimeo
        new(@"^vimeo\.com\/[0-9]{8}(?!watch$|watch\/.*$|upgrade).*", ServiceNameEnum.Vimeo, MediaEnum.WebRecorded),
        new(@"^vimeo\.com\/(channels)\/[-a-zA-Z0-9_]+\/[0-9]{1,}", ServiceNameEnum.Vimeo, MediaEnum.WebRecorded),
        new(@"player\.vimeo\.com\/(video\/)", ServiceNameEnum.Vimeo, MediaEnum.WebRecorded),
    ];

    public Vimeo()
    {
        this.ServiceName = ServiceNameEnum.Vimeo;
        this.HomePage = "https://www.vimeo.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;

        // vimeo.com/channels/1842744/823567029?autoplay=1
        // vimeo.com/video/736065868?h=4ccbe9d46b
        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @"^vimeo\.com\/(channels)\/[-a-zA-Z0-9_]+\/", "");
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("vimeo.com/video/", "").Trim();
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("vimeo.com/", "").Trim();
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("player.video/", "").Trim();
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("player.vimeo.com/video/", "").Trim();
        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @"[?].*", "");

        media.SanitizedUrl = string.Format(VIMEO_VIDEO, urlWithoutSchemeAndSubdomain);

        media.Javascript = "var video=document.getElementsByTagName('video')[0];video.requestFullscreen(),video.muted=!1,video.volume=1,video.play(),video.currentTime=.9;";
        media.Javascript += "var video=document.getElementsByTagName('video')[0];video.requestFullscreen();";

        return await base.OnReplace(media);
    }
}