using System.Diagnostics;
using System.Text.RegularExpressions;
using UrlMediaDetector.Enums;
using UrlMediaDetector.Interfaces;
using UrlMediaDetector.Models;

namespace UrlMediaDetector.Services.Base;

public abstract class BaseChannel : IServiceFinder
{
    protected ServiceNameEnum ServiceName { get; set; } = ServiceNameEnum.Unknown;

    public string HomePage = string.Empty;
    public string Javascript = string.Empty;
    public bool UseUrlWithoutSchemeAndSubdomain = true;

    public abstract IEnumerable<ServicePatternModel> Patterns { get; }

    public virtual async Task<(bool, IUrlMedia?)> Match(IUrlMedia serviceDatum)
    {
        // Determine which URL format to use
        string selectedUrl = UseUrlWithoutSchemeAndSubdomain
            ? serviceDatum.UrlWithoutSchemeAndSubdomain
            : serviceDatum.SanitizedUrl;

        // Find the first matching pattern using regex
        var patternMatch = Patterns.FirstOrDefault(p => Regex.IsMatch(selectedUrl, p.Pattern));

        if (patternMatch != null)
        {
            Debug.WriteLine($"[BaseChannel] Success Find Channel");
            Debug.WriteLine($"[BaseChannel] Match SanitizedUrl {serviceDatum.SanitizedUrl} ");
            Debug.WriteLine($"[BaseChannel] Match FullServiceUrl {serviceDatum.FullServiceUrl} ");
            Debug.WriteLine($"[BaseChannel] Match ReferrerUrl {serviceDatum.ReferrerUrl} ");
            Debug.WriteLine($"[BaseChannel] patternMatch.Service {patternMatch.ServiceName} ");
            Debug.WriteLine($"[BaseChannel] patternMatch.File {patternMatch.MediaType} ");

            serviceDatum.MediaType = patternMatch.MediaType;

            Debug.WriteLine($"[BaseChannel] serviceDatum.ServiceName {serviceDatum.ServiceName} ");
            Debug.WriteLine($"[BaseChannel] serviceDatum.MediaType {serviceDatum.MediaType} ");

            IUrlMedia service = await OnReplace(serviceDatum);

            return (true, service); // Return a tuple (true, matched service)
        }

        return (false, null); // Return false and no service when not matched
    }

    public virtual async Task<IUrlMedia> OnReplace(IUrlMedia urlMediaModel)
    {
        urlMediaModel.ServiceName = this.ServiceName;
        Debug.WriteLine($"[BaseChannel] urlMediaModel.ServiceName {urlMediaModel.ServiceName} ");
        urlMediaModel.HomePage = this.HomePage;
        return urlMediaModel;
    }

    public virtual async Task<IUrlMedia> OnReplace(IUrlMedia urlMediaModel, IPlayerPreferenceDialog dialog)
    {
        urlMediaModel.ServiceName = this.ServiceName;
        urlMediaModel.HomePage = this.HomePage;
        return urlMediaModel;
    }
}