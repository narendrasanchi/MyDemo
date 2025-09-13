using AWSDemo.AIServices.Models;
using Common;

namespace AWSDemo.AIServices.Contracts
{
    public interface IRekognitionService
    {
        Task<Response<ImageAnalysisResponse>> DetectLabelsAsync(ImageAnalysisRequest request);
        Task<Response<FaceDetectionResponse>> DetectFacesAsync(FaceDetectionRequest request);
        Task<Response<ContentModerationResponse>> DetectModerationLabelsAsync(ContentModerationRequest request);
        Task<Response<ImageAnalysisResponse>> DetectTextAsync(ImageAnalysisRequest request);
    }
}