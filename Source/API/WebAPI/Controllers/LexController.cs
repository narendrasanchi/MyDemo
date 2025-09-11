using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LexController : ControllerBase
    {
        private readonly ILexService _lexService;
        private readonly ILogger<LexController> _logger;

        public LexController(ILexService lexService, ILogger<LexController> logger)
        {
            _lexService = lexService;
            _logger = logger;
        }

        [HttpPost("send-message")]
        public async Task<ActionResult<Response<ChatResponse>>> SendMessage([FromBody] ChatMessage message)
        {
            try
            {
                var result = await _lexService.SendMessageAsync(message);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendMessage");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("create-session")]
        public async Task<ActionResult<Response<CreateSessionResponse>>> CreateSession([FromBody] CreateSessionRequest request)
        {
            try
            {
                var result = await _lexService.CreateSessionAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateSession");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("session/{sessionId}")]
        public async Task<ActionResult<Response<bool>>> DeleteSession(string sessionId)
        {
            try
            {
                var result = await _lexService.DeleteSessionAsync(sessionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteSession");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}