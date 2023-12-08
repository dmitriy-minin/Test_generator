using System.Text.Json.Serialization;

namespace Test_generator_console.Classes.GPT_classes
{
    internal class RequestClass
    {
        [JsonPropertyName("model")]
        public string ModelId { get; set; } = "";
        [JsonPropertyName("messages")]
        public List<MessageClass> Messages { get; set; } = new();
    }
}
