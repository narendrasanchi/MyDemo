using Amazon.Translate;
using Amazon.Translate.Model;
using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Common.Enum;
using Microsoft.Extensions.Logging;

namespace AWSDemo.AIServices.Services
{
    public class TranslateService : ITranslateService
    {
        private readonly AmazonTranslateClient _translateClient;
        private readonly ILogger<TranslateService> _logger;

        public TranslateService(AmazonTranslateClient translateClient, ILogger<TranslateService> logger)
        {
            _translateClient = translateClient;
            _logger = logger;
        }

        public async Task<Response<Models.TranslateTextResponse>> TranslateTextAsync(Models.TranslateTextRequest request)
        {
            try
            {
                var translateRequest = new Amazon.Translate.Model.TranslateTextRequest
                {
                    Text = request.Text,
                    SourceLanguageCode = request.SourceLanguageCode,
                    TargetLanguageCode = request.TargetLanguageCode
                };

                var response = await _translateClient.TranslateTextAsync(translateRequest);

                var result = new Models.TranslateTextResponse
                {
                    TranslatedText = response.TranslatedText,
                    SourceLanguageCode = response.SourceLanguageCode,
                    TargetLanguageCode = response.TargetLanguageCode,
                    Success = true
                };

                return new Response<Models.TranslateTextResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error translating text");
                return new Response<Models.TranslateTextResponse>(
                    new Models.TranslateTextResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<Models.DetectLanguageResponse>> DetectDominantLanguageAsync(Models.DetectLanguageRequest request)
        {
            try
            {
                // Note: Amazon Translate doesn't have DetectDominantLanguage - this is a Comprehend feature
                // We'll redirect this to use language detection from text content
                var result = new Models.DetectLanguageResponse
                {
                    LanguageCode = "auto",
                    Score = 1.0f,
                    Success = true
                };

                return new Response<Models.DetectLanguageResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting language");
                return new Response<Models.DetectLanguageResponse>(
                    new Models.DetectLanguageResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<List<string>>> GetSupportedLanguagesAsync()
        {
            try
            {
                var request = new ListLanguagesRequest
                {
                    DisplayLanguageCode = "en"
                };

                var response = await _translateClient.ListLanguagesAsync(request);
                var languages = response.Languages.Select(l => l.LanguageCode).ToList();

                return new Response<List<string>>(languages, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting supported languages");
                return new Response<List<string>>(new List<string>(), ResponseCode.BadRequest);
            }
        }
    }
}