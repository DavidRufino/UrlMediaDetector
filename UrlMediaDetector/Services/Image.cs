using System.Diagnostics;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Image : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Image
        new(@"^(imagemedia[:](?!data))", ServiceNameEnum.Image, MediaEnum.Image),
        new(@"(http|https).*[\w]+\.(jpg|jpeg|png|gif|bmp|webp)(?=[\?\&]?.*$)$", ServiceNameEnum.Image, MediaEnum.Image),
    ];

    public Image()
    {
        this.ServiceName = ServiceNameEnum.Image;
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
            pageUrl = pageUrl.Replace("imagemedia:", "");

            media.SanitizedUrl = pageUrl;
        }

        return await base.OnReplace(media);
    }
}
