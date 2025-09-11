using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Common.Enum;
using Microsoft.Extensions.Logging;

namespace AWSDemo.AIServices.Services
{
    public class ComprehendService : IComprehendService
    {
        private readonly AmazonComprehendClient _comprehendClient;
        private readonly ILogger<ComprehendService> _logger;

        public ComprehendService(AmazonComprehendClient comprehendClient, ILogger<ComprehendService> logger)
        {
            _comprehendClient = comprehendClient;
            _logger = logger;
        }

        public async Task<Response<SentimentAnalysisResponse>> DetectSentimentAsync(SentimentAnalysisRequest request)
        {
            try
            {
                var detectRequest = new DetectSentimentRequest
                {
                    Text = request.Text,
                    LanguageCode = LanguageCode.FindValue(request.LanguageCode)
                };

                var response = await _comprehendClient.DetectSentimentAsync(detectRequest);

                var result = new SentimentAnalysisResponse
                {
                    Sentiment = response.Sentiment.Value,
                    SentimentScore = new Models.SentimentScore
                    {
                        Positive = response.SentimentScore.Positive,
                        Negative = response.SentimentScore.Negative,
                        Neutral = response.SentimentScore.Neutral,
                        Mixed = response.SentimentScore.Mixed
                    },
                    Success = true
                };

                return new Response<SentimentAnalysisResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting sentiment");
                return new Response<SentimentAnalysisResponse>(
                    new SentimentAnalysisResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<EntityDetectionResponse>> DetectEntitiesAsync(EntityDetectionRequest request)
        {
            try
            {
                var detectRequest = new DetectEntitiesRequest
                {
                    Text = request.Text,
                    LanguageCode = LanguageCode.FindValue(request.LanguageCode)
                };

                var response = await _comprehendClient.DetectEntitiesAsync(detectRequest);

                var entities = response.Entities.Select(e => new DetectedEntity
                {
                    Text = e.Text,
                    Type = e.Type.Value,
                    Score = e.Score,
                    BeginOffset = e.BeginOffset,
                    EndOffset = e.EndOffset
                }).ToList();

                var result = new EntityDetectionResponse
                {
                    Entities = entities,
                    Success = true
                };

                return new Response<EntityDetectionResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting entities");
                return new Response<EntityDetectionResponse>(
                    new EntityDetectionResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<KeyPhrasesResponse>> DetectKeyPhrasesAsync(KeyPhrasesRequest request)
        {
            try
            {
                var detectRequest = new DetectKeyPhrasesRequest
                {
                    Text = request.Text,
                    LanguageCode = LanguageCode.FindValue(request.LanguageCode)
                };

                var response = await _comprehendClient.DetectKeyPhrasesAsync(detectRequest);

                var keyPhrases = response.KeyPhrases.Select(kp => new Models.KeyPhrase
                {
                    Text = kp.Text,
                    Score = kp.Score,
                    BeginOffset = kp.BeginOffset,
                    EndOffset = kp.EndOffset
                }).ToList();

                var result = new KeyPhrasesResponse
                {
                    KeyPhrases = keyPhrases,
                    Success = true
                };

                return new Response<KeyPhrasesResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting key phrases");
                return new Response<KeyPhrasesResponse>(
                    new KeyPhrasesResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<DetectLanguageResponse>> DetectDominantLanguageAsync(DetectLanguageRequest request)
        {
            try
            {
                var detectRequest = new DetectDominantLanguageRequest
                {
                    Text = request.Text
                };

                var response = await _comprehendClient.DetectDominantLanguageAsync(detectRequest);
                var dominantLanguage = response.Languages.FirstOrDefault();

                var result = new DetectLanguageResponse
                {
                    LanguageCode = dominantLanguage?.LanguageCode ?? string.Empty,
                    Score = dominantLanguage?.Score ?? 0,
                    Success = true
                };

                return new Response<DetectLanguageResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting dominant language");
                return new Response<DetectLanguageResponse>(
                    new DetectLanguageResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }
    }
}