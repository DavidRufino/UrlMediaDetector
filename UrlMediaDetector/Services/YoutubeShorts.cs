using System.Diagnostics;
using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class YoutubeShorts : BaseChannel
{
    private const string YOUTUBESHORTS_VIDEO = "https://www.youtube.com/shorts/{0}";

    public YoutubeShorts()
    {
        this.ServiceName = ServiceNameEnum.YoutubeShorts;
        this.HomePage = "https://www.youtube.com/shorts/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Youtube Shorts
        new(@"^(youtube\.com|youtu\.be)\/(?!iframe_api|channel|time_continue|feed|watch[?]time|[a-z]{1}\/)(shorts)\/?.*", ServiceNameEnum.YoutubeShorts, MediaEnum.WebRecorded)
    ];

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        //  Filetype: WebShorts as Youtube Shorts
        //  SE NAO é um Youtuber Shorts Video
        //  Se foi vindo de um outro servico que esta reproduzindo o Youtube
        //  As vezes o url schema permanecera, sendo necessario remove-lo novamente
        pageUrl = pageUrl.Replace("https://", "");
        pageUrl = pageUrl.Replace("http://", "");
        pageUrl = pageUrl.Replace("https//", "");
        pageUrl = pageUrl.Replace("http//", "");
        pageUrl = pageUrl.Replace("www.", "");
        pageUrl = pageUrl.Replace("apiuviewvideos.blogspot.com/?", "").Trim();
        pageUrl = pageUrl.Replace("uview-api.netlify.app/api/youtube/?", "").Trim();
        pageUrl = pageUrl.Replace("idruf.com/apiv2/Youtube/?", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/watch?feature=player_profilepage&v=", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/watch?feature=player_detailpage&v=", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/watch?feature=player_embedded&v=", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/embed/", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/shorts/", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/shorts", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/watch?v=", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/watch?", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/live/", "").Trim();
        pageUrl = pageUrl.Replace("youtu.be/", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/", "").Trim();
        pageUrl = pageUrl.Replace("&feature=youtu.be", "").Trim();
        pageUrl = pageUrl.Replace("youtube.googleapis.com/v/", "").Trim();

        pageUrl = Regex.Replace(pageUrl, "(time_continue=.*(=))", ""); // https://www.youtube.com/watch?time_continue=5&v=4BnWnanXbq0
        pageUrl = Regex.Replace(pageUrl, "(&(?!list).*)", "");
        pageUrl = Regex.Replace(pageUrl, "([A-Za-z0-9._%+-=&?]+t=)", "");

        Debug.WriteLine("youtube shorts extracted: " + pageUrl);

        media.SanitizedUrl = string.Format(YOUTUBESHORTS_VIDEO, pageUrl);

        //  Remover elemento HTML na pagina shorts
        media.Javascript = "var masterheader=document.getElementById('masthead-container');masterheader?.parentNode.removeChild(masterheader);var tpytappdrawer=document.getElementsByTagName('ytd-mini-guide-renderer')[0];tpytappdrawer?.parentNode.removeChild(tpytappdrawer);var ytdminiguided=document.getElementsByTagName('tp-yt-app-drawer')[0];ytdminiguided?.parentNode.removeChild(ytdminiguided);var ytdpagemanager1=document.getElementsByTagName('ytd-shorts')[0];ytdpagemanager1.style.margin=0;ytdpagemanager1.style.height='100%';ytdpagemanager1.style.width='100%';ytdpagemanager1.style.overflow='hidden';var ytdpagemanager2=document.getElementById('shorts-container');ytdpagemanager2.style.setProperty('height','calc(100vh + 24px)');ytdpagemanager2.style.marginTop='-24px';var ytdpagemanager3=document.getElementsByTagName('ytd-page-manager')[0];ytdpagemanager3.className='';ytdpagemanager3.style.margin=0;ytdpagemanager3.style.padding=0;ytdpagemanager3.style.height='100%';ytdpagemanager3.style.width='100%';";
        media.Javascript += "document.body.style.overflow='hidden';";

        return await base.OnReplace(media);
    }
}