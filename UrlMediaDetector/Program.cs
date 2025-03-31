using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Services;

namespace UrlMediaDetector;

public class Program
{
    // Thread-safe, lazily initialized singleton instance of WindowService.
    private static readonly Lazy<Program> _instance = new(() => new Program());

    // Public property to access the singleton instance.
    public static Program Instance => _instance.Value;

    private readonly List<IServiceFinder> _availableServices;

    public Program()
    {
        // Register services here
        this._availableServices = new List<IServiceFinder>()
        {
            new YoutubeMusic(), // tested 2025-02-25
            new YoutubeChannel(), // tested 2025-02-23
            new YoutubeShorts(), // tested 2025-02-25
            new Youku(), // tested 2025-02-25
            new Vimeo(), // tested 2025-02-25
            new Twitch(), // tested 2025-02-25
            new Spotify(), // tested 2025-02-25
            new Soundcloud(), // tested 2025-02-25
            new Rumble(),
            new Reddit(), // tested 2025-02-26
            new PrimeVideo(),
            new OKRU(), // tested 2025-02-26
            new Netflix(),
            new Max(),
            new JioCinema(),
            new Hulu(),
            new Frame(), // tested 2025-02-24
            new FacebookReels(), // tested 2025-02-26
            new DisneyPlus(),
            new Dailymotion(), // tested 2025-02-23
            new Crunchyroll(), // tested 2025-02-25
            new Bilibili(), // tested 2025-02-26
            new BilibiliTv(), // tested 2025-02-26

            // Add more service here
            new Radio(),
            new Video(),
            new Audio(),
            new Image(),
        };
    }

    public async Task<IUrlMedia?> MatchServiceAsync(IUrlMedia serviceDatum)
    {
        foreach (var service in _availableServices)
        {
            var (matched, result) = await service.Match(serviceDatum);
            
            // Return the matched result immediately
            if (matched) return result; 
        }

        // Return original serviceDatum if no match found
        return serviceDatum; 
    }

    public void OnReplace(string serviceName)
    {

    }
}