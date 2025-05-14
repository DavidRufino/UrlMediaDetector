using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Document : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Document
        new(@"(http|https).*[\w]+\.(txt|rtf|js|mjs|ts|json|xml|html|md|c|cs|r)(?=[\?\&]?.*$)$", ServiceNameEnum.Document, MediaEnum.Document),
        new(@"^[a-zA-Z]{1}:(\\|\/)(?:[^\\/:*?' <>|\r\n]+\\)*[^\\/:*?'<>|\r\n].*\.(txt|rtf|js|mjs|ts|json|xml|html|md|c|cs|r)", ServiceNameEnum.Document, MediaEnum.Document),
    ];

    public Document()
    {
        this.ServiceName = ServiceNameEnum.Document;
        this.HomePage = null;
        this.UseUrlWithoutSchemeAndSubdomain = false;
    }

    public override async Task<IUrlMedia> OnReplace(IUrlMedia media)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        return await base.OnReplace(media);
    }
}