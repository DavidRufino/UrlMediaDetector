using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Max : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        //  Max
        new(@"^(max)[.]com(\/(movie|show|mini-series|video\/watch|video\/watch-sport))\/([0-9a-z]{8,36})", ServiceNameEnum.Max, MediaEnum.WebRecorded),
    ];

    public Max()
    {
        this.ServiceName = ServiceNameEnum.Max;
        this.HomePage = "https://www.max.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }
}
