using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Youku : BaseChannel
{
    private string YOUKU_VIDEO = "https://player.youku.com/embed/{0}";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        //  Youku
        new(@"^youku\.(com)(\/v_show)", ServiceNameEnum.Youku, MediaEnum.WebRecorded),
        new(@"^youku\.(com)(\/user_show)", ServiceNameEnum.Youku, MediaEnum.WebRecorded),
        new(@"^youku\.(com)(\/v_nextstage)", ServiceNameEnum.Youku, MediaEnum.WebRecorded),
        new(@"^youku\.(com)(\/video)", ServiceNameEnum.Youku, MediaEnum.WebRecorded),
    ];

    public Youku()
    {
        this.ServiceName = ServiceNameEnum.Youku;
        this.HomePage = "https://www.youku.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        /*urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, "(.html[A-Za-z0-9._%+-=&?]*)", "");
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("youku.com/v_show/id_", "").Trim();
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("youku.com/v_nextstage/id_", "").Trim();
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("player.youku.com/embed/", "").Trim();
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("youku.com/user_show/id_", "").Trim();
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("youku.com/video/id_", "").Trim();
        urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("youku.com/u/", "").Trim();
        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, "([A-Za-z0-9._%+-=&?]+id_)", "");

        media.SanitizedUrl = string.Format(YOUKU_VIDEO, urlWithoutSchemeAndSubdomain);*/
        //identifyData.VIDEO_URL = new string[] { pageUrl };

        //media.ApplyFullScreen = true;
        media.Javascript = @"!function(){function isFullscreen(){return!!document.fullscreenElement}
!function goFullScreen(){
!
function fullscreenClick(){const e=function findFirstElement(e){for(const n of e){const e=document.querySelector(n);if(e)return e}return null}(['icon#fullscreen-icon']);if(e&&!isFullscreen()){const n=new MouseEvent('click',{bubbles:!0,cancelable:!0});e.dispatchEvent(n)}}();
const e=document.querySelector('video');e&&(
isFullscreen()||function enterFullscreen(e){isFullscreen()||e.requestFullscreen().then((()=>{})).catch((e=>{}))}(e))}()}();";

        return await base.OnReplace(media);
    }
}
