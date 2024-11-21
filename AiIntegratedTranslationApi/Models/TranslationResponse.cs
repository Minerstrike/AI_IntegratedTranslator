using System.Text.Json.Serialization;

namespace AiIntegratedTranslationApi.Models;


public struct AI_TranslationResponse_Json
{
    public AI_TranslationResponse_Json() { }

    [JsonPropertyName("Target")] public string Target { get; set; } = nameof(Target) + "_value";
    [JsonPropertyName("TargetTranslated")] public string TargetTranslated { get; set; } = nameof(TargetTranslated) + "_value";
    [JsonPropertyName("ExSentence")] public string ExSentence { get; set; } = nameof(ExSentence) + "_value";
    [JsonPropertyName("ExSentenceTranslated")] public string ExSentenceTranslated { get; set; } = nameof(ExSentenceTranslated) + "_value";
    //[JsonPropertyName("Definition")]            public string   Definition              { get; set; } = nameof(Definition) + "_value";
    //[JsonPropertyName("DefinitionTranslated")]  public string   DefinitionTranslated    { get; set; } = nameof(DefinitionTranslated) + "_value";
}
