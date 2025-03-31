using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Netflix : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        //  Netflix
        new(@"^netflix\.com\/watch\/", ServiceNameEnum.Netflix, MediaEnum.WebRecorded)
    ];

    public Netflix()
    {
        this.ServiceName = ServiceNameEnum.Netflix;
        this.HomePage = "https://www.netflix.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }
}
