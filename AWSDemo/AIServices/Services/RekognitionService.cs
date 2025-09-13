using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Common.Enum;
using Microsoft.Extensions.Logging;

namespace AWSDemo.AIServices.Services
{
    public class RekognitionService : IRekognitionService
    {
        private readonly AmazonRekognitionClient _rekognitionClient;
        private readonly ILogger<RekognitionService> _logger;

        public RekognitionService(AmazonRekognitionClient rekognitionClient, ILogger<RekognitionService> logger)
        {
            _rekognitionClient = rekognitionClient;
            _logger = logger;
        }

        public async Task<Response<ImageAnalysisResponse>> DetectLabelsAsync(ImageAnalysisRequest request)
        {
            try
            {
                var detectRequest = new DetectLabelsRequest
                {
                    MaxLabels = request.MaxLabels,
                    MinConfidence = request.MinConfidence
                };

                // Set image source (either S3 or bytes)
                if (!string.IsNullOrEmpty(request.S3BucketName) && !string.IsNullOrEmpty(request.S3ObjectKey))
                {
                    detectRequest.Image = new Amazon.Rekognition.Model.Image
                    {
                        S3Object = new S3Object
                        {
                            Bucket = request.S3BucketName,
                            Name = request.S3ObjectKey
                        }
                    };
                }
                else if (request.ImageData.Length > 0)
                {
                    detectRequest.Image = new Amazon.Rekognition.Model.Image
                    {
                        Bytes = new MemoryStream(request.ImageData)
                    };
                }

                var response = await _rekognitionClient.DetectLabelsAsync(detectRequest);

                var labels = response.Labels.Select(l => new DetectedLabel
                {
                    Name = l.Name,
                    Confidence = l.Confidence,
                    Categories = l.Categories?.Select(c => c.Name).ToList() ?? new List<string>()
                }).ToList();

                var result = new ImageAnalysisResponse
                {
                    Labels = labels,
                    Success = true
                };

                return new Response<ImageAnalysisResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting labels");
                return new Response<ImageAnalysisResponse>(
                    new ImageAnalysisResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<FaceDetectionResponse>> DetectFacesAsync(FaceDetectionRequest request)
        {
            try
            {
                var detectRequest = new DetectFacesRequest
                {
                    Attributes = new List<string> { "ALL" }
                };

                // Set image source
                if (!string.IsNullOrEmpty(request.S3BucketName) && !string.IsNullOrEmpty(request.S3ObjectKey))
                {
                    detectRequest.Image = new Amazon.Rekognition.Model.Image
                    {
                        S3Object = new S3Object
                        {
                            Bucket = request.S3BucketName,
                            Name = request.S3ObjectKey
                        }
                    };
                }
                else if (request.ImageData.Length > 0)
                {
                    detectRequest.Image = new Amazon.Rekognition.Model.Image
                    {
                        Bytes = new MemoryStream(request.ImageData)
                    };
                }

                var response = await _rekognitionClient.DetectFacesAsync(detectRequest);

                var faces = response.FaceDetails.Select(f => new DetectedFace
                {
                    BoundingBox = new Models.BoundingBox
                    {
                        Width = f.BoundingBox.Width,
                        Height = f.BoundingBox.Height,
                        Left = f.BoundingBox.Left,
                        Top = f.BoundingBox.Top
                    },
                    Landmarks = f.Landmarks?.Select(l => new Models.Landmark
                    {
                        Type = l.Type.Value,
                        X = l.X,
                        Y = l.Y
                    }).ToList() ?? new List<Models.Landmark>(),
                    AgeRangeLow = f.AgeRange?.Low ?? 0,
                    AgeRangeHigh = f.AgeRange?.High ?? 0,
                    Gender = f.Gender?.Value ?? string.Empty,
                    Emotions = f.Emotions?.Select(e => new Models.Emotion
                    {
                        Type = e.Type.Value,
                        Confidence = e.Confidence
                    }).ToList() ?? new List<Models.Emotion>()
                }).ToList();

                var result = new FaceDetectionResponse
                {
                    Faces = faces,
                    Success = true
                };

                return new Response<FaceDetectionResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting faces");
                return new Response<FaceDetectionResponse>(
                    new FaceDetectionResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<ContentModerationResponse>> DetectModerationLabelsAsync(ContentModerationRequest request)
        {
            try
            {
                var detectRequest = new DetectModerationLabelsRequest
                {
                    MinConfidence = request.MinConfidence
                };

                // Set image source
                if (!string.IsNullOrEmpty(request.S3BucketName) && !string.IsNullOrEmpty(request.S3ObjectKey))
                {
                    detectRequest.Image = new Amazon.Rekognition.Model.Image
                    {
                        S3Object = new S3Object
                        {
                            Bucket = request.S3BucketName,
                            Name = request.S3ObjectKey
                        }
                    };
                }
                else if (request.ImageData.Length > 0)
                {
                    detectRequest.Image = new Amazon.Rekognition.Model.Image
                    {
                        Bytes = new MemoryStream(request.ImageData)
                    };
                }

                var response = await _rekognitionClient.DetectModerationLabelsAsync(detectRequest);

                var moderationLabels = response.ModerationLabels.Select(ml => new Models.ModerationLabel
                {
                    Name = ml.Name,
                    Confidence = ml.Confidence,
                    ParentName = ml.ParentName ?? string.Empty
                }).ToList();

                var result = new ContentModerationResponse
                {
                    ModerationLabels = moderationLabels,
                    Success = true
                };

                return new Response<ContentModerationResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting moderation labels");
                return new Response<ContentModerationResponse>(
                    new ContentModerationResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<ImageAnalysisResponse>> DetectTextAsync(ImageAnalysisRequest request)
        {
            try
            {
                var detectRequest = new DetectTextRequest();

                // Set image source
                if (!string.IsNullOrEmpty(request.S3BucketName) && !string.IsNullOrEmpty(request.S3ObjectKey))
                {
                    detectRequest.Image = new Amazon.Rekognition.Model.Image
                    {
                        S3Object = new S3Object
                        {
                            Bucket = request.S3BucketName,
                            Name = request.S3ObjectKey
                        }
                    };
                }
                else if (request.ImageData.Length > 0)
                {
                    detectRequest.Image = new Amazon.Rekognition.Model.Image
                    {
                        Bytes = new MemoryStream(request.ImageData)
                    };
                }

                var response = await _rekognitionClient.DetectTextAsync(detectRequest);

                var textDetections = response.TextDetections
                    .Where(td => td.Confidence >= request.MinConfidence)
                    .Select(td => new DetectedLabel
                    {
                        Name = td.DetectedText,
                        Confidence = td.Confidence,
                        Categories = new List<string> { td.Type.Value }
                    }).ToList();

                var result = new ImageAnalysisResponse
                {
                    Labels = textDetections,
                    Success = true
                };

                return new Response<ImageAnalysisResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error detecting text");
                return new Response<ImageAnalysisResponse>(
                    new ImageAnalysisResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }
    }
}