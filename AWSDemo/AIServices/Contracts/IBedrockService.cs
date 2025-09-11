using AWSDemo.AIServices.Models;
using Common;

namespace AWSDemo.AIServices.Contracts
{
    public interface IBedrockService
    {
        Task<Response<TextGenerationResponse>> GenerateTextAsync(TextGenerationRequest request);
        Task<Response<ImageGenerationResponse>> GenerateImageAsync(ImageGenerationRequest request);
        Task<Response<ListModelsResponse>> GetAvailableModelsAsync();
    }
}