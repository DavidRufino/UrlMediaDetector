namespace UrlMediaDetector.Interfaces;

public interface IServiceFinder
{
    Task<(bool, IUrlMedia?)> Match(IUrlMedia serviceDatum);

    Task<IUrlMedia> OnReplace(IUrlMedia urlMediaModel);
    Task<IUrlMedia> OnReplace(IUrlMedia urlMediaModel, IPlayerPreferenceDialog dialog);
}
