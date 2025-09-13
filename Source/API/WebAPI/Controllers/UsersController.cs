using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ICognitoUserService _cognitoUserService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(ICognitoUserService cognitoUserService, ILogger<UsersController> logger)
        {
            _cognitoUserService = cognitoUserService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new user in AWS Cognito
        /// </summary>
        /// <param name="request">User registration details</param>
        /// <returns>Success or error response</returns>
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _cognitoUserService.RegisterUserAsync(request);

                if (result)
                {
                    return Ok(new { message = "User registered successfully", username = request.Username });
                }

                return StatusCode(500, new { message = "Failed to register user" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RegisterUser");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get all users from AWS Cognito
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserResponse>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _cognitoUserService.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUsers");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get a specific user by username
        /// </summary>
        /// <param name="username">Username to retrieve</param>
        /// <returns>User details</returns>
        [HttpGet("{username}")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUser(string username)
        {
            try
            {
                var user = await _cognitoUserService.GetUserAsync(username);
                
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUser for username: {Username}", username);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Update user attributes in AWS Cognito
        /// </summary>
        /// <param name="username">Username to update</param>
        /// <param name="request">Updated user details</param>
        /// <returns>Success or error response</returns>
        [HttpPut("{username}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUser(string username, [FromBody] UpdateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Check if user exists
                var existingUser = await _cognitoUserService.GetUserAsync(username);
                if (existingUser == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var result = await _cognitoUserService.UpdateUserAsync(username, request);

                if (result)
                {
                    return Ok(new { message = "User updated successfully", username = username });
                }

                return StatusCode(500, new { message = "Failed to update user" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateUser for username: {Username}", username);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        /// <summary>
        /// Delete a user from AWS Cognito
        /// </summary>
        /// <param name="username">Username to delete</param>
        /// <returns>Success or error response</returns>
        [HttpDelete("{username}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser(string username)
        {
            try
            {
                // Check if user exists
                var existingUser = await _cognitoUserService.GetUserAsync(username);
                if (existingUser == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var result = await _cognitoUserService.DeleteUserAsync(username);

                if (result)
                {
                    return Ok(new { message = "User deleted successfully", username = username });
                }

                return StatusCode(500, new { message = "Failed to delete user" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteUser for username: {Username}", username);
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}