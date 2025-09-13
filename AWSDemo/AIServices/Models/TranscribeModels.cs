namespace AWSDemo.AIServices.Models
{
    public class TranscribeRequest
    {
        public string S3BucketName { get; set; } = string.Empty;
        public string S3ObjectKey { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = "en-US";
        public string JobName { get; set; } = string.Empty;
    }

    public class TranscribeResponse
    {
        public string JobName { get; set; } = string.Empty;
        public string JobStatus { get; set; } = string.Empty;
        public string TranscriptText { get; set; } = string.Empty;
        public string TranscriptFileUri { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class TranscribeJobStatusResponse
    {
        public string JobName { get; set; } = string.Empty;
        public string JobStatus { get; set; } = string.Empty;
        public string TranscriptText { get; set; } = string.Empty;
        public double? CompletionProgress { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}