using System.Text.Json.Serialization;

namespace UrlMediaDetector.Models;

public partial class oEmbedModel
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("provider_name")]
    public string ProviderName { get; set; }

    [JsonPropertyName("provider_url")]
    public string ProviderUrl { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("author_name")]
    public string AuthorName { get; set; }

    [JsonPropertyName("author_url")]
    public string AuthorUrl { get; set; }

    [JsonPropertyName("account_type")]
    public string AccountType { get; set; }

    [JsonPropertyName("html")]
    public string Html { get; set; }

    [JsonPropertyName("width")]
    public string Width { get; set; }

    [JsonPropertyName("height")]
    public string Height { get; set; }

    [JsonPropertyName("duration")]
    public string Duration { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("thumbnail_url")]
    public string ThumbnailUrl { get; set; }

    [JsonPropertyName("thumbnail_width")]
    public string ThumbnailWidth { get; set; }

    [JsonPropertyName("thumbnail_height")]
    public string ThumbnailHeight { get; set; }

    [JsonPropertyName("thumbnail_url_with_play_button")]
    public string ThumbnailUrlWithPlayButton { get; set; }

    [JsonPropertyName("upload_date")]
    public DateTimeOffset UploadDate { get; set; }

    [JsonPropertyName("video_id")]
    public string VideoId { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; }
}