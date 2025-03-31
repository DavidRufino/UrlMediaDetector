using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;
using UrlMediaDetector.Services.Base;

namespace UrlMediaDetector.Services;

public class JioCinema : BaseChannel
{
    public override IEnumerable<ServicePatternModel> Patterns =>
    [
        // JioCinema
        new(@"^jiocinema\.com\/(watch|movies|sports)\/(tv|video[s]?|movie[s]?|football\/([0-9]{4}\/match\/live|video[s]?)).*[a-z0-9]{32}", ServiceNameEnum.JioCinema, MediaEnum.WebRecorded),
        new(@"^jiocinema\.com\/(watch|movies|sports)\/(cricket).*", ServiceNameEnum.JioCinema, MediaEnum.WebRecorded),
    ];

    public JioCinema()
    {
        this.ServiceName = ServiceNameEnum.JioCinema;
        this.HomePage = "https://www.jiocinema.com/";
        this.UseUrlWithoutSchemeAndSubdomain = true;
    }

    public override async Task<IUrlMedia?> OnReplace(IUrlMedia media)
    {

        //media.ApplyFullScreen = true;
        media.Javascript = "var videoconfigd=document.getElementsByTagName('video')[0];videoconfigd.play();";
        media.Javascript += "var videoconfigd=document.getElementsByTagName('video')[0];videoconfigd.requestFullscreen();";

        return await base.OnReplace(media);
    }
}
