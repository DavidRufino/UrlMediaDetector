using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Hulu : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        //  Hulu
        new(@"^hulu\.(com|jp)\/(watch)(\/)?.*", ServiceNameEnum.Hulu, MediaEnum.WebRecorded),
        new(@"^hulu\.(com|jp)\/(series)(\/)?.*", ServiceNameEnum.Hulu, MediaEnum.WebRecorded),
        new(@"^hulu\.(com|jp)\/(app)\/(watch)(\/)?.*", ServiceNameEnum.Hulu, MediaEnum.WebRecorded),
    ];

    public Hulu()
    {
        this.ServiceName = ServiceNameEnum.Hulu;
        this.HomePage = "https://www.hulu.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }
}
