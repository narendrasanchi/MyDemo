using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComprehendController : ControllerBase
{
    private readonly IAmazonComprehend _comprehendClient;
    private readonly ILogger<ComprehendController> _logger;

    public ComprehendController(IAmazonComprehend comprehendClient, ILogger<ComprehendController> logger)
    {
        _comprehendClient = comprehendClient;
        _logger = logger;
    }

    [HttpPost("sentiment")]
    public async Task<IActionResult> AnalyzeSentiment([FromBody] SentimentAnalysisRequest request)
    {
        try
        {
            var sentimentRequest = new DetectSentimentRequest
            {
                Text = request.Text,
                LanguageCode = request.LanguageCode
            };

            var response = await _comprehendClient.DetectSentimentAsync(sentimentRequest);

            return Ok(new SentimentAnalysisResponse
            {
                Text = request.Text,
                Sentiment = response.Sentiment.Value,
                SentimentScore = new SentimentScores
                {
                    Positive = response.SentimentScore.Positive,
                    Negative = response.SentimentScore.Negative,
                    Neutral = response.SentimentScore.Neutral,
                    Mixed = response.SentimentScore.Mixed
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing sentiment: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("entities")]
    public async Task<IActionResult> DetectEntities([FromBody] EntityDetectionRequest request)
    {
        try
        {
            var entitiesRequest = new DetectEntitiesRequest
            {
                Text = request.Text,
                LanguageCode = request.LanguageCode
            };

            var response = await _comprehendClient.DetectEntitiesAsync(entitiesRequest);

            var entities = response.Entities.Select(e => new EntityInfo
            {
                Text = e.Text,
                Type = e.Type.Value,
                Score = e.Score,
                BeginOffset = e.BeginOffset,
                EndOffset = e.EndOffset
            }).ToList();

            return Ok(new EntityDetectionResponse
            {
                Text = request.Text,
                Entities = entities,
                EntityCount = entities.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting entities: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("key-phrases")]
    public async Task<IActionResult> DetectKeyPhrases([FromBody] KeyPhrasesRequest request)
    {
        try
        {
            var keyPhrasesRequest = new DetectKeyPhrasesRequest
            {
                Text = request.Text,
                LanguageCode = request.LanguageCode
            };

            var response = await _comprehendClient.DetectKeyPhrasesAsync(keyPhrasesRequest);

            var keyPhrases = response.KeyPhrases.Select(kp => new KeyPhraseInfo
            {
                Text = kp.Text,
                Score = kp.Score,
                BeginOffset = kp.BeginOffset,
                EndOffset = kp.EndOffset
            }).ToList();

            return Ok(new KeyPhrasesResponse
            {
                Text = request.Text,
                KeyPhrases = keyPhrases,
                KeyPhrasesCount = keyPhrases.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting key phrases: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("language")]
    public async Task<IActionResult> DetectDominantLanguage([FromBody] LanguageDetectionRequest request)
    {
        try
        {
            var languageRequest = new DetectDominantLanguageRequest
            {
                Text = request.Text
            };

            var response = await _comprehendClient.DetectDominantLanguageAsync(languageRequest);

            var languages = response.Languages.Select(l => new DominantLanguage
            {
                LanguageCode = l.LanguageCode,
                Score = l.Score
            }).ToList();

            return Ok(new DominantLanguageResponse
            {
                Text = request.Text,
                Languages = languages,
                PrimaryLanguage = languages.OrderByDescending(l => l.Score).FirstOrDefault()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error detecting dominant language: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("comprehensive-analysis")]
    public async Task<IActionResult> ComprehensiveAnalysis([FromBody] ComprehensiveAnalysisRequest request)
    {
        try
        {
            // Perform all analyses in parallel
            var sentimentTask = _comprehendClient.DetectSentimentAsync(new DetectSentimentRequest
            {
                Text = request.Text,
                LanguageCode = request.LanguageCode
            });

            var entitiesTask = _comprehendClient.DetectEntitiesAsync(new DetectEntitiesRequest
            {
                Text = request.Text,
                LanguageCode = request.LanguageCode
            });

            var keyPhrasesTask = _comprehendClient.DetectKeyPhrasesAsync(new DetectKeyPhrasesRequest
            {
                Text = request.Text,
                LanguageCode = request.LanguageCode
            });

            var languageTask = _comprehendClient.DetectDominantLanguageAsync(new DetectDominantLanguageRequest
            {
                Text = request.Text
            });

            await Task.WhenAll(sentimentTask, entitiesTask, keyPhrasesTask, languageTask);

            var sentimentResponse = await sentimentTask;
            var entitiesResponse = await entitiesTask;
            var keyPhrasesResponse = await keyPhrasesTask;
            var languageResponse = await languageTask;

            return Ok(new ComprehensiveAnalysisResponse
            {
                Text = request.Text,
                Sentiment = new SentimentAnalysisResponse
                {
                    Text = request.Text,
                    Sentiment = sentimentResponse.Sentiment.Value,
                    SentimentScore = new SentimentScores
                    {
                        Positive = sentimentResponse.SentimentScore.Positive,
                        Negative = sentimentResponse.SentimentScore.Negative,
                        Neutral = sentimentResponse.SentimentScore.Neutral,
                        Mixed = sentimentResponse.SentimentScore.Mixed
                    }
                },
                Entities = entitiesResponse.Entities.Select(e => new EntityInfo
                {
                    Text = e.Text,
                    Type = e.Type.Value,
                    Score = e.Score,
                    BeginOffset = e.BeginOffset,
                    EndOffset = e.EndOffset
                }).ToList(),
                KeyPhrases = keyPhrasesResponse.KeyPhrases.Select(kp => new KeyPhraseInfo
                {
                    Text = kp.Text,
                    Score = kp.Score,
                    BeginOffset = kp.BeginOffset,
                    EndOffset = kp.EndOffset
                }).ToList(),
                DetectedLanguages = languageResponse.Languages.Select(l => new DominantLanguage
                {
                    LanguageCode = l.LanguageCode,
                    Score = l.Score
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error performing comprehensive analysis: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }
}

public class SentimentAnalysisRequest
{
    [Required]
    [StringLength(5000, MinimumLength = 1)]
    public string Text { get; set; } = string.Empty;

    [Required]
    public string LanguageCode { get; set; } = "en";
}

public class EntityDetectionRequest
{
    [Required]
    [StringLength(5000, MinimumLength = 1)]
    public string Text { get; set; } = string.Empty;

    [Required]
    public string LanguageCode { get; set; } = "en";
}

public class KeyPhrasesRequest
{
    [Required]
    [StringLength(5000, MinimumLength = 1)]
    public string Text { get; set; } = string.Empty;

    [Required]
    public string LanguageCode { get; set; } = "en";
}

public class LanguageDetectionRequest
{
    [Required]
    [StringLength(5000, MinimumLength = 1)]
    public string Text { get; set; } = string.Empty;
}

public class ComprehensiveAnalysisRequest
{
    [Required]
    [StringLength(5000, MinimumLength = 1)]
    public string Text { get; set; } = string.Empty;

    [Required]
    public string LanguageCode { get; set; } = "en";
}

public class SentimentAnalysisResponse
{
    public string Text { get; set; } = string.Empty;
    public string Sentiment { get; set; } = string.Empty;
    public SentimentScores SentimentScore { get; set; } = new();
}

public class SentimentScores
{
    public float Positive { get; set; }
    public float Negative { get; set; }
    public float Neutral { get; set; }
    public float Mixed { get; set; }
}

public class EntityDetectionResponse
{
    public string Text { get; set; } = string.Empty;
    public List<EntityInfo> Entities { get; set; } = new();
    public int EntityCount { get; set; }
}

public class EntityInfo
{
    public string Text { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public float Score { get; set; }
    public int BeginOffset { get; set; }
    public int EndOffset { get; set; }
}

public class KeyPhrasesResponse
{
    public string Text { get; set; } = string.Empty;
    public List<KeyPhraseInfo> KeyPhrases { get; set; } = new();
    public int KeyPhrasesCount { get; set; }
}

public class KeyPhraseInfo
{
    public string Text { get; set; } = string.Empty;
    public float Score { get; set; }
    public int BeginOffset { get; set; }
    public int EndOffset { get; set; }
}

public class DominantLanguageResponse
{
    public string Text { get; set; } = string.Empty;
    public List<DominantLanguage> Languages { get; set; } = new();
    public DominantLanguage? PrimaryLanguage { get; set; }
}

public class DominantLanguage
{
    public string LanguageCode { get; set; } = string.Empty;
    public float Score { get; set; }
}

public class ComprehensiveAnalysisResponse
{
    public string Text { get; set; } = string.Empty;
    public SentimentAnalysisResponse Sentiment { get; set; } = new();
    public List<EntityInfo> Entities { get; set; } = new();
    public List<KeyPhraseInfo> KeyPhrases { get; set; } = new();
    public List<DominantLanguage> DetectedLanguages { get; set; } = new();
}