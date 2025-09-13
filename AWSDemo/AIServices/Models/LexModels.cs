namespace AWSDemo.AIServices.Models
{
    public class ChatMessage
    {
        public string Message { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public Dictionary<string, string> SessionAttributes { get; set; } = new();
        public Dictionary<string, string> RequestAttributes { get; set; } = new();
    }

    public class ChatResponse
    {
        public string Message { get; set; } = string.Empty;
        public string DialogAction { get; set; } = string.Empty;
        public string IntentName { get; set; } = string.Empty;
        public Dictionary<string, string> SessionAttributes { get; set; } = new();
        public Dictionary<string, object> Slots { get; set; } = new();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class CreateSessionRequest
    {
        public string BotName { get; set; } = string.Empty;
        public string BotAlias { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string LocaleId { get; set; } = "en_US";
    }

    public class CreateSessionResponse
    {
        public string SessionId { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}