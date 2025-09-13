namespace AWSDemo.AIServices.Models
{
    public class ImageAnalysisRequest
    {
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public string S3BucketName { get; set; } = string.Empty;
        public string S3ObjectKey { get; set; } = string.Empty;
        public int MaxLabels { get; set; } = 10;
        public float MinConfidence { get; set; } = 50.0f;
    }

    public class ImageAnalysisResponse
    {
        public List<DetectedLabel> Labels { get; set; } = new();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class DetectedLabel
    {
        public string Name { get; set; } = string.Empty;
        public float Confidence { get; set; }
        public List<string> Categories { get; set; } = new();
    }

    public class FaceDetectionRequest
    {
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public string S3BucketName { get; set; } = string.Empty;
        public string S3ObjectKey { get; set; } = string.Empty;
    }

    public class FaceDetectionResponse
    {
        public List<DetectedFace> Faces { get; set; } = new();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class DetectedFace
    {
        public BoundingBox BoundingBox { get; set; } = new();
        public List<Landmark> Landmarks { get; set; } = new();
        public float AgeRangeLow { get; set; }
        public float AgeRangeHigh { get; set; }
        public string Gender { get; set; } = string.Empty;
        public List<Emotion> Emotions { get; set; } = new();
    }

    public class BoundingBox
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public float Left { get; set; }
        public float Top { get; set; }
    }

    public class Landmark
    {
        public string Type { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class Emotion
    {
        public string Type { get; set; } = string.Empty;
        public float Confidence { get; set; }
    }

    public class ContentModerationRequest
    {
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public string S3BucketName { get; set; } = string.Empty;
        public string S3ObjectKey { get; set; } = string.Empty;
        public float MinConfidence { get; set; } = 50.0f;
    }

    public class ContentModerationResponse
    {
        public List<ModerationLabel> ModerationLabels { get; set; } = new();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class ModerationLabel
    {
        public string Name { get; set; } = string.Empty;
        public float Confidence { get; set; }
        public string ParentName { get; set; } = string.Empty;
    }
}