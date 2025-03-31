using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Twitch : BaseChannel
{
    /// <summary>
    /// https://dev.twitch.tv/docs/embed/
    /// </summary>
    private const string PARENT = "";
    private const string TWITCH_STREAM = "https://player.twitch.tv/?channel={0}&autoplay=true&parent={1}";
    private const string TWITCH_VODv = "https://player.twitch.tv/?video={0}&autoplay=true&parent={1}";
    private const string TWITCH_CHAT = "https://www.twitch.tv/embed/{0}/chat?darkpopout&parent={1}";
    //private const string TWITCH_CHAT = "https://www.twitch.tv/popout/{0}/chat?parent={1}";
    //private const string TWITCH_CHAT = "https://www.twitch.tv/embed/{0}/chat?parent={1}";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        //  Twitch - Live
        new(@"player\.twitch\.tv\/\?channel=", ServiceNameEnum.Twitch, MediaEnum.WebLive),
        new(@"^twitch\.tv\/((?!login)(?!download)(?!directory)(?!videos)(?!jobs)\w+[A-Za-z-0-9_])((?!browse||about).)*", ServiceNameEnum.Twitch, MediaEnum.WebLive),

        // Twich Recorded
        new(@"^twitch\.tv\/(videos)\/(v\?[0-9]|[0-9])+", ServiceNameEnum.Twitch, MediaEnum.WebRecorded),
        new(@"player\.twitch\.tv\/\?autoplay=false&video=v", ServiceNameEnum.Twitch, MediaEnum.WebRecorded),
        new(@"player\.twitch\.tv\/\?video=", ServiceNameEnum.Twitch, MediaEnum.WebRecorded)
    ];

    public Twitch()
    {
        this.ServiceName = ServiceNameEnum.Twitch;
        this.HomePage = "https://www.twitch.tv/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        if (media.MediaType == MediaEnum.WebLive)
        {
            pageUrl = pageUrl.Replace("https://", "");
            pageUrl = pageUrl.Replace("http://", "");
            pageUrl = pageUrl.Replace("https//", "");
            pageUrl = pageUrl.Replace("http//", "");
            pageUrl = pageUrl.Replace("www.", "");
            pageUrl = pageUrl.Replace("player.twitch.tv/?channel=", "").Trim();
            pageUrl = pageUrl.Replace("go.twitch.tv/widgets/live_embed_player.swf?channel=", "").Trim();
            pageUrl = pageUrl.Replace("go.twitch.tv/", "").Trim();
            pageUrl = pageUrl.Replace("player.twitch.tv/?video= ", "").Trim();
            pageUrl = pageUrl.Replace("twitch.tv/", "").Trim();
            pageUrl = Regex.Replace(pageUrl, @".*\?channel=", "");
            pageUrl = Regex.Replace(pageUrl, @"[&].*", "");

            //pageUrl = Regex.Replace(pageUrl, @"\w([A-Za-z0-9._%+-]+[/])", "");
            //pageUrl = pageUrl.Replace("&autoplay=false&parent=idruf.com", "").Trim();
            pageUrl = Regex.Replace(pageUrl, @"\?.*", "");

            media.VideoId = pageUrl;
            media.SanitizedUrl = string.Format(TWITCH_STREAM, pageUrl, PARENT);
            media.LiveChatUrl = string.Format(TWITCH_CHAT, pageUrl, PARENT);
        }
        else
        {
            //https://www.twitch.tv/videos/1912798372
            //https://player.twitch.tv/?video=1912798372&parent=www.example.com
            pageUrl = pageUrl.Replace("player.twitch.tv/?autoplay=false&video=v", "").Trim();
            pageUrl = pageUrl.Replace("player.twitch.tv/?autoplay=false&video=", "").Trim();
            pageUrl = pageUrl.Replace("go.twitch.tv/widgets/live_embed_player.swf?channel=", "").Trim();
            pageUrl = pageUrl.Replace("go.twitch.tv/", "").Trim();
            //pageUrl = pageUrl.Replace("player.twitch.tv/?video= ", "").Trim();
            pageUrl = Regex.Replace(pageUrl, @".*\/videos\/", "");
            pageUrl = Regex.Replace(pageUrl, @".*\?video=", "");

            pageUrl = Regex.Replace(pageUrl, @"[&].*", "");
            pageUrl = Regex.Replace(pageUrl, @"\?.*", "");
            //pageUrl = pageUrl.Replace("&autoplay=true&parent=idruf.com", "").Trim();

            media.SanitizedUrl = string.Format(TWITCH_VODv, pageUrl, PARENT);
        }

        return await base.OnReplace(media);
    }
}