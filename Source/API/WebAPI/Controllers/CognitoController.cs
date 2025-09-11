using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CognitoController : ControllerBase
    {
        private readonly ICognitoService _cognitoService;
        private readonly ILogger<CognitoController> _logger;

        public CognitoController(ICognitoService cognitoService, ILogger<CognitoController> logger)
        {
            _cognitoService = cognitoService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response<AWSDemo.AIServices.Models.UserRegistrationResponse>>> Register([FromBody] AWSDemo.AIServices.Models.UserRegistrationRequest request)
        {
            try
            {
                var result = await _cognitoService.RegisterUserAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Register");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("confirm-signup")]
        public async Task<ActionResult<Response<AWSDemo.AIServices.Models.ConfirmSignUpResponse>>> ConfirmSignUp([FromBody] AWSDemo.AIServices.Models.ConfirmSignUpRequest request)
        {
            try
            {
                var result = await _cognitoService.ConfirmSignUpAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ConfirmSignUp");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<AWSDemo.AIServices.Models.UserLoginResponse>>> Login([FromBody] AWSDemo.AIServices.Models.UserLoginRequest request)
        {
            try
            {
                var result = await _cognitoService.LoginAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Login");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<Response<AWSDemo.AIServices.Models.ForgotPasswordResponse>>> ForgotPassword([FromBody] AWSDemo.AIServices.Models.ForgotPasswordRequest request)
        {
            try
            {
                var result = await _cognitoService.ForgotPasswordAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ForgotPassword");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("confirm-forgot-password")]
        public async Task<ActionResult<Response<AWSDemo.AIServices.Models.ConfirmForgotPasswordResponse>>> ConfirmForgotPassword([FromBody] AWSDemo.AIServices.Models.ConfirmForgotPasswordRequest request)
        {
            try
            {
                var result = await _cognitoService.ConfirmForgotPasswordAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ConfirmForgotPassword");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<Response<AWSDemo.AIServices.Models.RefreshTokenResponse>>> RefreshToken([FromBody] AWSDemo.AIServices.Models.RefreshTokenRequest request)
        {
            try
            {
                var result = await _cognitoService.RefreshTokenAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RefreshToken");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("signout")]
        public async Task<ActionResult<Response<bool>>> SignOut([FromBody] string accessToken)
        {
            try
            {
                var result = await _cognitoService.SignOutAsync(accessToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SignOut");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}