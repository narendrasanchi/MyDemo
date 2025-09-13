using AWSDemo.AIServices.Models;
using Common;

namespace AWSDemo.AIServices.Contracts
{
    public interface IComprehendService
    {
        Task<Response<SentimentAnalysisResponse>> DetectSentimentAsync(SentimentAnalysisRequest request);
        Task<Response<EntityDetectionResponse>> DetectEntitiesAsync(EntityDetectionRequest request);
        Task<Response<KeyPhrasesResponse>> DetectKeyPhrasesAsync(KeyPhrasesRequest request);
        Task<Response<DetectLanguageResponse>> DetectDominantLanguageAsync(DetectLanguageRequest request);
    }
}