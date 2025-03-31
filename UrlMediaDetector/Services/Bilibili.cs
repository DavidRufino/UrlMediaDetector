using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Bilibili : BaseChannel
{
    private string BILIBILI_LIVE = "https://www.bilibili.com/blackboard/live/live-activity-player.html?cid={0}&quality=0";
    private string BILIBILI_EMBED = "https://player.bilibili.com/player.html?isOutside=true&bvid={0}";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Bilibili
        new(@"(m[.])?bilibili[.]com\/(video|bangumi\/play|cheese\/play)(?!anime|read|cm|v|\/digital|space|manga|detail|digital).*", ServiceNameEnum.Bilibili, MediaEnum.WebRecorded),
        new(@"(live[.])?bilibili[.]com\/([0-9]{2,})(?!video|bangumi\/play|cheese\/play|anime|read|cm|v|d|\/digital|space|manga|detail|digital\/)([a-z-A-Z0-9]).*", ServiceNameEnum.Bilibili, MediaEnum.WebLive),
    ];

    public Bilibili()
    {
        this.ServiceName = ServiceNameEnum.Bilibili;
        this.HomePage = "https://www.bilibili.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        if (media.MediaType == MediaEnum.WebRecorded)
        {
            // player.bilibili.com/player.html?aid=999717926&bvid=BV1v44y1c7PB&cid=1233654822&page=1
            // https://www.bilibili.com/video/BV1Bh4y1S78m?spm_id_from=333.1007.tianma.1-1-1.click

            // make the property pageUrl receive the default url
            pageUrl = pageUrl.Replace("https://", "");
            pageUrl = pageUrl.Replace("http://", "");
            pageUrl = pageUrl.Replace("https//", "");
            pageUrl = pageUrl.Replace("http//", "");
            pageUrl = pageUrl.Replace("www.", "");
            pageUrl = Regex.Replace(pageUrl, @"player\.bilibili.*bvid=", "");
            pageUrl = pageUrl.Replace("bilibili.com/video/", "").Trim();
            pageUrl = Regex.Replace(pageUrl, @"(\/)?[?].*", "");
            pageUrl = Regex.Replace(pageUrl, @"[&].*", "");
            pageUrl = Regex.Replace(pageUrl, @"/", "");

            //media.Javascript = "var video=document.getElementsByTagName('video')[0];video.play();";
            //media.Javascript += "var video=document.getElementsByTagName('video')[0];video.requestFullscreen();";

            media.SanitizedUrl = string.Format(BILIBILI_EMBED, pageUrl);
            media.MediaType = MediaEnum.WebRecorded;
        }
        else
        {
            //  Formato Web é para live.bilibili.com
            //  Format Web Lives
            //  Live

            pageUrl = pageUrl.Replace("https://", "").Trim();
            pageUrl = pageUrl.Replace("http://", "").Trim();
            pageUrl = pageUrl.Replace("live.bilibili.com/", "").Trim();
            pageUrl = Regex.Replace(pageUrl, @"[?].*", "");
            pageUrl = Regex.Replace(pageUrl, @"/", "");
            media.SanitizedUrl = string.Format(BILIBILI_LIVE, pageUrl);

            Console.WriteLine("live.bilibili: pageUrl " + pageUrl);

            //  Exibir apenas o chat do livi bilibili
            media.Javascript = "var gridChat=document.getElementById('aside-area-vm');gridChat.style.height='100%';gridChat.style.width='100%';document.body.innerHTML='';document.body.appendChild(gridChat);";

            //video.ScriptHTML = "var video=document.getElementById('bilibiliPlayer').getElementsByClassName('bilibili-player-video')[0].getElementsByTagName('video')[0];null==video&&(video=document.getElementById('bilibiliPlayer').getElementsByClassName('bilibili-player-video')[0].getElementsByTagName('bwp-video')[0]),video.muted=!1,video.volume=1,video.play(),video.requestFullscreen();";
            //video.ScriptHTML = "var video=document.getElementsByTagName('video')[0];video.play();";
            //video.ScriptVideoFullScreen = "var uvideoconfig=document.getElementsByTagName('video')[0];uvideoconfig.requestFullscreen();uvideoconfig.setAttribute('disablePictureInPicture', '');uvideoconfig.play();";
            //video.DisablePictureInPicture = "var video = document.getElementsByTagName('video')[0]; video.setAttribute('disablePictureInPicture', '');";
        }

        return await base.OnReplace(media);
    }
}
