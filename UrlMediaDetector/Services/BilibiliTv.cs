using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class BilibiliTv : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // BilibiliTv
        new(@"^bilibili(.tv)\/([a-zA-Z]{2}\/)?(play|video\/).*", ServiceNameEnum.BilibiliTv, MediaEnum.WebRecorded),
    ];

    public BilibiliTv()
    {
        this.ServiceName = ServiceNameEnum.BilibiliTv;
        this.HomePage = "https://www.bilibili.tv/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        media.Javascript = @"function goFullScreen(){const e=findFirstElement(['div.player-mobile-display']);if(e)simulateDoubleClick(e),console.log('Double click simulated on:',e);else{console.error('No matching element found.');const e=document.querySelector('video');e?enterFullscreen(e):console.error('No <video> element found.')}}function findFirstElement(e){for(const l of e){const e=document.querySelector(l);if(e)return e}return null}function simulateDoubleClick(e){const l=new MouseEvent('dblclick',{bubbles:!0,cancelable:!0});e.dispatchEvent(l)}function enterFullscreen(e){document.fullscreenElement?console.log('Fullscreen is already active.'):e.requestFullscreen().then((()=>console.log('Element is now fullscreen:',e))).catch((e=>console.error('Error entering fullscreen:',e)))}goFullScreen();";

        return await base.OnReplace(media);
    }
}
