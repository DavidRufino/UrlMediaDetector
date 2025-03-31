using System.Text.Json.Serialization;

namespace UrlMediaDetector.Models;

public partial class RumbleModel
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("author_name")]
    public string AuthorName { get; set; }

    [JsonPropertyName("author_url")]
    public Uri AuthorUrl { get; set; }

    [JsonPropertyName("provider_name")]
    public string ProviderName { get; set; }

    [JsonPropertyName("provider_url")]
    public Uri ProviderUrl { get; set; }

    [JsonPropertyName("html")]
    public string Html { get; set; }

    [JsonPropertyName("width")]
    public long Width { get; set; }

    [JsonPropertyName("height")]
    public long Height { get; set; }

    [JsonPropertyName("duration")]
    public long Duration { get; set; }

    [JsonPropertyName("thumbnail_url")]
    public Uri ThumbnailUrl { get; set; }

    [JsonPropertyName("thumbnail_width")]
    public long ThumbnailWidth { get; set; }

    [JsonPropertyName("thumbnail_height")]
    public long ThumbnailHeight { get; set; }
}
