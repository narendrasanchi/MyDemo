using Amazon.Polly;

namespace AWSDemo.AIServices.Models
{
    public class TextToSpeechRequest
    {
        public string Text { get; set; } = string.Empty;
        public string VoiceId { get; set; } = "Joanna";
        public OutputFormat OutputFormat { get; set; } = OutputFormat.Mp3;
    }

    public class TextToSpeechResponse
    {
        public byte[] AudioData { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}