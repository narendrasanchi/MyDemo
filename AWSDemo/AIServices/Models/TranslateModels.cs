namespace AWSDemo.AIServices.Models
{
    public class TranslateTextRequest
    {
        public string Text { get; set; } = string.Empty;
        public string SourceLanguageCode { get; set; } = "auto";
        public string TargetLanguageCode { get; set; } = "en";
    }

    public class TranslateTextResponse
    {
        public string TranslatedText { get; set; } = string.Empty;
        public string SourceLanguageCode { get; set; } = string.Empty;
        public string TargetLanguageCode { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class DetectLanguageRequest
    {
        public string Text { get; set; } = string.Empty;
    }

    public class DetectLanguageResponse
    {
        public string LanguageCode { get; set; } = string.Empty;
        public float Score { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}