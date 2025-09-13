namespace WebAPI.Models
{
    public class UserResponse
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
        public bool EmailVerified { get; set; }
        public bool PhoneVerified { get; set; }
        public string UserStatus { get; set; } = string.Empty;
        public DateTime? CreationDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}