using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Radio : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // JioCinema
         new(@"^(radio[:])", ServiceNameEnum.Radio, MediaEnum.Audio),
    ];

    public Radio()
    {
        this.ServiceName = ServiceNameEnum.Radio;
        this.HomePage = null;
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;

        //  Remover a tag sub-protocol: radio:
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("radio:", "");

        media.SanitizedUrl = urlWithoutSchemeAndSubdomain;

        return await base.OnReplace(media);
    }
}
