using System.Web;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UrlMediaDetector.Interfaces;

namespace UrlMediaDetector.Services;

public class YoutubeChannel : BaseChannel
{
    /// <summary>
    /// https://developers.google.com/youtube/iframe_api_reference
    /// </summary>
    private string YOUTUBEAPI_VIDEO = "";
    private string YOUTUBE_CHAT = "https://www.youtube.com/live_chat?is_popout=1&v={0}";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        //  Youtube - Web
        new(@"^(youtube\.com|youtu\.be)\/(?!iframe_api|channel|time_continue|feed|watch[?]time|[a-z]{1}\/)(watch[?]v=|watch[?]app=desktop&v=|watch[?]list=|embed).*", ServiceNameEnum.Youtube, MediaEnum.WebRecorded),
        new(@"^(youtube\.com|youtu\.be)\/(live)\/[-a-zA-Z0-9_]+", ServiceNameEnum.Youtube, MediaEnum.WebRecorded),
        new(@"^(youtube\.com|youtu\.be)\/(playlist)(\?list=)?", ServiceNameEnum.Youtube, MediaEnum.WebRecorded),
        new(@"^youtu\.be\/([-a-zA-Z0-9_]{11,})((\?|\&)t=(\d+)s?)?", ServiceNameEnum.Youtube, MediaEnum.WebRecorded),
    ];

    public YoutubeChannel()
    {
        this.ServiceName = ServiceNameEnum.Youtube;
        this.HomePage = "https://www.youtube.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();
        Uri uri = new Uri(fullServiceUrl);
        string timePosition = "0";

        // Extract query parameters from a URL string.
        var queryParameters = HttpUtility.ParseQueryString(uri.Query);
        if (queryParameters is not null)
        {
            timePosition = queryParameters["t"] ?? "0";
        }

        //  Se foi vindo de um outro servico que esta reproduzindo o Youtube
        //  As vezes o url schema permanecera, sendo necessario remove-lo novamente
        pageUrl = pageUrl.Replace("https://", "");
        pageUrl = pageUrl.Replace("http://", "");
        pageUrl = pageUrl.Replace("https//", "");
        pageUrl = pageUrl.Replace("http//", "");
        pageUrl = pageUrl.Replace("www.", "");
        pageUrl = pageUrl.Replace("youtube.com/watch?feature=player_profilepage&v=", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/watch?feature=player_detailpage&v=", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/watch?feature=player_embedded&v=", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/embed/", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/shorts/", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/watch?v=", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/watch?list=", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/watch?", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/live/", "").Trim();
        pageUrl = pageUrl.Replace("youtu.be/live/", "").Trim();
        pageUrl = pageUrl.Replace("youtu.be/", "").Trim();
        pageUrl = pageUrl.Replace("youtube.com/", "").Trim();
        pageUrl = pageUrl.Replace("&feature=youtu.be", "").Trim();
        pageUrl = pageUrl.Replace("app=desktop&v=", "").Trim();
        pageUrl = pageUrl.Replace("youtube.googleapis.com/v/", "").Trim();
        pageUrl = Regex.Replace(pageUrl, "(time_continue=.*(=))", "");

        // Remove query parameter
        pageUrl = Regex.Replace(pageUrl, "(&(?!list).*)", "");
        pageUrl = Regex.Replace(pageUrl, ".*([A-Za-z0-9._%+-=&?](list=))", ""); // remover all back the parameter list=
        pageUrl = Regex.Replace(pageUrl, @"(\?|\&){1}(t=).*", ""); // remove the paramter: t=
        pageUrl = Regex.Replace(pageUrl, @"[?].*", "");
        pageUrl = Regex.Replace(pageUrl, @"(\.com\/)$", ""); // remvoe the last .com/ if have

        Debug.WriteLine($"Youtube final url: {pageUrl}");

        media.SanitizedUrl = string.Format(YOUTUBEAPI_VIDEO, pageUrl, timePosition);

        //  Se o ID for menor que 12 characteres
        if (pageUrl.Length < 12)
        {
            //  Adicione o CHAT, pois TALVEZ seja uma live
            media.LiveChatUrl = string.Format(YOUTUBE_CHAT, pageUrl);
        }

        return await base.OnReplace(media);
    }
}