using UrlMediaDetector.Enums;

namespace UrlMediaDetector.Models;

public class ServicePatternModel
{
    public ServicePatternModel(string pattern, ServiceNameEnum name, MediaEnum mediaType)
    {
        this.Pattern = pattern;
        this.ServiceName = name;
        this.MediaType = mediaType;
    }

    public string Pattern { get; }
    public ServiceNameEnum ServiceName { get; }
    public MediaEnum MediaType { get; }

    public override string ToString()
    {
        return $"ServiceName: {ServiceName}, MediaType: {MediaType}";
    }
}