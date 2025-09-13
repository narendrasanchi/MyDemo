using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Common.Enum;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AWSDemo.AIServices.Services
{
    public class TranscribeService : ITranscribeService
    {
        private readonly AmazonTranscribeServiceClient _transcribeClient;
        private readonly ILogger<TranscribeService> _logger;

        public TranscribeService(AmazonTranscribeServiceClient transcribeClient, ILogger<TranscribeService> logger)
        {
            _transcribeClient = transcribeClient;
            _logger = logger;
        }

        public async Task<Response<TranscribeResponse>> StartTranscriptionJobAsync(TranscribeRequest request)
        {
            try
            {
                var transcriptionRequest = new StartTranscriptionJobRequest
                {
                    TranscriptionJobName = request.JobName,
                    LanguageCode = LanguageCode.FindValue(request.LanguageCode),
                    Media = new Media
                    {
                        MediaFileUri = $"s3://{request.S3BucketName}/{request.S3ObjectKey}"
                    },
                    OutputBucketName = request.S3BucketName
                };

                var response = await _transcribeClient.StartTranscriptionJobAsync(transcriptionRequest);

                var result = new TranscribeResponse
                {
                    JobName = response.TranscriptionJob.TranscriptionJobName,
                    JobStatus = response.TranscriptionJob.TranscriptionJobStatus.Value,
                    Success = true
                };

                return new Response<TranscribeResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting transcription job");
                return new Response<TranscribeResponse>(
                    new TranscribeResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<TranscribeJobStatusResponse>> GetTranscriptionJobStatusAsync(string jobName)
        {
            try
            {
                var request = new GetTranscriptionJobRequest
                {
                    TranscriptionJobName = jobName
                };

                var response = await _transcribeClient.GetTranscriptionJobAsync(request);
                var job = response.TranscriptionJob;

                var result = new TranscribeJobStatusResponse
                {
                    JobName = job.TranscriptionJobName,
                    JobStatus = job.TranscriptionJobStatus.Value,
                    Success = true
                };

                if (job.TranscriptionJobStatus == TranscriptionJobStatus.COMPLETED && job.Transcript != null)
                {
                    var transcriptResponse = await GetTranscriptTextAsync(job.Transcript.TranscriptFileUri);
                    if (transcriptResponse.Code == ResponseCode.Ok)
                    {
                        result.TranscriptText = transcriptResponse.Model ?? string.Empty;
                    }
                }

                return new Response<TranscribeJobStatusResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transcription job status");
                return new Response<TranscribeJobStatusResponse>(
                    new TranscribeJobStatusResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<string>> GetTranscriptTextAsync(string transcriptUri)
        {
            try
            {
                using var httpClient = new HttpClient();
                var jsonContent = await httpClient.GetStringAsync(transcriptUri);
                
                using var jsonDoc = JsonDocument.Parse(jsonContent);
                var transcriptText = jsonDoc.RootElement
                    .GetProperty("results")
                    .GetProperty("transcripts")[0]
                    .GetProperty("transcript")
                    .GetString() ?? string.Empty;

                return new Response<string>(transcriptText, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting transcript text");
                return new Response<string>(string.Empty, ResponseCode.BadRequest);
            }
        }

        public async Task<Response<bool>> DeleteTranscriptionJobAsync(string jobName)
        {
            try
            {
                var request = new DeleteTranscriptionJobRequest
                {
                    TranscriptionJobName = jobName
                };

                await _transcribeClient.DeleteTranscriptionJobAsync(request);
                return new Response<bool>(true, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting transcription job");
                return new Response<bool>(false, ResponseCode.BadRequest);
            }
        }
    }
}