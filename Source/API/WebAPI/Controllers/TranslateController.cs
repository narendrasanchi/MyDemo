using Amazon.Translate;
using Amazon.Translate.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TranslateController : ControllerBase
{
    private readonly IAmazonTranslate _translateClient;
    private readonly ILogger<TranslateController> _logger;

    public TranslateController(IAmazonTranslate translateClient, ILogger<TranslateController> logger)
    {
        _translateClient = translateClient;
        _logger = logger;
    }

    [HttpPost("translate")]
    public async Task<IActionResult> TranslateText([FromBody] TranslationRequest request)
    {
        try
        {
            var translateRequest = new TranslateTextRequest
            {
                Text = request.Text,
                SourceLanguageCode = request.SourceLanguageCode,
                TargetLanguageCode = request.TargetLanguageCode
            };

            var response = await _translateClient.TranslateTextAsync(translateRequest);

            return Ok(new TranslationResponse
            {
                OriginalText = request.Text,
                TranslatedText = response.TranslatedText,
                SourceLanguageCode = response.SourceLanguageCode,
                TargetLanguageCode = response.TargetLanguageCode,
                DetectedSourceLanguageCode = response.SourceLanguageCode
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error translating text: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("detect-language")]
    public async Task<IActionResult> DetectLanguage([FromBody] DetectLanguageRequest request)
    {
        try
        {
            // AWS Translate doesn't have a separate detect language API
            // We can use auto detection by setting source language to "auto"
            var translateRequest = new TranslateTextRequest
            {
                Text = request.Text,
                SourceLanguageCode = "auto",
                TargetLanguageCode = "en" // Translate to English to detect source
            };

            var response = await _translateClient.TranslateTextAsync(translateRequest);

            return Ok(new LanguageDetectionResponse
            {
                Text = request.Text,
                DetectedLanguageCode = response.SourceLanguageCode,
                Confidence = 0.95 // AWS doesn't provide confidence score directly
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting language: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("supported-languages")]
    public IActionResult GetSupportedLanguages()
    {
        var supportedLanguages = new[]
        {
            new { Code = "auto", Name = "Auto-detect" },
            new { Code = "ar", Name = "Arabic" },
            new { Code = "zh", Name = "Chinese (Simplified)" },
            new { Code = "zh-TW", Name = "Chinese (Traditional)" },
            new { Code = "cs", Name = "Czech" },
            new { Code = "da", Name = "Danish" },
            new { Code = "nl", Name = "Dutch" },
            new { Code = "en", Name = "English" },
            new { Code = "fi", Name = "Finnish" },
            new { Code = "fr", Name = "French" },
            new { Code = "de", Name = "German" },
            new { Code = "he", Name = "Hebrew" },
            new { Code = "hi", Name = "Hindi" },
            new { Code = "id", Name = "Indonesian" },
            new { Code = "it", Name = "Italian" },
            new { Code = "ja", Name = "Japanese" },
            new { Code = "ko", Name = "Korean" },
            new { Code = "ms", Name = "Malay" },
            new { Code = "no", Name = "Norwegian" },
            new { Code = "fa", Name = "Persian" },
            new { Code = "pl", Name = "Polish" },
            new { Code = "pt", Name = "Portuguese" },
            new { Code = "ru", Name = "Russian" },
            new { Code = "es", Name = "Spanish" },
            new { Code = "sv", Name = "Swedish" },
            new { Code = "tr", Name = "Turkish" },
            new { Code = "uk", Name = "Ukrainian" }
        };

        return Ok(supportedLanguages);
    }

    [HttpPost("batch-translate")]
    public async Task<IActionResult> BatchTranslate([FromBody] BatchTranslationRequest request)
    {
        try
        {
            var results = new List<TranslationResponse>();

            foreach (var text in request.Texts)
            {
                var translateRequest = new TranslateTextRequest
                {
                    Text = text,
                    SourceLanguageCode = request.SourceLanguageCode,
                    TargetLanguageCode = request.TargetLanguageCode
                };

                var response = await _translateClient.TranslateTextAsync(translateRequest);

                results.Add(new TranslationResponse
                {
                    OriginalText = text,
                    TranslatedText = response.TranslatedText,
                    SourceLanguageCode = response.SourceLanguageCode,
                    TargetLanguageCode = response.TargetLanguageCode,
                    DetectedSourceLanguageCode = response.SourceLanguageCode
                });
            }

            return Ok(new BatchTranslationResponse
            {
                Results = results,
                TotalCount = results.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in batch translation: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }
}

public class TranslationRequest
{
    [Required]
    [StringLength(5000, MinimumLength = 1)]
    public string Text { get; set; } = string.Empty;

    [Required]
    public string SourceLanguageCode { get; set; } = "auto";

    [Required]
    public string TargetLanguageCode { get; set; } = "en";
}

public class DetectLanguageRequest
{
    [Required]
    [StringLength(5000, MinimumLength = 1)]
    public string Text { get; set; } = string.Empty;
}

public class BatchTranslationRequest
{
    [Required]
    public List<string> Texts { get; set; } = new();

    [Required]
    public string SourceLanguageCode { get; set; } = "auto";

    [Required]
    public string TargetLanguageCode { get; set; } = "en";
}

public class TranslationResponse
{
    public string OriginalText { get; set; } = string.Empty;
    public string TranslatedText { get; set; } = string.Empty;
    public string SourceLanguageCode { get; set; } = string.Empty;
    public string TargetLanguageCode { get; set; } = string.Empty;
    public string DetectedSourceLanguageCode { get; set; } = string.Empty;
}

public class LanguageDetectionResponse
{
    public string Text { get; set; } = string.Empty;
    public string DetectedLanguageCode { get; set; } = string.Empty;
    public double Confidence { get; set; }
}

public class BatchTranslationResponse
{
    public List<TranslationResponse> Results { get; set; } = new();
    public int TotalCount { get; set; }
}