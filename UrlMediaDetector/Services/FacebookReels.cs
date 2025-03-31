using System.Diagnostics;
using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class FacebookReels : BaseChannel
{
    private string FACEBOOKREELS_VIDEO = "https://www.facebook.com/reel/{0}";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        //  FacebookReels
        new(@"^facebook\.com\/reel\/[0-9]{0,}", ServiceNameEnum.FacebookReels, MediaEnum.WebRecorded),
        new(@"^facebook\.com\/reel", ServiceNameEnum.FacebookReels, MediaEnum.WebRecorded),
    ];

    public FacebookReels()
    {
        this.ServiceName = ServiceNameEnum.FacebookReels;
        this.HomePage = "https://www.facebook.com/reel/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;

        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @"^facebook.com/reel/", "");
        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @"^facebook.com/reel", "");

        //  Sample Enter Url
        //URL: https://www.facebook.com/reel/{VIDEO-ID}
        //ex: https://www.facebook.com/reel/822241495469936

        //  Obter o video embed do Facebook
        urlWithoutSchemeAndSubdomain = string.Format(FACEBOOKREELS_VIDEO, urlWithoutSchemeAndSubdomain);

        Debug.WriteLine("[FacebookReels] Facebook Reels pageUrl " + urlWithoutSchemeAndSubdomain);
        Debug.WriteLine("[FacebookReels] Facebook Reels defaultUrl " + urlWithoutSchemeAndSubdomain);

        media.SanitizedUrl = urlWithoutSchemeAndSubdomain;

        //  Exibir o div informado em full screen
        media.Javascript = "var videocontentreels=document.querySelector(\"div[class='xjbqb8w x1lq5wgf xgqcy7u x30kzoy x9jhf4c x78zum5 x1q0g3np xod5an3 x14vqqas x6ikm8r x10wlt62 x1n2onr6 x1k90msu x6o7n8i x9lcvmn x1m6m0jg']\"),ControlVideoReelsBtn=document.querySelectorAll(\"div[class='x1ypdohk x5yr21d x1n2onr6 x3hqpx7 xg01cxk x47corl'],div[class='x1ypdohk x5yr21d x1n2onr6 x3hqpx7'], div[class='x5yr21d x1n2onr6 x3hqpx7 xg01cxk x47corl'], div[class='x5yr21d x1n2onr6 x3hqpx7']\"),previousVideoReelsBtn=ControlVideoReelsBtn[0]?.children[0]??document.querySelector(\"div[aria-label='Previous Card']\"),nextVideoReelsBtn=ControlVideoReelsBtn[1]?.children[0]??document.querySelector(\"div[aria-label='Next Card']\");null!=previousVideoReelsBtn&&(previousVideoReelsBtn.style.display=\"none\"),null!=nextVideoReelsBtn&&(nextVideoReelsBtn.style.display=\"none\");let masterHeader=null;for(let e=0;e<document.body.children.length;e++)if(\"DIV\"===document.body.children[e].tagName){masterHeader=document.body.children[e];break}masterHeader.innerHTML=\"\",masterHeader.appendChild(videocontentreels),null!=nextVideoReelsBtn&&masterHeader.appendChild(nextVideoReelsBtn),null!=previousVideoReelsBtn&&masterHeader.appendChild(previousVideoReelsBtn),masterHeader.style.margin=0,masterHeader.style.height=\"100%\",document.documentElement.style.margin=0,document.documentElement.style.height=\"100%\",document.body.style.margin=0,document.body.style.height=\"100%\",videocontentreels.style.margin=0,videocontentreels.style.height=\"100%\",videocontentreels.style.width=\"100%\";var videocontrolsreels=document.querySelector(\"div[class='x1i10hfl x1qjc9v5 xjbqb8w xjqpnuy xa49m3k xqeqjp1 x2hbi6w x13fuv20 xu3j5b3 x1q0q8m5 x26u7qi x972fbf xcfux6l x1qhh985 xm0m39n x9f619 x1ypdohk xdl72j9 x2lah0s xe8uvvx xdj266r x11i5rnm xat24cr x1mh8g0r x2lwn1j xeuugli xexx8yu x4uap5 x18d9i69 xkhd6sd x1n2onr6 x16tdsg8 x1hl2dhg xggy1nq x1ja2u2z x1t137rt x1o1ewxj x3x9cwd x1e5q0jg x13rtm0m x3nfvp2 x1q0g3np x87ps6o x1lku1pv x1a2a7pz']\");null!=videocontrolsreels?.className&&(videocontrolsreels.className=\"\");var videocontrolsreelstwo=document.querySelector(\"div[class='x78zum5 x5yr21d xl56j7k x1n2onr6 xh8yej3']\");null!=videocontrolsreelstwo?.className&&(videocontrolsreelstwo.className=\"\");var mastervideointervalId=window.setInterval((function(){var e=document.querySelector(\"div[class='x6s0dn4 x18l40ae x5yr21d x1n2onr6 xh8yej3']\");null==e?.className||(e.className=\"\")}),1e3);window.addEventListener(\"wheel\",(e=>{e.preventDefault();Math.sign(e.deltaY)<1?nextVideoReelsBtn?.click():previousVideoReelsBtn?.click(),clearInterval(l);let l=setInterval((()=>{let e=document.querySelector(\"div[data-video-id]\");if(e){let t=e.getAttribute(\"data-video-id\");t&&(window.chrome.webview.postMessage(`https://www.facebook.com/reel/${t}`),clearInterval(l))}}),1e3)}));";
        //media.Javascript += " window.addEventListener(\"wheel\",(e=>{Math.sign(e.deltaY)<1?nextVideoReelsBtn.click():previousVideoReelsBtn.click(),clearInterval(t);var t=window.setInterval((function(){var e=document.querySelector(\"div[data-video-id]\");if(null!=e){var i=e?.getAttribute(\"data-video-id\");window.chrome.webview.postMessage(\"https://www.facebook.com/reel/\"+i),clearInterval(t)}}),1e3)}));";
        //video.ScriptChatFullScreen = "var videomasterreels = document.querySelector(\"div[class='x6s0dn4 x18l40ae x5yr21d x1n2onr6 xh8yej3']\");videomasterreels.className = '';";
        //video.ScriptVideoFullScreen = "var videocontentreels=document.querySelector(\"div[class='xjbqb8w x1lq5wgf xgqcy7u x30kzoy x9jhf4c x78zum5 x1q0g3np xod5an3 x14vqqas x6ikm8r x10wlt62 x1n2onr6 x1k90msu x6o7n8i x9lcvmn x1m6m0jg']\"),ControlVideoReelsBtn=document.querySelectorAll(\"div[class='x1ypdohk x5yr21d x1n2onr6 x3hqpx7 xg01cxk x47corl'],div[class='x1ypdohk x5yr21d x1n2onr6 x3hqpx7'], div[class='x5yr21d x1n2onr6 x3hqpx7 xg01cxk x47corl'], div[class='x5yr21d x1n2onr6 x3hqpx7']\"),previousVideoReelsBtn=ControlVideoReelsBtn[0].children[0]??document.querySelector(\"div[aria-label='Previous Card']\"),nextVideoReelsBtn=ControlVideoReelsBtn[1].children[0]??document.querySelector(\"div[aria-label='Next Card']\");null!=previousVideoReelsBtn&&(previousVideoReelsBtn.style.display=\"none\"),null!=nextVideoReelsBtn&&(nextVideoReelsBtn.style.display=\"none\");var masterheader=document.body.children[0];masterheader.innerHTML=\"\",masterheader.appendChild(videocontentreels),null!=nextVideoReelsBtn&&masterheader.appendChild(nextVideoReelsBtn),null!=previousVideoReelsBtn&&masterheader.appendChild(previousVideoReelsBtn),masterheader.style.margin=0,masterheader.style.height=\"100%\",document.documentElement.style.margin=0,document.documentElement.style.height=\"100%\",document.body.style.margin=0,document.body.style.height=\"100%\",videocontentreels.style.margin=0,videocontentreels.style.height=\"100%\";var videocontrolsreels=document.querySelector(\"div[class='x1i10hfl x1qjc9v5 xjbqb8w xjqpnuy xa49m3k xqeqjp1 x2hbi6w x13fuv20 xu3j5b3 x1q0q8m5 x26u7qi x972fbf xcfux6l x1qhh985 xm0m39n x9f619 x1ypdohk xdl72j9 x2lah0s xe8uvvx xdj266r x11i5rnm xat24cr x1mh8g0r x2lwn1j xeuugli xexx8yu x4uap5 x18d9i69 xkhd6sd x1n2onr6 x16tdsg8 x1hl2dhg xggy1nq x1ja2u2z x1t137rt x1o1ewxj x3x9cwd x1e5q0jg x13rtm0m x3nfvp2 x1q0g3np x87ps6o x1lku1pv x1a2a7pz']\");null!=videocontrolsreels?.className&&(videocontrolsreels.className=\"\");var videocontrolsreelstwo=document.querySelector(\"div[class='x78zum5 x5yr21d xl56j7k x1n2onr6 xh8yej3']\");null!=videocontrolsreelstwo?.className&&(videocontrolsreelstwo.className=\"\");var mastervideointervalId=window.setInterval((function(){var e=document.querySelector(\"div[class='x6s0dn4 x18l40ae x5yr21d x1n2onr6 xh8yej3']\");null==e?.className||(e.className=\"\"),console.log(\"mastervideointervalId\")}),1e3);";
        //video.ScriptVideoFullScreen += " window.addEventListener(\"wheel\",(e=>{Math.sign(e.deltaY)<1?nextVideoReelsBtn.click():previousVideoReelsBtn.click(),clearInterval(t);var t=window.setInterval((function(){var e=document.querySelector(\"div[data-video-id]\");if(null!=e){var i=e?.getAttribute(\"data-video-id\");window.chrome.webview.postMessage(\"https://www.facebook.com/reel/\"+i),clearInterval(t)}}),1e3)}));";

        return await base.OnReplace(media);
    }
}