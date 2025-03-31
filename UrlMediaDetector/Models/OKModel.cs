using System.Text.Json.Serialization;

namespace UrlMediaDetector.Models
{
    public partial class OKModel
    {
        [JsonPropertyName("provider")]
        public string Provider { get; set; }

        [JsonPropertyName("service")]
        public string Service { get; set; }

        [JsonPropertyName("owner")]
        public bool? Owner { get; set; }

        [JsonPropertyName("voted")]
        public bool? Voted { get; set; }

        [JsonPropertyName("likeCount")]
        public string LikeCount { get; set; }

        [JsonPropertyName("compilation")]
        public string Compilation { get; set; }

        [JsonPropertyName("compilationIcon")]
        public Uri? CompilationIcon { get; set; }

        [JsonPropertyName("compilationTitle")]
        public string CompilationTitle { get; set; }

        [JsonPropertyName("subscribed")]
        public bool? Subscribed { get; set; }

        [JsonPropertyName("isWatchLater")]
        public bool? IsWatchLater { get; set; }

        [JsonPropertyName("slot")]
        public string Slot { get; set; }

        [JsonPropertyName("siteZone")]
        public string SiteZone { get; set; }

        [JsonPropertyName("showAd")]
        public bool? ShowAd { get; set; }

        [JsonPropertyName("fromTime")]
        public string FromTime { get; set; }

        [JsonPropertyName("author")]
        public OKAuthorDatum Author { get; set; }

        [JsonPropertyName("movie")]
        public OKMovieDatum Movie { get; set; }

        [JsonPropertyName("partnerId")]
        public long PartnerId { get; set; }

        [JsonPropertyName("ownerMovieId")]
        public string OwnerMovieId { get; set; }

        [JsonPropertyName("alwaysShowRec")]
        public bool? AlwaysShowRec { get; set; }

        [JsonPropertyName("videos")]
        public OKVideoDatum[] Videos { get; set; }

        [JsonPropertyName("hlsManifestUrl")]
        public string HlsManifestUrl { get; set; }

        [JsonPropertyName("hlsMasterPlaylistUrl")]
        public Uri? HlsMasterPlaylistUrl { get; set; }

        [JsonPropertyName("hlsPlaybackMasterPlaylistUrl")]
        public Uri? HlsPlaybackMasterPlaylistUrl { get; set; }

        [JsonPropertyName("liveDashManifestUrl")]
        public Uri? LiveDashManifestUrl { get; set; }

        [JsonPropertyName("livePlaybackDashManifestUrl")]
        public Uri? LivePlaybackDashManifestUrl { get; set; }

        [JsonPropertyName("liveCmafUrl")]
        public Uri? LiveCmafUrl { get; set; }

        [JsonPropertyName("failoverHosts")]
        public string[] FailoverHosts { get; set; }

        [JsonPropertyName("autoplay")]
        public OKAutoplayDatum Autoplay { get; set; }

        [JsonPropertyName("liveStreamInfo")]
        public OKLiveStreamInfoDatum LiveStreamInfo { get; set; }

        [JsonPropertyName("liveChat")]
        public OKLiveChatDatum LiveChat { get; set; }

        [JsonPropertyName("p2pInfo")]
        public OKP2PInfoDatum P2PInfo { get; set; }

        [JsonPropertyName("stunServers")]
        public OKStunServerDatum[] StunServers { get; set; }
    }

    public partial class OKAuthorDatum
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("profile")]
        public string Profile { get; set; }
    }

    public partial class OKAutoplayDatum
    {
        [JsonPropertyName("autoplayEnabled")]
        public bool? AutoplayEnabled { get; set; }

        [JsonPropertyName("timeFromEnabled")]
        public bool? TimeFromEnabled { get; set; }

        [JsonPropertyName("noRec")]
        public bool? NoRec { get; set; }

        [JsonPropertyName("fullScreenExit")]
        public bool? FullScreenExit { get; set; }

        [JsonPropertyName("vitrinaSection")]
        public string VitrinaSection { get; set; }
    }

    public partial class OKLiveChatDatum
    {
        [JsonPropertyName("chatUrl")]
        public string ChatUrl { get; set; }

        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("timeout")]
        public string Timeout { get; set; }

        [JsonPropertyName("showChatOverVideo")]
        public bool? ShowChatOverVideo { get; set; }
    }

    public partial class OKLiveStreamInfoDatum
    {
        [JsonPropertyName("startsInSec")]
        public string StartsInSec { get; set; }

        [JsonPropertyName("endsInSec")]
        public string EndsInSec { get; set; }

        [JsonPropertyName("startTime")]
        public string StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public string EndTime { get; set; }

        [JsonPropertyName("playbackDuration")]
        public string PlaybackDuration { get; set; }

        [JsonPropertyName("maxPlaybackDuration")]
        public string MaxPlaybackDuration { get; set; }
    }

    public partial class OKMovieDatum
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("movieId")]
        public string MovieId { get; set; }

        [JsonPropertyName("groupId")]
        public string GroupId { get; set; }

        [JsonPropertyName("likeId")]
        public string LikeId { get; set; }

        [JsonPropertyName("contentId")]
        public string ContentId { get; set; }

        [JsonPropertyName("poster")]
        public Uri? Poster { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("url")]
        public Uri? Url { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("statusText")]
        public string StatusText { get; set; }

        [JsonPropertyName("isLive")]
        public bool? IsLive { get; set; }

        [JsonPropertyName("notPublished")]
        public bool? NotPublished { get; set; }
    }

    public partial class OKP2PInfoDatum
    {
        [JsonPropertyName("isPeerEnabled")]
        public string IsPeerEnabled { get; set; }

        [JsonPropertyName("ubsc")]
        public string Ubsc { get; set; }

        [JsonPropertyName("pbsc")]
        public string Pbsc { get; set; }

        [JsonPropertyName("mptpc")]
        public string Mptpc { get; set; }

        [JsonPropertyName("pctmt")]
        public string Pctmt { get; set; }

        [JsonPropertyName("pbesc")]
        public string Pbesc { get; set; }

        [JsonPropertyName("prrt")]
        public string Prrt { get; set; }

        [JsonPropertyName("srt")]
        public string Srt { get; set; }

        [JsonPropertyName("swrt")]
        public string Swrt { get; set; }

        [JsonPropertyName("dctt")]
        public string Dctt { get; set; }
    }

    public partial class OKStunServerDatum
    {
        [JsonPropertyName("urls")]
        public string[] Urls { get; set; }
    }

    public partial class OKVideoDatum
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("seekSchema")]
        public long SeekSchema { get; set; }

        [JsonPropertyName("disallowed")]
        public bool Disallowed { get; set; }
    }
}
