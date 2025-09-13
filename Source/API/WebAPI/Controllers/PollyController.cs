using Amazon.Polly;
using Amazon.Polly.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PollyController : ControllerBase
{
    private readonly IAmazonPolly _pollyClient;
    private readonly ILogger<PollyController> _logger;

    public PollyController(IAmazonPolly pollyClient, ILogger<PollyController> logger)
    {
        _pollyClient = pollyClient;
        _logger = logger;
    }

    [HttpPost("text-to-speech")]
    public async Task<IActionResult> ConvertTextToSpeech([FromBody] TextToSpeechRequest request)
    {
        try
        {
            var synthesizeSpeechRequest = new SynthesizeSpeechRequest
            {
                Text = request.Text,
                OutputFormat = OutputFormat.Mp3,
                VoiceId = request.VoiceId,
                Engine = request.Neural ? Engine.Neural : Engine.Standard
            };

            var response = await _pollyClient.SynthesizeSpeechAsync(synthesizeSpeechRequest);
            
            var memoryStream = new MemoryStream();
            await response.AudioStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            return File(memoryStream.ToArray(), "audio/mpeg", $"speech_{DateTime.UtcNow.Ticks}.mp3");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error converting text to speech: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("voices")]
    public async Task<IActionResult> GetVoices([FromQuery] string? languageCode = null)
    {
        try
        {
            var request = new DescribeVoicesRequest();
            if (!string.IsNullOrEmpty(languageCode))
            {
                request.LanguageCode = languageCode;
            }

            var response = await _pollyClient.DescribeVoicesAsync(request);
            var voices = response.Voices.Select(v => new
            {
                Id = v.Id,
                Name = v.Name,
                Gender = v.Gender,
                LanguageCode = v.LanguageCode,
                LanguageName = v.LanguageName,
                SupportedEngines = v.SupportedEngines
            });

            return Ok(voices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting voices: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("languages")]
    public async Task<IActionResult> GetLanguages()
    {
        try
        {
            var response = await _pollyClient.DescribeVoicesAsync(new DescribeVoicesRequest());
            var languages = response.Voices
                .GroupBy(v => v.LanguageCode)
                .Select(g => new
                {
                    Code = g.Key,
                    Name = g.First().LanguageName,
                    VoiceCount = g.Count()
                })
                .OrderBy(l => l.Name);

            return Ok(languages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting languages: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }
}

public class TextToSpeechRequest
{
    [Required]
    [StringLength(3000, MinimumLength = 1)]
    public string Text { get; set; } = string.Empty;

    [Required]
    public string VoiceId { get; set; } = "Joanna";

    public bool Neural { get; set; } = false;
}