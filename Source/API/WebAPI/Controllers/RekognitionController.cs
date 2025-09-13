using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RekognitionController : ControllerBase
    {
        private readonly IRekognitionService _rekognitionService;
        private readonly ILogger<RekognitionController> _logger;

        public RekognitionController(IRekognitionService rekognitionService, ILogger<RekognitionController> logger)
        {
            _rekognitionService = rekognitionService;
            _logger = logger;
        }

        [HttpPost("detect-labels")]
        public async Task<ActionResult<Response<ImageAnalysisResponse>>> DetectLabels([FromForm] IFormFile imageFile, [FromQuery] int maxLabels = 10, [FromQuery] float minConfidence = 50.0f)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    return BadRequest("No image file provided");
                }

                using var memoryStream = new MemoryStream();
                await imageFile.CopyToAsync(memoryStream);

                var request = new ImageAnalysisRequest
                {
                    ImageData = memoryStream.ToArray(),
                    MaxLabels = maxLabels,
                    MinConfidence = minConfidence
                };

                var result = await _rekognitionService.DetectLabelsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DetectLabels");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("detect-faces")]
        public async Task<ActionResult<Response<FaceDetectionResponse>>> DetectFaces([FromForm] IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    return BadRequest("No image file provided");
                }

                using var memoryStream = new MemoryStream();
                await imageFile.CopyToAsync(memoryStream);

                var request = new FaceDetectionRequest
                {
                    ImageData = memoryStream.ToArray()
                };

                var result = await _rekognitionService.DetectFacesAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DetectFaces");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("detect-text")]
        public async Task<ActionResult<Response<ImageAnalysisResponse>>> DetectText([FromForm] IFormFile imageFile, [FromQuery] float minConfidence = 50.0f)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    return BadRequest("No image file provided");
                }

                using var memoryStream = new MemoryStream();
                await imageFile.CopyToAsync(memoryStream);

                var request = new ImageAnalysisRequest
                {
                    ImageData = memoryStream.ToArray(),
                    MinConfidence = minConfidence
                };

                var result = await _rekognitionService.DetectTextAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DetectText");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("moderation")]
        public async Task<ActionResult<Response<ContentModerationResponse>>> DetectModerationLabels([FromForm] IFormFile imageFile, [FromQuery] float minConfidence = 50.0f)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    return BadRequest("No image file provided");
                }

                using var memoryStream = new MemoryStream();
                await imageFile.CopyToAsync(memoryStream);

                var request = new ContentModerationRequest
                {
                    ImageData = memoryStream.ToArray(),
                    MinConfidence = minConfidence
                };

                var result = await _rekognitionService.DetectModerationLabelsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DetectModerationLabels");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}