using Amazon.Polly;
using Amazon.Polly.Model;
using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Common.Enum;
using Microsoft.Extensions.Logging;

namespace AWSDemo.AIServices.Services
{
    public class PollyService : IPollyService
    {
        private readonly AmazonPollyClient _pollyClient;
        private readonly ILogger<PollyService> _logger;

        public PollyService(AmazonPollyClient pollyClient, ILogger<PollyService> logger)
        {
            _pollyClient = pollyClient;
            _logger = logger;
        }

        public async Task<Response<TextToSpeechResponse>> SynthesizeSpeechAsync(TextToSpeechRequest request)
        {
            try
            {
                var synthesizeSpeechRequest = new SynthesizeSpeechRequest
                {
                    Text = request.Text,
                    VoiceId = VoiceId.FindValue(request.VoiceId),
                    OutputFormat = request.OutputFormat
                };

                var response = await _pollyClient.SynthesizeSpeechAsync(synthesizeSpeechRequest);

                using var memoryStream = new MemoryStream();
                await response.AudioStream.CopyToAsync(memoryStream);

                var result = new TextToSpeechResponse
                {
                    AudioData = memoryStream.ToArray(),
                    ContentType = response.ContentType,
                    Success = true
                };

                return new Response<TextToSpeechResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error synthesizing speech");
                return new Response<TextToSpeechResponse>(
                    new TextToSpeechResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<List<string>>> GetAvailableVoicesAsync(string languageCode = "")
        {
            try
            {
                var request = new DescribeVoicesRequest();
                if (!string.IsNullOrEmpty(languageCode))
                {
                    request.LanguageCode = LanguageCode.FindValue(languageCode);
                }

                var response = await _pollyClient.DescribeVoicesAsync(request);
                var voices = response.Voices.Select(v => v.Id.Value).ToList();

                return new Response<List<string>>(voices, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available voices");
                return new Response<List<string>>(new List<string>(), ResponseCode.BadRequest);
            }
        }
    }
}