using System.Text.Json.Serialization;

namespace Test_generator_console.Classes.GPT_classes
{
    internal class ChoiceClass
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("message")]
        public MessageClass Message { get; set; } = new();
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = "";
    }
}
