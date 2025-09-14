using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class RegisterUserRequest
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? PhoneNumber { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
    }
}