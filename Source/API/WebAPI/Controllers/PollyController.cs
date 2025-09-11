using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollyController : ControllerBase
    {
        private readonly IPollyService _pollyService;
        private readonly ILogger<PollyController> _logger;

        public PollyController(IPollyService pollyService, ILogger<PollyController> logger)
        {
            _pollyService = pollyService;
            _logger = logger;
        }

        [HttpPost("synthesize")]
        public async Task<ActionResult<Response<TextToSpeechResponse>>> SynthesizeSpeech([FromBody] TextToSpeechRequest request)
        {
            try
            {
                var result = await _pollyService.SynthesizeSpeechAsync(request);
                
                if (result.IsSuccess && result.Model?.Success == true)
                {
                    return File(result.Model.AudioData, result.Model.ContentType, "speech.mp3");
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SynthesizeSpeech");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("voices")]
        public async Task<ActionResult<Response<List<string>>>> GetVoices([FromQuery] string languageCode = "")
        {
            try
            {
                var result = await _pollyService.GetAvailableVoicesAsync(languageCode);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetVoices");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}