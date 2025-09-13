using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class UpdateUserRequest
    {
        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
    }
}