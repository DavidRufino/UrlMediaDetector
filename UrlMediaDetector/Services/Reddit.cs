using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Reddit : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Reddit
        new(@"^(?!.*\bold\b)https?:\/\/.*(preview\.)(redd\.it).*", ServiceNameEnum.Reddit, MediaEnum.WebRecorded),
        new(@"^(?!.*\bold\b)https?:\/\/.*(redd)\.it\/.*", ServiceNameEnum.Reddit, MediaEnum.WebRecorded),
        new(@"^(?!.*\bold\b)https?:\/\/.*(reddit.com)\/[a-zA-Z]{1}\/(?:[^\\/:*?'<>|\r\n]+\/)*(comments)\/.*", ServiceNameEnum.Reddit, MediaEnum.WebRecorded),
    ];

    public Reddit()
    {
        this.ServiceName = ServiceNameEnum.Reddit;
        this.HomePage = "https://www.reddit.com/";
        this.UseUrlWithoutSchemeAndSubdomain = false;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        // Disabled
        /*
        if (media.MediaType == MediaEnum.WebRecorded || media.MediaType == MediaEnum.Video)
        {
            //  Regex math patter para encontrar esta url especifica
            // Replace "www." with "old." only at the start of the URL
            pageUrl = Regex.Replace(pageUrl, @"^(https?://)www\.", "$1old.");
            var videoURL = await HttpFetcherHelper.Instance.GetUrlFromWebPageContents(pageUrl, @"(?<=data-hls-url="")[^""]+(?="")");
            if (string.IsNullOrEmpty(videoURL)) return null;

            Debug.WriteLine("[Reddit] FilterURL ELSE videoURL " + videoURL);

            //  Verificar se é um video do Youtube ou do Reddit
            //  As vezes um comentario é um video do Youtube ou de um outro servico
            if (Regex.IsMatch(videoURL, @"^http(?:s?):\/\/(?:www\.)?youtu(?:be\.com\/watch\?v=|\.be\/)([\w\-]*)(&(amp;)[\w\=]*)?"))
            {
                media.ServiceName = ServiceNameEnum.Youtube;
                media.MediaType = MediaEnum.WebRecorded;
                media.FullServiceUrl = videoURL;

                YoutubeChannel youtubeChannel = new();
                media = await youtubeChannel.OnReplace(media);
            }
            else
            {
                media.ServiceName = ServiceNameEnum.Reddit;
                media.MediaType = MediaEnum.Video;
                media.SanitizedUrl = videoURL;
            }
        }
        else
        {
            // Check if the video URL is from Reddit
            if (pageUrl.Contains("https://v.redd.it/"))
            {
                pageUrl = string.Format("{0}/HLSPlaylist.m3u8", pageUrl);
                media.ServiceName = ServiceNameEnum.Reddit;
                media.MediaType = MediaEnum.Video;
                media.SanitizedUrl = pageUrl;
            }
        }
        */

        media.Javascript = @"const redditFullscreenElements = new Set();
function OnMediaScreen() {
    let element =
        document.getElementsByTagName('shreddit-aspect-ratio')[0] ||
        document.querySelector(""shreddit-async-loader[bundlename='gallery_carousel']"") ||
        document.querySelector(""div[slot='post-media-container']"");
    if (element && !redditFullscreenElements.has(element)) {
        element.requestFullscreen()
            .then(() => {
                redditFullscreenElements.add(element);
            })
            .catch((err) => console.error('Error entering fullscreen:', err));
    }
}
OnMediaScreen();";

        return await base.OnReplace(media);
    }
}
