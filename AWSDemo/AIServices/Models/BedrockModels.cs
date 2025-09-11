namespace AWSDemo.AIServices.Models
{
    public class TextGenerationRequest
    {
        public string Prompt { get; set; } = string.Empty;
        public string ModelId { get; set; } = "anthropic.claude-3-sonnet-20240229-v1:0";
        public int MaxTokens { get; set; } = 1000;
        public float Temperature { get; set; } = 0.7f;
        public float TopP { get; set; } = 0.9f;
    }

    public class TextGenerationResponse
    {
        public string GeneratedText { get; set; } = string.Empty;
        public string StopReason { get; set; } = string.Empty;
        public int TokensUsed { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class BedrockModelInfo
    {
        public string ModelId { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class ListModelsResponse
    {
        public List<BedrockModelInfo> Models { get; set; } = new();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class ImageGenerationRequest
    {
        public string Prompt { get; set; } = string.Empty;
        public string ModelId { get; set; } = "amazon.titan-image-generator-v1";
        public int Width { get; set; } = 512;
        public int Height { get; set; } = 512;
        public string Quality { get; set; } = "standard";
    }

    public class ImageGenerationResponse
    {
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = "image/png";
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}