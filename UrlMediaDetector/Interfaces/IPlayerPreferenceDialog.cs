namespace UrlMediaDetector.Interfaces;

public interface IPlayerPreferenceDialog
{
    Task<bool> Show();
}