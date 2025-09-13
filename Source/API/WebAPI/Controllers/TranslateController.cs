using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranslateController : ControllerBase
    {
        private readonly ITranslateService _translateService;
        private readonly ILogger<TranslateController> _logger;

        public TranslateController(ITranslateService translateService, ILogger<TranslateController> logger)
        {
            _translateService = translateService;
            _logger = logger;
        }

        [HttpPost("translate")]
        public async Task<ActionResult<Response<AWSDemo.AIServices.Models.TranslateTextResponse>>> TranslateText([FromBody] AWSDemo.AIServices.Models.TranslateTextRequest request)
        {
            try
            {
                var result = await _translateService.TranslateTextAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TranslateText");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("detect-language")]
        public async Task<ActionResult<Response<AWSDemo.AIServices.Models.DetectLanguageResponse>>> DetectLanguage([FromBody] AWSDemo.AIServices.Models.DetectLanguageRequest request)
        {
            try
            {
                var result = await _translateService.DetectDominantLanguageAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DetectLanguage");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("supported-languages")]
        public async Task<ActionResult<Response<List<string>>>> GetSupportedLanguages()
        {
            try
            {
                var result = await _translateService.GetSupportedLanguagesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSupportedLanguages");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}