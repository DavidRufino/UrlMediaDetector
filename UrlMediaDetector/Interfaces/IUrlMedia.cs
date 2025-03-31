using UrlMediaDetector.Enums;

namespace UrlMediaDetector.Interfaces;

public interface IUrlMedia
{
    string Title { get; set; }
    string VideoId { get; set; }
    string? Favicon { get; set; }

    ServiceNameEnum? ServiceName { get; set; }
    MediaEnum MediaType { get; set; }

    /// <summary>
    /// The URL with unnecessary elements removed (e.g., tracking parameters).
    /// </summary>
    string SanitizedUrl { get; set; }

    /// <summary>
    /// excluding domain and subdomain.
    /// </summary>
    string UrlWithoutSchemeAndSubdomain { get; set; }

    /// <summary>
    /// The original, unfiltered service URL, including scheme, domain, and full path.
    /// </summary>
    string FullServiceUrl { get; set; }

    /// <summary>
    /// The referrer URL from which the service was accessed.
    /// </summary>
    string? ReferrerUrl { get; set; }

    string LiveChatUrl { get; set; }
    string HomePage { get; set; }

    string[] VideoResolutions { get; set; }
    string? UserAgent { get; set; }

    bool ApplyFullScreen { get; set; }
    string Javascript { get; set; }
}
