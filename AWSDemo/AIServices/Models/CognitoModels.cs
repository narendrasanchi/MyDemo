namespace AWSDemo.AIServices.Models
{
    public class UserRegistrationRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Dictionary<string, string> UserAttributes { get; set; } = new();
    }

    public class UserRegistrationResponse
    {
        public string UserSub { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public bool EmailVerificationRequired { get; set; }
        public bool PhoneVerificationRequired { get; set; }
    }

    public class UserLoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UserLoginResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string IdToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string ChallengeName { get; set; } = string.Empty;
        public string Session { get; set; } = string.Empty;
    }

    public class ConfirmSignUpRequest
    {
        public string Username { get; set; } = string.Empty;
        public string ConfirmationCode { get; set; } = string.Empty;
    }

    public class ConfirmSignUpResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class ForgotPasswordRequest
    {
        public string Username { get; set; } = string.Empty;
    }

    public class ForgotPasswordResponse
    {
        public string CodeDeliveryDetails { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class ConfirmForgotPasswordRequest
    {
        public string Username { get; set; } = string.Empty;
        public string ConfirmationCode { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class ConfirmForgotPasswordResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class RefreshTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string IdToken { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}