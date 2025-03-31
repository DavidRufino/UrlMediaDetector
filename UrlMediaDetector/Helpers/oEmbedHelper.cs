using System.Diagnostics;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Models;
using UrlMediaDetector.Models.Contexts;

namespace UrlMediaDetector.Helpers;

public class oEmbedHelper
{
    // Singleton instance of HttpFetcherHelper with lazy initialization.
    private static readonly Lazy<oEmbedHelper> _instance = new(() => new oEmbedHelper());

    // Public property to access the singleton instance.
    public static oEmbedHelper Instance => _instance.Value;

    /// <summary>
    /// Method para fazer requisição no servico oEmbed baseado no servico desejado
    /// <para>Obtem o titulo do video</para>
    /// </summary>
    /// <param name="service"></param>
    /// <param name="sourceUrl"></param>
    /// <returns></returns>
    public async Task<string> GetTitle(ServiceNameEnum name, string title, string sourceUrl)
    {
        Debug.WriteLine("[oEmbedUtil] GetTitle " + sourceUrl);

        try
        {
            string urlEndpoint = null;

            switch (name)
            {
                case ServiceNameEnum.Youtube:
                    //  if contains /embed/ on path
                    if (sourceUrl.Contains("/embed/"))
                    {
                        // Remove all query parameter
                        sourceUrl = Regex.Replace(sourceUrl, "([?]).*", "");

                        // Extract the video ID from the embed URL
                        string videoId = GetLastPathSegment(sourceUrl);

                        // Construct the regular YouTube watch URL
                        sourceUrl = string.Format("https://www.youtube.com/watch?v={0}", videoId);
                    }
                    urlEndpoint = string.Format(YOUTUBE_OEMBED_ENDPOINT, sourceUrl);
                    break;

                case ServiceNameEnum.Vimeo:
                    urlEndpoint = string.Format(VIMEO_OEMBED_ENDPOINT, sourceUrl);
                    break;

                case ServiceNameEnum.Dailymotion:
                    urlEndpoint = string.Format(DAILYMOTION_OEMBED_ENDPOINT, sourceUrl);
                    break;

                case ServiceNameEnum.Reddit:
                    urlEndpoint = string.Format(REDDIT_OEMBED_ENDPOINT, sourceUrl);
                    break;

                case ServiceNameEnum.Spotify:
                    //  Se for spotify no formato embed, transforme-o no formato normal,
                    //  pois sera necessario para obter o seu TITULO atravez do oEmbed API
                    sourceUrl = sourceUrl.Replace("spotify.com/embed/", "spotify.com/");
                    //  Remover os Query Parameter caso tenha
                    sourceUrl = Regex.Replace(sourceUrl, @"([?]).*", "");
                    urlEndpoint = string.Format(SPOTIFY_OEMBED_ENDPOINT, sourceUrl);
                    break;

                case ServiceNameEnum.Facebook:
                    urlEndpoint = string.Format(FACEBOOK_OEMBED_ENDPOINT, sourceUrl);
                    break;

                case ServiceNameEnum.Soundcloud:
                    urlEndpoint = string.Format(SOUNDCLOUD_OEMBED_ENDPOINT, sourceUrl);
                    break;

                //  NAO POSSUI ENDPOINT
                default: return title ?? sourceUrl;
            }

            if (string.IsNullOrEmpty(urlEndpoint)) return title;

            // Retrieve the JsonTypeInfo for the oEmbedModel type from the source generation context
            // This type information is used to efficiently handle serialization and deserialization without reflection.
            JsonTypeInfo<oEmbedModel> typeInfo = JsonContext.Default.oEmbedModel;

            var response = await HttpFetcherHelper.Instance.GetJsonFromEndpoint<oEmbedModel>(urlEndpoint, typeInfo);
            if (response == null) return sourceUrl;

            return response.Title ?? title;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[oEmbedUtil] GetTitle Error {ex}");
            return title ?? sourceUrl;
        }
    }
    
