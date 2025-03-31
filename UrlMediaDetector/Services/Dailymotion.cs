using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Dailymotion : BaseChannel
{
    private const string DAILYMOTION_VIDEO = "http://www.dailymotion.com/embed/video/{0}?autoplay=true";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        //  Dailymotion
        new(@"^dailymotion\.com\/video", ServiceNameEnum.Dailymotion, MediaEnum.WebRecorded),
        new(@"^dailymotion\.com\/embed\/video\/", ServiceNameEnum.Dailymotion, MediaEnum.WebRecorded),
        new(@"^dailymotion\.com\/player\/", ServiceNameEnum.Dailymotion, MediaEnum.WebRecorded),
    ];

    public Dailymotion()
    {
        this.ServiceName = ServiceNameEnum.Dailymotion;
        this.HomePage = "https://www.dailymotion.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;

        //dailymotion.com/player/x9yoc.html?video=x8nm9o6&mute=true&loop=false
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("dailymotion.com/embed/video/", "").Trim();
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("dailymotion.com/video/", "").Trim();
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("dailymotion.com/player/", "").Trim();

        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, "(&(?!list).*)", "");

        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @".*\?video=", ""); // Define the first pattern to match up to "?video="
        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @"[?].*", "");
        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @"[&].*", ""); // Define the second pattern to match everything after "&" (including "&")

        media.SanitizedUrl = string.Format(DAILYMOTION_VIDEO, urlWithoutSchemeAndSubdomain);

        media.VideoId = urlWithoutSchemeAndSubdomain;

        return await base.OnReplace(media);
    }
}