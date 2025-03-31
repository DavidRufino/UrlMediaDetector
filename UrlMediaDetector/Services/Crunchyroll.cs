using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class Crunchyroll : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        //  Crunchyroll
        new(@"^(crunchyroll.*)\/(episode-[0-9]|watch\/).*", ServiceNameEnum.Crunchyroll, MediaEnum.WebRecorded),
    ];

    public Crunchyroll()
    {
        this.ServiceName = ServiceNameEnum.Crunchyroll;
        this.HomePage = "https://www.crunchyroll.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia? media)
    {
        var fullServiceUrl = media.FullServiceUrl;
        string pageUrl = fullServiceUrl.ToString();

        media.SanitizedUrl = pageUrl;

        //media.Javascript = "$('#vilos-player').parents().siblings().hide();$('#vilos-player').css({ position: 'fixed', left: '0', top: '0' });";
        media.Javascript += "var crunchyrollvdf = document.getElementsByClassName('video-player')[0]; if(crunchyrollvdf==null){document.getElementById('vilos-player').requestFullscreen();}else{crunchyrollvdf.requestFullscreen();}";

        return await base.OnReplace(media);
    }
}
