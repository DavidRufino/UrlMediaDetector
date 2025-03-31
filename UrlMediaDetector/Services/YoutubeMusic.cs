using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class YoutubeMusic : BaseChannel
{
    public YoutubeMusic()
    {
        this.ServiceName = ServiceNameEnum.YoutubeMusic;
        this.HomePage = "https://music.youtube.com/";
        this.UseUrlWithoutSchemeAndSubdomain = false;
    }

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Youtube Music
        new(@"music\.youtube\.com\/(?!iframe_api|channel|time_continue|feed|watch[?]time|[a-z]{1}\/)(playlist[?]|watch[?]v=)\/?[0-9a-zA-Z].*", ServiceNameEnum.YoutubeMusic, MediaEnum.WebRecorded),
    ];

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        //  Se for Music Youtube
        media.SanitizedUrl = pageUrl;

        //  Criar script javascript
        //  Remover a barra do site music.youtube
        media.Javascript = "var gridnavbar = document.getElementsByTagName('ytmusic-nav-bar')[0];gridnavbar.parentNode.removeChild(gridnavbar);var gridnavbackground = document.getElementById('nav-bar-background');gridnavbackground.parentNode.removeChild(gridnavbackground);var gridnavdivider = document.getElementById('nav-bar-divider');gridnavdivider.parentNode.removeChild(gridnavdivider);var gridcontrolpip = document.querySelector(\"div[class='top-row-buttons style-scope ytmusic-player']\");gridcontrolpip.parentNode.removeChild(gridcontrolpip);";

        return await base.OnReplace(media);
    }
}