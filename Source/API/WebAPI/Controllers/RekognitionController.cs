using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RekognitionController : ControllerBase
{
    private readonly IAmazonRekognition _rekognitionClient;
    private readonly ILogger<RekognitionController> _logger;

    public RekognitionController(IAmazonRekognition rekognitionClient, ILogger<RekognitionController> logger)
    {
        _rekognitionClient = rekognitionClient;
        _logger = logger;
    }

    [HttpPost("detect-labels")]
    public async Task<IActionResult> DetectLabels(IFormFile imageFile, [FromQuery] int maxLabels = 10, [FromQuery] float minConfidence = 70)
    {
        try
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No image file uploaded.");
            }

            var imageBytes = await GetImageBytes(imageFile);

            var request = new DetectLabelsRequest
            {
                Image = new Image { Bytes = new MemoryStream(imageBytes) },
                MaxLabels = maxLabels,
                MinConfidence = minConfidence
            };

            var response = await _rekognitionClient.DetectLabelsAsync(request);

            var labels = response.Labels.Select(l => new LabelInfo
            {
                Name = l.Name,
                Confidence = l.Confidence,
                Instances = l.Instances?.Select(i => new InstanceInfo
                {
                    Confidence = i.Confidence,
                    BoundingBox = i.BoundingBox != null ? new BoundingBoxInfo
                    {
                        Left = i.BoundingBox.Left,
                        Top = i.BoundingBox.Top,
                        Width = i.BoundingBox.Width,
                        Height = i.BoundingBox.Height
                    } : null
                }).ToList() ?? new List<InstanceInfo>(),
                Categories = l.Categories?.Select(c => new CategoryInfo
                {
                    Name = c.Name
                }).ToList() ?? new List<CategoryInfo>()
            }).ToList();

            return Ok(new ImageLabelsResponse
            {
                Labels = labels,
                LabelCount = labels.Count,
                ImageOrientation = response.OrientationCorrection?.Value ?? "ROTATE_0"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting labels: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("detect-faces")]
    public async Task<IActionResult> DetectFaces(IFormFile imageFile)
    {
        try
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No image file uploaded.");
            }

            var imageBytes = await GetImageBytes(imageFile);

            var request = new DetectFacesRequest
            {
                Image = new Image { Bytes = new MemoryStream(imageBytes) },
                Attributes = new List<string> { "ALL" }
            };

            var response = await _rekognitionClient.DetectFacesAsync(request);

            var faces = response.FaceDetails.Select(f => new FaceInfo
            {
                Confidence = f.Confidence,
                BoundingBox = new BoundingBoxInfo
                {
                    Left = f.BoundingBox.Left,
                    Top = f.BoundingBox.Top,
                    Width = f.BoundingBox.Width,
                    Height = f.BoundingBox.Height
                },
                AgeRange = new AgeRangeInfo
                {
                    Low = f.AgeRange.Low,
                    High = f.AgeRange.High
                },
                Gender = f.Gender.Value,
                GenderConfidence = f.Gender.Confidence,
                Emotions = f.Emotions.Select(e => new EmotionInfo
                {
                    Type = e.Type.Value,
                    Confidence = e.Confidence
                }).ToList(),
                Smile = new SmileInfo
                {
                    Value = f.Smile.Value,
                    Confidence = f.Smile.Confidence
                },
                Eyeglasses = new EyeglassesInfo
                {
                    Value = f.Eyeglasses.Value,
                    Confidence = f.Eyeglasses.Confidence
                },
                Sunglasses = new SunglassesInfo
                {
                    Value = f.Sunglasses.Value,
                    Confidence = f.Sunglasses.Confidence
                },
                Beard = new BeardInfo
                {
                    Value = f.Beard.Value,
                    Confidence = f.Beard.Confidence
                },
                Mustache = new MustacheInfo
                {
                    Value = f.Mustache.Value,
                    Confidence = f.Mustache.Confidence
                }
            }).ToList();

            return Ok(new FaceDetectionResponse
            {
                Faces = faces,
                FaceCount = faces.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting faces: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("detect-moderation")]
    public async Task<IActionResult> DetectModerationLabels(IFormFile imageFile, [FromQuery] float minConfidence = 60)
    {
        try
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No image file uploaded.");
            }

            var imageBytes = await GetImageBytes(imageFile);

            var request = new DetectModerationLabelsRequest
            {
                Image = new Image { Bytes = new MemoryStream(imageBytes) },
                MinConfidence = minConfidence
            };

            var response = await _rekognitionClient.DetectModerationLabelsAsync(request);

            var moderationLabels = response.ModerationLabels.Select(m => new ModerationLabelInfo
            {
                Name = m.Name,
                Confidence = m.Confidence,
                ParentName = m.ParentName
            }).ToList();

            return Ok(new ModerationLabelsResponse
            {
                ModerationLabels = moderationLabels,
                ModerationLabelCount = moderationLabels.Count,
                IsContentAppropriate = moderationLabels.Count == 0,
                HumanLoopActivationOutput = response.HumanLoopActivationOutput?.HumanLoopActivationReasons != null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting moderation labels: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("detect-text")]
    public async Task<IActionResult> DetectText(IFormFile imageFile)
    {
        try
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No image file uploaded.");
            }

            var imageBytes = await GetImageBytes(imageFile);

            var request = new DetectTextRequest
            {
                Image = new Image { Bytes = new MemoryStream(imageBytes) }
            };

            var response = await _rekognitionClient.DetectTextAsync(request);

            var textDetections = response.TextDetections.Select(t => new TextDetectionInfo
            {
                DetectedText = t.DetectedText,
                Confidence = t.Confidence,
                Type = t.Type.Value,
                Id = t.Id,
                ParentId = t.ParentId,
                Geometry = new GeometryInfo
                {
                    BoundingBox = new BoundingBoxInfo
                    {
                        Left = t.Geometry.BoundingBox.Left,
                        Top = t.Geometry.BoundingBox.Top,
                        Width = t.Geometry.BoundingBox.Width,
                        Height = t.Geometry.BoundingBox.Height
                    }
                }
            }).ToList();

            return Ok(new TextDetectionResponse
            {
                TextDetections = textDetections,
                TextDetectionCount = textDetections.Count,
                ExtractedText = string.Join(" ", textDetections
                    .Where(t => t.Type == "WORD")
                    .OrderBy(t => t.Geometry.BoundingBox.Top)
                    .ThenBy(t => t.Geometry.BoundingBox.Left)
                    .Select(t => t.DetectedText))
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting text: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    private async Task<byte[]> GetImageBytes(IFormFile imageFile)
    {
        // Validate image format
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(fileExtension))
        {
            throw new ArgumentException($"Unsupported image format. Allowed formats: {string.Join(", ", allowedExtensions)}");
        }

        // Validate file size (5MB limit)
        if (imageFile.Length > 5 * 1024 * 1024)
        {
            throw new ArgumentException("Image file size cannot exceed 5MB.");
        }

        using var memoryStream = new MemoryStream();
        await imageFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}

public class ImageLabelsResponse
{
    public List<LabelInfo> Labels { get; set; } = new();
    public int LabelCount { get; set; }
    public string ImageOrientation { get; set; } = string.Empty;
}

public class LabelInfo
{
    public string Name { get; set; } = string.Empty;
    public float Confidence { get; set; }
    public List<InstanceInfo> Instances { get; set; } = new();
    public List<CategoryInfo> Categories { get; set; } = new();
}

public class InstanceInfo
{
    public float Confidence { get; set; }
    public BoundingBoxInfo? BoundingBox { get; set; }
}

public class CategoryInfo
{
    public string Name { get; set; } = string.Empty;
}

public class BoundingBoxInfo
{
    public float Left { get; set; }
    public float Top { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
}

public class FaceDetectionResponse
{
    public List<FaceInfo> Faces { get; set; } = new();
    public int FaceCount { get; set; }
}

public class FaceInfo
{
    public float Confidence { get; set; }
    public BoundingBoxInfo BoundingBox { get; set; } = new();
    public AgeRangeInfo AgeRange { get; set; } = new();
    public string Gender { get; set; } = string.Empty;
    public float GenderConfidence { get; set; }
    public List<EmotionInfo> Emotions { get; set; } = new();
    public SmileInfo Smile { get; set; } = new();
    public EyeglassesInfo Eyeglasses { get; set; } = new();
    public SunglassesInfo Sunglasses { get; set; } = new();
    public BeardInfo Beard { get; set; } = new();
    public MustacheInfo Mustache { get; set; } = new();
}

public class AgeRangeInfo
{
    public int Low { get; set; }
    public int High { get; set; }
}

public class EmotionInfo
{
    public string Type { get; set; } = string.Empty;
    public float Confidence { get; set; }
}

public class SmileInfo
{
    public bool Value { get; set; }
    public float Confidence { get; set; }
}

public class EyeglassesInfo
{
    public bool Value { get; set; }
    public float Confidence { get; set; }
}

public class SunglassesInfo
{
    public bool Value { get; set; }
    public float Confidence { get; set; }
}

public class BeardInfo
{
    public bool Value { get; set; }
    public float Confidence { get; set; }
}

public class MustacheInfo
{
    public bool Value { get; set; }
    public float Confidence { get; set; }
}

public class ModerationLabelsResponse
{
    public List<ModerationLabelInfo> ModerationLabels { get; set; } = new();
    public int ModerationLabelCount { get; set; }
    public bool IsContentAppropriate { get; set; }
    public bool HumanLoopActivationOutput { get; set; }
}

public class ModerationLabelInfo
{
    public string Name { get; set; } = string.Empty;
    public float Confidence { get; set; }
    public string ParentName { get; set; } = string.Empty;
}

public class TextDetectionResponse
{
    public List<TextDetectionInfo> TextDetections { get; set; } = new();
    public int TextDetectionCount { get; set; }
    public string ExtractedText { get; set; } = string.Empty;
}

public class TextDetectionInfo
{
    public string DetectedText { get; set; } = string.Empty;
    public float Confidence { get; set; }
    public string Type { get; set; } = string.Empty;
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public GeometryInfo Geometry { get; set; } = new();
}

public class GeometryInfo
{
    public BoundingBoxInfo BoundingBox { get; set; } = new();
}