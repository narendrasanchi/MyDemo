namespace WebAPI.Models
{
    public class AwsSettings
    {
        public string Region { get; set; } = string.Empty;
        public CognitoSettings Cognito { get; set; } = new CognitoSettings();
    }

    public class CognitoSettings
    {
        public string UserPoolId { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string? ClientSecret { get; set; }
    }
}