    // Method to get the last path segment from a given URL
    private string GetLastPathSegment(string url)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult))
        {
            // Get the path segments and return the last one
            string[] segments = uriResult.Segments;
            if (segments.Length > 0)
            {
                // Trim any trailing slashes from the last segment
                return segments[^1].Trim('/');
            }
        }

        // If the URL is not valid or there are no segments, return an empty string
        return string.Empty;
    }

    /// <summary>
    /// Youtube oEmbed ENDPOINT, precisa ser o SOURCE URL
    /// <para>SAMPLE http://www.youtube.com/oembed?url=https://www.youtube.com/watch?v=xVMsAgHy_IY&format=json
    /// </para>
    /// </summary>
    private const string YOUTUBE_OEMBED_ENDPOINT = "https://www.youtube.com/oembed?url={0}&format=json";

    /// <summary>
    /// Vimeo oEmbed ENDPOINT, precisa ser o SOURCE URL
    /// <para>SAMPLE https://vimeo.com/api/oembed.json?url=https://vimeo.com/686896740
    /// </para>
    /// </summary>
    private const string VIMEO_OEMBED_ENDPOINT = "https://vimeo.com/api/oembed.json?url={0}";

    /// <summary>
    /// Facebook oEmbed ENDPOINT, precisa ser o SOURCE URL
    /// <para>SAMPLE ?
    /// </para>
    /// </summary>
    private const string FACEBOOK_OEMBED_ENDPOINT = "https://www.facebook.com/plugins/video/oembed.json/?url={0}";

    /// <summary>
    /// Dailymotion oEmbed ENDPOINT, precisa ser o SOURCE URL
    /// <para>SAMPLE https://www.dailymotion.com/services/oembed?format=json&url=https://www.dailymotion.com/video/x8aaeem
    /// </para>
    /// </summary>
    private const string DAILYMOTION_OEMBED_ENDPOINT = "https://www.dailymotion.com/services/oembed?format=json&url={0}";

    /// <summary>
    /// REDDIT oEmbed ENDPOINT, precisa ser o SOURCE URL
    /// <para>SAMPLE https://www.reddit.com/oembed?url=https://www.reddit.com/r/RocketLeague/comments/u9amhl/was_i_in_the_right/
    /// </para>
    /// </summary>
    private const string REDDIT_OEMBED_ENDPOINT = "https://www.reddit.com/oembed?url={0}";

    /// <summary>
    /// Rumble oEmbed ENDPOINT, precisa ser o SOURCE URL
    /// <para>SAMPLE https://rumble.com/api/Media/oembed.json?url=https://rumble.com/v129mxu-behavior-analyst-reacts-to-deleted-amber-heard-video.html
    /// </para>
    /// </summary>
    private const string RUMBLE_OEMBED_ENDPOINT = "https://rumble.com/api/Media/oembed.json?url=https://rumble.com/{0}";

    /// <summary>
    /// Twitter oEmbed ENDPOINT, precisa ser o SOURCE URL
    /// <para>SAMPLE https://publish.twitter.com/oembed?url=https://twitter.com/Reuters/status/1518659858966360064
    /// </para>
    /// </summary>
    private const string TWITTER_OEMBED_ENDPOINT = "https://publish.twitter.com/oembed?url={0}";

    /// <summary>
    /// Spotify oEmbed ENDPOINT, precisa ser o SOURCE URL
    /// <para>
    /// SAMPLE https://open.spotify.com/oembed/?url=https://open.spotify.com/track/2qToAcex0ruZfbEbAy9OhW
    /// https://open.spotify.com/oembed/?url=spotify%3Aartist%3A7ae4vgLLhir2MCjyhgbGOQ
    /// </para>
    /// </summary>
    private const string SPOTIFY_OEMBED_ENDPOINT = "https://open.spotify.com/oembed/?url={0}";

    /// <summary>
    /// SoundCLoud oEmbed ENDPOINT, precisa ser o SOURCE URL
    /// <para>
    /// SAMPLE https://soundcloud.com/oembed?url=https%3A%2F%2Fsoundcloud.com%2Fforss%2Fflickermood&format=json
    /// </para>
    /// </summary>
    private const string SOUNDCLOUD_OEMBED_ENDPOINT = "https://soundcloud.com/oembed?url={0}&format=json";
}
