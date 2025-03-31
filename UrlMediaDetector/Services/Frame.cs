using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Frame : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Frame
        new(@"^(mega\.nz\/embed)", ServiceNameEnum.Frame, MediaEnum.WebRecorded),
        new(@"^(yourupload.com\/embed\/)", ServiceNameEnum.Frame, MediaEnum.WebRecorded),
        new(@"^(embedsb\.com\/e\/)", ServiceNameEnum.Frame, MediaEnum.WebRecorded),
        new(@"^(my\.mail\.ru\/video\/embed\/)", ServiceNameEnum.Frame, MediaEnum.WebRecorded),
        new(@"^(dood\.[a-zA-Z]{2}\/[a-z-A-Z]{1}\/)([a-zA-Z0-9]{12,})$", ServiceNameEnum.Frame, MediaEnum.WebRecorded),
        new(@"^(watchsb\.com\/[a-z-A-Z]{1}\/)([a-zA-Z0-9]{12,})$", ServiceNameEnum.Frame, MediaEnum.WebRecorded),
        new(@"^.*(\/)(embed)(\/).*", ServiceNameEnum.Frame, MediaEnum.WebRecorded),
    ];

    public Frame()
    {
        this.ServiceName = ServiceNameEnum.Frame;
        this.HomePage = null;
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        //  Web Video
        if (pageUrl.ToLower().StartsWith("open-pip:"))
        {
            //media.SanitizedUrl = media.SanitizedUrl;
            // do nothing
        }
        else
        {
            // for custom scheme: open-pip, and without
            media.SanitizedUrl = pageUrl;
        }

        return await base.OnReplace(media);
    }
}
