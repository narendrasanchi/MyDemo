using AWSDemo.AIServices.Models;
using Common;

namespace AWSDemo.AIServices.Contracts
{
    public interface ITranslateService
    {
        Task<Response<Models.TranslateTextResponse>> TranslateTextAsync(Models.TranslateTextRequest request);
        Task<Response<Models.DetectLanguageResponse>> DetectDominantLanguageAsync(Models.DetectLanguageRequest request);
        Task<Response<List<string>>> GetSupportedLanguagesAsync();
    }
}