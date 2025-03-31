using System.Diagnostics;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Video : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Video
        new(@"^(playmedia[:])", ServiceNameEnum.Video, MediaEnum.Video),
        new(@"(http|https).*[\w]+\.(mkv|ogv|avi|wmv|mp4|mpeg|ts|flv|webm|pls|asx|m3u|m3u8|mov)(?=[\?\&]?.*$)$", ServiceNameEnum.Video, MediaEnum.Video),
        new(@"^[a-zA-Z]{1}:(\\|\/)(?:[^\\/:*?' <>|\r\n]+\\)*[^\\/:*?'<>|\r\n].*\.(mkv|ogv|avi|wmv|mp4|mpeg|ts|flv|webm|pls|asx|m3u|m3u8|mov)", ServiceNameEnum.Video, MediaEnum.Video),
    ];

    public Video()
    {
        this.ServiceName = ServiceNameEnum.Video;
        this.HomePage = null;
        this.UseUrlWithoutSchemeAndSubdomain = false;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
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
