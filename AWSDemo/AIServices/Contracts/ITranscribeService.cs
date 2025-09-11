using AWSDemo.AIServices.Models;
using Common;

namespace AWSDemo.AIServices.Contracts
{
    public interface ITranscribeService
    {
        Task<Response<TranscribeResponse>> StartTranscriptionJobAsync(TranscribeRequest request);
        Task<Response<TranscribeJobStatusResponse>> GetTranscriptionJobStatusAsync(string jobName);
        Task<Response<string>> GetTranscriptTextAsync(string transcriptUri);
        Task<Response<bool>> DeleteTranscriptionJobAsync(string jobName);
    }
}