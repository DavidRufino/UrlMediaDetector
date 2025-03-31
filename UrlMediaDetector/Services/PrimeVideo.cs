using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class PrimeVideo : BaseChannel
{
    private const string PRIME_VIDEO = "https://www.primevideo.com/detail/{0}?autoplay=1&t=0";

    public PrimeVideo()
    {
        this.ServiceName = ServiceNameEnum.PrimeVideo;
        this.HomePage = "https://www.primevideo.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // Prime Video
        new(@"primevideo\.com(\/region\/.*\/detail|\/detail)", ServiceNameEnum.PrimeVideo, MediaEnum.WebRecorded),
        new(@"(amazon\.).*\/.*\/(video)\/(detail)", ServiceNameEnum.PrimeVideo, MediaEnum.WebRecorded),
        new(@"^(primevideo\.com).*(detail)", ServiceNameEnum.PrimeVideo, MediaEnum.WebRecorded),
    ];

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {
        var urlWithoutSchemeAndSubdomain = media.UrlWithoutSchemeAndSubdomain;
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        if (Regex.IsMatch(urlWithoutSchemeAndSubdomain, @"^primevideo\.com/detail"))
        {
            urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("primevideo.com/detail/", "").Trim();
            urlWithoutSchemeAndSubdomain = urlWithoutSchemeAndSubdomain.Replace("?autoplay=1&t=0", "").Trim();
            urlWithoutSchemeAndSubdomain = Regex.Replace(urlWithoutSchemeAndSubdomain, "([/]ref+[A-Za-z0-9._%+-=&?]*)", "");

            media.SanitizedUrl = string.Format(PRIME_VIDEO, urlWithoutSchemeAndSubdomain);
        }
        else
        {
            //  Se for uma pagina www.amazon.de
            //  @"(amazon[.]de)\/.*\/(video)\/(detail)"
            //  https://www.amazon.de/gp/video/detail/B09QH9ZW64/ref=atv_hm_hom_1_c_ixO5Hr_brws_3_1/?language=en
            //  https://www.amazon.de/-/en/gp/video/detail/B0B8TFM1ST/ref=atv_hm_hom_1_c_YMtUbY_HSae789f_1_1
            //  https://www.amazon.de/gp/video/detail/B0B6STPSVJ/ref=atv_dp_atf_prime_sd_tv_resume_t1ADAAAAAA0wr0?language=en&autoplay=1&t=0

            //  Remover todos os conteudos que vieram da Extension apartir do parameter uviewsource
            pageUrl = Regex.Replace(pageUrl, @"[?].*", "");

            media.SanitizedUrl = string.Format("{0}{1}", pageUrl, "?autoplay=1");
        }

        return await base.OnReplace(media);
    }
}
