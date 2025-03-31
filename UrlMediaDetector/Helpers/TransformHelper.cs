using System.Diagnostics;
using UrlMediaDetector.Services;

namespace UrlMediaDetector.Helpers;

public class TransformHelper
{
    /// <summary>
    /// Creates and returns an instance of a media service class based on the provided service name.
    /// </summary>
    public object? CreateMediaServiceInstance(string serviceName)
    {
        Debug.WriteLine($"[TransformHelper] FindTypes {serviceName}");

        return serviceName switch
        {
            "Youtube" => new YoutubeChannel(),
            "YoutubeMusic" => new YoutubeMusic(),
            "YoutubeShorts" => new YoutubeShorts(),
            "Youku" => new Youku(),
            "Vimeo" => new Vimeo(),
            "Twitch" => new Twitch(),
            "Spotify" => new Spotify(),
            "Soundcloud" => new Soundcloud(),
            "Rumble" => new Rumble(),
            "Reddit" => new Reddit(),
            "PrimeVideo" => new PrimeVideo(),
            "OKRU" => new OKRU(),
            "Netflix" => new Netflix(),
            "Max" => new Max(),
            "JioCinema" => new JioCinema(),
            "Hulu" => new Hulu(),
            "Frame" => new Frame(),
            "FacebookReels" => new FacebookReels(),
            "DisneyPlus" => new DisneyPlus(),
            "Dailymotion" => new Dailymotion(),
            "Crunchyroll" => new Crunchyroll(),
            "Bilibili" => new Bilibili(),
            "BilibiliTv" => new BilibiliTv(),
            "Radio" => new Radio(),
            "Video" => new Video(),
            "Audio" => new Audio(),
            "Image" => new Image(),
            _ => null // Default case if no match
        };
    }
}