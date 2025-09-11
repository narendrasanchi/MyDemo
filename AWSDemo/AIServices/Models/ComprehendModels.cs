namespace AWSDemo.AIServices.Models
{
    public class SentimentAnalysisRequest
    {
        public string Text { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = "en";
    }

    public class SentimentAnalysisResponse
    {
        public string Sentiment { get; set; } = string.Empty;
        public SentimentScore SentimentScore { get; set; } = new();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class SentimentScore
    {
        public float Positive { get; set; }
        public float Negative { get; set; }
        public float Neutral { get; set; }
        public float Mixed { get; set; }
    }

    public class EntityDetectionRequest
    {
        public string Text { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = "en";
    }

    public class EntityDetectionResponse
    {
        public List<DetectedEntity> Entities { get; set; } = new();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class DetectedEntity
    {
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public float Score { get; set; }
        public int BeginOffset { get; set; }
        public int EndOffset { get; set; }
    }

    public class KeyPhrasesRequest
    {
        public string Text { get; set; } = string.Empty;
        public string LanguageCode { get; set; } = "en";
    }

    public class KeyPhrasesResponse
    {
        public List<KeyPhrase> KeyPhrases { get; set; } = new();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class KeyPhrase
    {
        public string Text { get; set; } = string.Empty;
        public float Score { get; set; }
        public int BeginOffset { get; set; }
        public int EndOffset { get; set; }
    }
}