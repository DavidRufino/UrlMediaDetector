using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Facebook : BaseChannel
{
    private string FACEBOOK_SOCIALVIDEO = "https://www.facebook.com/plugins/video.php?href=https://www.{0}&show_text=false&width=560";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Facebook
        new(@"^facebook\.com\/photo", ServiceNameEnum.Facebook, MediaEnum.WebRecorded),
        new(@"^facebook\.com\/video\/embed[?]", ServiceNameEnum.Facebook, MediaEnum.WebRecorded),
        new(@"^fb\.gg\/v\/", ServiceNameEnum.Facebook, MediaEnum.WebRecorded),
        new(@"^facebook\.com(\/[A-Za-z-0-9.]+\/[videos]+\/)", ServiceNameEnum.Facebook, MediaEnum.WebRecorded),
        new(@"^facebook\.com\/(watch\/)[?](ref=search&v[=]|v[=])([A-Za-z-0-9.]{1,}).*", ServiceNameEnum.Facebook, MediaEnum.WebRecorded),
        new(@"^facebook\.com\/[A-Za-z-0-9.]+\/(videos)\/[0-9]{11,}$", ServiceNameEnum.Facebook, MediaEnum.WebRecorded), // page facebook gaming
        new(@"^facebook\.com\/(plugins)\/(video)\.php\?", ServiceNameEnum.Facebook, MediaEnum.WebRecorded), // page facebook gaming
    ];

    public Facebook()
    {
        this.ServiceName = ServiceNameEnum.Facebook;
        this.HomePage = "https://www.facebook.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;

        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("facebook.com/plugins/video.php?href=https://www.", "");
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("&show_text=false&width=560", "");
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("?ref=search&v=", "?v=");

        //  Obter o video embed do Facebook
        media.SanitizedUrl = string.Format(FACEBOOK_SOCIALVIDEO, urlWithoutSchemeAndSubdomain);

        //  Exibir o div informado em full screen
        media.Javascript = "var video=document.getElementsByTagName('video')[0];video.requestFullscreen();";

        return await base.OnReplace(media);
    }
}