using System.Text.Json.Serialization;

namespace UrlMediaDetector.Models.Contexts
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(oEmbedModel))]
    [JsonSerializable(typeof(OKModel))]
    [JsonSerializable(typeof(RumbleModel))]
    [JsonSerializable(typeof(ServicePatternModel))]
    public partial class JsonContext : JsonSerializerContext
    {
    }
}