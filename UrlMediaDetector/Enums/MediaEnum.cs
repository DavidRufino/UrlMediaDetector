namespace UrlMediaDetector.Enums;

public enum MediaEnum
{
    Unknown,
    WebLive,        // Live stream from the web (Twitch Live, YouTube Live)
    WebRecorded,    // Recorded video on the web (Twitch VOD, YouTube video)
    Video,          // Video file played locally (e.g., in MP4)
    Audio,          // Audio file played locally (MP3)
    Image           // Static image file (JPG, PNG, WEBP)
}
