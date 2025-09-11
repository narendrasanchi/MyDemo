using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranscribeController : ControllerBase
    {
        private readonly ITranscribeService _transcribeService;
        private readonly ILogger<TranscribeController> _logger;

        public TranscribeController(ITranscribeService transcribeService, ILogger<TranscribeController> logger)
        {
            _transcribeService = transcribeService;
            _logger = logger;
        }

        [HttpPost("start-job")]
        public async Task<ActionResult<Response<TranscribeResponse>>> StartTranscriptionJob([FromBody] TranscribeRequest request)
        {
            try
            {
                var result = await _transcribeService.StartTranscriptionJobAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StartTranscriptionJob");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("job-status/{jobName}")]
        public async Task<ActionResult<Response<TranscribeJobStatusResponse>>> GetJobStatus(string jobName)
        {
            try
            {
                var result = await _transcribeService.GetTranscriptionJobStatusAsync(jobName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetJobStatus");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("transcript")]
        public async Task<ActionResult<Response<string>>> GetTranscript([FromQuery] string transcriptUri)
        {
            try
            {
                var result = await _transcribeService.GetTranscriptTextAsync(transcriptUri);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTranscript");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("job/{jobName}")]
        public async Task<ActionResult<Response<bool>>> DeleteJob(string jobName)
        {
            try
            {
                var result = await _transcribeService.DeleteTranscriptionJobAsync(jobName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteJob");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}