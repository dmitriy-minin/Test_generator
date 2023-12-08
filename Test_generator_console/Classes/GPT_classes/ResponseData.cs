using System.Text.Json.Serialization;

namespace Test_generator_console.Classes.GPT_classes
{
    internal class ResponseData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = "";
        [JsonPropertyName("object")]
        public string Object { get; set; } = "";
        [JsonPropertyName("created")]
        public ulong Created { get; set; }
        [JsonPropertyName("choices")]
        public List<ChoiceClass> Choices { get; set; } = new();
        [JsonPropertyName("usage")]
        public UsageClass Usage { get; set; } = new();
    }
}
