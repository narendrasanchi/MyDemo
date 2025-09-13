using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BedrockController : ControllerBase
    {
        private readonly IBedrockService _bedrockService;
        private readonly ILogger<BedrockController> _logger;

        public BedrockController(IBedrockService bedrockService, ILogger<BedrockController> logger)
        {
            _bedrockService = bedrockService;
            _logger = logger;
        }

        [HttpPost("generate-text")]
        public async Task<ActionResult<Response<TextGenerationResponse>>> GenerateText([FromBody] TextGenerationRequest request)
        {
            try
            {
                var result = await _bedrockService.GenerateTextAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GenerateText");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("generate-image")]
        public async Task<ActionResult<Response<ImageGenerationResponse>>> GenerateImage([FromBody] ImageGenerationRequest request)
        {
            try
            {
                var result = await _bedrockService.GenerateImageAsync(request);
                
                if (result.IsSuccess && result.Model?.Success == true)
                {
                    return File(result.Model.ImageData, result.Model.ContentType, "generated-image.png");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GenerateImage");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("models")]
        public async Task<ActionResult<Response<ListModelsResponse>>> GetAvailableModels()
        {
            try
            {
                var result = await _bedrockService.GetAvailableModelsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAvailableModels");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}