using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprehendController : ControllerBase
    {
        private readonly IComprehendService _comprehendService;
        private readonly ILogger<ComprehendController> _logger;

        public ComprehendController(IComprehendService comprehendService, ILogger<ComprehendController> logger)
        {
            _comprehendService = comprehendService;
            _logger = logger;
        }

        [HttpPost("sentiment")]
        public async Task<ActionResult<Response<SentimentAnalysisResponse>>> AnalyzeSentiment([FromBody] SentimentAnalysisRequest request)
        {
            try
            {
                var result = await _comprehendService.DetectSentimentAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AnalyzeSentiment");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("entities")]
        public async Task<ActionResult<Response<EntityDetectionResponse>>> DetectEntities([FromBody] EntityDetectionRequest request)
        {
            try
            {
                var result = await _comprehendService.DetectEntitiesAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DetectEntities");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("key-phrases")]
        public async Task<ActionResult<Response<KeyPhrasesResponse>>> DetectKeyPhrases([FromBody] KeyPhrasesRequest request)
        {
            try
            {
                var result = await _comprehendService.DetectKeyPhrasesAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DetectKeyPhrases");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("detect-language")]
        public async Task<ActionResult<Response<AWSDemo.AIServices.Models.DetectLanguageResponse>>> DetectLanguage([FromBody] AWSDemo.AIServices.Models.DetectLanguageRequest request)
        {
            try
            {
                var result = await _comprehendService.DetectDominantLanguageAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DetectLanguage");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}