using System.Text.Json.Serialization;

namespace Test_generator_console.Classes.GPT_classes
{
    internal class MessageClass
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = "";
        [JsonPropertyName("content")]
        public string Content { get; set; } = "";
    }
}
