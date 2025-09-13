using AWSDemo.AIServices.Models;
using Common;

namespace AWSDemo.AIServices.Contracts
{
    public interface IPollyService
    {
        Task<Response<TextToSpeechResponse>> SynthesizeSpeechAsync(TextToSpeechRequest request);
        Task<Response<List<string>>> GetAvailableVoicesAsync(string languageCode = "");
    }
}