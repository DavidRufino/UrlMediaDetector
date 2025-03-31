using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Helpers;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class OKRU : BaseChannel
{
    private const string OK_RU_VIDEOS = "https://ok.ru/videoembed/{0}?nochat=1";

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // OKRU
        new(@"^ok\.ru\/(videoembed|video|live)\/([0-9])+", ServiceNameEnum.OKRU, MediaEnum.WebRecorded),
    ];

    public OKRU()
    {
        this.ServiceName = ServiceNameEnum.OKRU;
        this.HomePage = "https://www.ok.ru/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @"^ok\.ru\/(videoembed|video|live)\/", "");
        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @"[?].*", "");
        media.SanitizedUrl = string.Format(OK_RU_VIDEOS, urlWithoutSchemeAndSubdomain);

        return await base.OnReplace(media);
    }

    /*
    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media, IPlayerPreferenceDialog dialog)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        var result = await dialog.Show();

        if (result)
        {
            //  Criando a url embed para poder obter o video
            urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @"ok\.ru\/(videoembed|video|live)\/", "");
            urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @"[?].*", "");
            var finalUrl = string.Format(OK_RU_VIDEOS, urlWithoutSchemeAndSubdomain);

            //  Obter o video.mp3 do elemento video na propriedade data-options
            //var script = await RequestPage(pageUrl, @"data-options=(\[{.*}\])");
            var scriptResult = await HttpFetcherHelper.Instance.GetUrlFromWebPageContents(finalUrl, @"data-options=(\""{).*(}\"")");
            if (string.IsNullOrEmpty(scriptResult)) return null;

            //  Decodificar o HTML se estiver codificado
            scriptResult = HttpFetcherHelper.Instance.CheckThenDecodeHTML(scriptResult);

            //  Remover os \\u0026 precisa fazer 2 vezes
            var scriptUnescape = System.Text.RegularExpressions.Regex.Unescape(scriptResult);
            scriptUnescape = System.Text.RegularExpressions.Regex.Unescape(scriptUnescape);

            Debug.WriteLine("[IdentifyHelper] FilterURL Одноклассники scriptUnescape " + scriptUnescape);

            string dataValuePattern = @"\""metadata\"":\""({.*?})\""";
            Match matchDataOptionPattern = Regex.Match(scriptUnescape, dataValuePattern);

            if (matchDataOptionPattern.Success && matchDataOptionPattern.Groups.Count > 1)
            {
                string okruMetadataValue = matchDataOptionPattern.Groups[1].Value;
                Debug.WriteLine($"OKRU matchDataOptionPattern {okruMetadataValue}");

                var okruJson = JsonSerializer.Deserialize<OKModel>(okruMetadataValue);
                if (okruJson is null) return null;

                // identify if okru datum have videos resolutions
                if (okruJson.Videos is not null && okruJson.Videos.Length > 0)
                {
                    Debug.WriteLine($"OKRU have videos resolutions");

                    // Create a list to hold video URLs
                    // dont know why, when user change the resolution, the video dont play more
                    List<string> videoUrls = new List<string>();
                    var videosLength = okruJson.Videos.Length;

                    foreach (var videoResolutions in okruJson.Videos)
                    {
                        if (string.IsNullOrEmpty(videoResolutions.Url)) continue;
                        if (string.IsNullOrEmpty(videoResolutions.Name)) continue;

                        // if video have higher than two video resolution, so block this resolutions
                        if (videosLength > 2)
                        {
                            if (videoResolutions.Name.Contains("mobile")) continue;
                            if (videoResolutions.Name.Contains("lowest")) continue;
                        }

                        Debug.WriteLine($"OKRU have videos resolutions {videoResolutions.Name}");
                        videoUrls.Add(videoResolutions.Url);
                    }

                    // Set the VIDEO_URL property of the IdentifyData object to the array of video URLs
                    media.VideoResolutions = videoUrls.ToArray();
                }

                // this property need if the this.IdentifyData.VIDEO_URL may null
                string urlVideo = okruJson?.HlsManifestUrl ?? okruJson?.HlsMasterPlaylistUrl?.ToString();

                Debug.WriteLine($"OKRU urlVideo {urlVideo}");

                media.SanitizedUrl = urlVideo;
                media.UserAgent = HttpFetcherHelper.Instance.UserAgent;
                media.MediaType = MediaEnum.Video;
                return media;
            }
        }

        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @"^ok\.ru\/(videoembed|video|live)\/", "");
        urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, @"[?].*", "");
        media.SanitizedUrl = string.Format(OK_RU_VIDEOS, urlWithoutSchemeAndSubdomain);

        return await base.OnReplace(media);
    }
    */
}