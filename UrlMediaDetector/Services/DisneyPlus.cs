using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class DisneyPlus : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        //  DisneyPlus
        new(@"^disneyplus\.com\/.*(browse|series|movies|play|video)\/[A-Za-z-0-9_-].*", ServiceNameEnum.DisneyPlus, MediaEnum.WebRecorded),
    ];

    public DisneyPlus()
    {
        this.ServiceName = ServiceNameEnum.DisneyPlus;
        this.HomePage = "https://www.disneyplus.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }
}
