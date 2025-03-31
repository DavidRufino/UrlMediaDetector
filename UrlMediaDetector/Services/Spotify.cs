using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Spotify : BaseChannel
{
    /// <summary>
    /// https://developer.spotify.com/documentation/web-api
    /// </summary>
    private string SPOTIFYAPI_VIDEO = "";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Spotify
        new(@"^(spotify\.com)\/(embed\/)?(artist|episode|playlist|show|album)\/[A-Za-z-0-9_-].*", ServiceNameEnum.Spotify, MediaEnum.WebRecorded),
    ];

    public Spotify()
    {
        this.ServiceName = ServiceNameEnum.Spotify;
        this.HomePage = "https://www.spotify.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        //  Exemplos
        //https://open.spotify.com/embed/playlist/37i9dQZF1DZ06evO0aBNIs?utm_source=generator&theme=0
        //https://open.spotify.com/episode/4S4cW21Z405I4uZgiIAc3A#login
        //https://open.spotify.com/show/6ES7rj1IzM55fn1s5mmnPj
        //https://open.spotify.com/playlist/37i9dQZEVXbNG2KDcFcKOF
        //https://open.spotify.com/album/2BmsXusvrTogEVvaW1j3nq

        //  Se foi vindo de um outro servico que esta reproduzindo o Spotify
        //  As vezes o url schema permanecera, sendo necessario remove-lo novamente
        var spotifyFinalUrl = pageUrl = Regex.Replace(pageUrl, @"https://open.spotify.com/embed/", "https://open.spotify.com/");
        //pageUrl_NOSCHEME = Regex.Replace(pageUrl_NOSCHEME, @".*(spotify\.com\/embed\/)", "");
        //pageUrl_NOSCHEME = Regex.Replace(pageUrl_NOSCHEME, @".*(spotify\.com\/)", "");
        //pageUrl_NOSCHEME = Regex.Replace(pageUrl_NOSCHEME, @"(#).*", "");
        //pageUrl_NOSCHEME = Regex.Replace(pageUrl_NOSCHEME, @"([?]).*", "");

        media.SanitizedUrl = string.Format(SPOTIFYAPI_VIDEO, WebUtility.UrlEncode(spotifyFinalUrl));

        return await base.OnReplace(media);
    }
}
