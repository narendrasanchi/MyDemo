using Amazon.LexRuntimeV2;
using Amazon.LexRuntimeV2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LexController : ControllerBase
{
    private readonly IAmazonLexRuntimeV2 _lexClient;
    private readonly ILogger<LexController> _logger;
    private readonly IConfiguration _configuration;

    public LexController(IAmazonLexRuntimeV2 lexClient, ILogger<LexController> logger, IConfiguration configuration)
    {
        _lexClient = lexClient;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost("recognize-text")]
    public async Task<IActionResult> RecognizeText([FromBody] LexChatRequest request)
    {
        try
        {
            // For demo purposes, we'll create a simple rule-based chatbot
            // In a real implementation, you would configure AWS Lex bot
            var botResponse = await SimulateLexBot(request.Message, request.SessionId);
            
            return Ok(botResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing chat message: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("recognize-speech")]
    public async Task<IActionResult> RecognizeSpeech(IFormFile audioFile, [FromBody] SpeechChatRequest request)
    {
        try
        {
            if (audioFile == null || audioFile.Length == 0)
            {
                return BadRequest("No audio file uploaded.");
            }

            // For demo purposes, we'll simulate speech recognition + bot response
            var transcribedText = await SimulateAudioTranscription(audioFile);
            var botResponse = await SimulateLexBot(transcribedText, request.SessionId);
            
            botResponse.TranscribedText = transcribedText;
            
            return Ok(botResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing speech message: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("start-conversation")]
    public IActionResult StartConversation()
    {
        var sessionId = Guid.NewGuid().ToString();
        
        return Ok(new ConversationResponse
        {
            SessionId = sessionId,
            Message = "Hello! I'm your AI assistant. How can I help you today?",
            Intent = "Welcome",
            DialogState = "ElicitIntent",
            Slots = new Dictionary<string, string>(),
            SessionAttributes = new Dictionary<string, string>()
        });
    }

    [HttpGet("session/{sessionId}/history")]
    public IActionResult GetConversationHistory(string sessionId)
    {
        // For demo purposes, return mock conversation history
        // In a real implementation, you would store and retrieve conversation history
        
        var mockHistory = new List<ConversationTurn>
        {
            new() 
            { 
                Timestamp = DateTime.UtcNow.AddMinutes(-10),
                UserMessage = "Hello",
                BotResponse = "Hello! I'm your AI assistant. How can I help you today?",
                Intent = "Welcome"
            },
            new() 
            { 
                Timestamp = DateTime.UtcNow.AddMinutes(-8),
                UserMessage = "What's the weather like?",
                BotResponse = "I'd be happy to help with weather information. Which city would you like to know about?",
                Intent = "GetWeather"
            }
        };

        return Ok(new ConversationHistoryResponse
        {
            SessionId = sessionId,
            ConversationHistory = mockHistory,
            TotalTurns = mockHistory.Count
        });
    }

    private async Task<ConversationResponse> SimulateLexBot(string message, string sessionId)
    {
        // Simulate processing time
        await Task.Delay(500);

        var lowerMessage = message.ToLowerInvariant();
        
        // Simple rule-based responses for demo
        return lowerMessage switch
        {
            var msg when msg.Contains("hello") || msg.Contains("hi") || msg.Contains("hey") =>
                new ConversationResponse
                {
                    SessionId = sessionId,
                    Message = "Hello! I'm your AI assistant. How can I help you today?",
                    Intent = "Greeting",
                    DialogState = "ElicitIntent",
                    Slots = new Dictionary<string, string>(),
                    SessionAttributes = new Dictionary<string, string>()
                },

            var msg when msg.Contains("weather") =>
                new ConversationResponse
                {
                    SessionId = sessionId,
                    Message = "I'd be happy to help with weather information. Which city would you like to know about?",
                    Intent = "GetWeather",
                    DialogState = "ElicitSlot",
                    Slots = new Dictionary<string, string> { { "City", "" } },
                    SessionAttributes = new Dictionary<string, string>()
                },

            var msg when msg.Contains("book") && msg.Contains("appointment") =>
                new ConversationResponse
                {
                    SessionId = sessionId,
                    Message = "I can help you book an appointment. What date would you prefer?",
                    Intent = "BookAppointment",
                    DialogState = "ElicitSlot",
                    Slots = new Dictionary<string, string> { { "Date", "" }, { "Time", "" } },
                    SessionAttributes = new Dictionary<string, string>()
                },

            var msg when msg.Contains("order") && msg.Contains("food") =>
                new ConversationResponse
                {
                    SessionId = sessionId,
                    Message = "I can help you order food. What would you like to eat today?",
                    Intent = "OrderFood",
                    DialogState = "ElicitSlot",
                    Slots = new Dictionary<string, string> { { "FoodItem", "" }, { "Quantity", "" } },
                    SessionAttributes = new Dictionary<string, string>()
                },

            var msg when msg.Contains("help") =>
                new ConversationResponse
                {
                    SessionId = sessionId,
                    Message = "I can help you with:\n- Weather information\n- Booking appointments\n- Ordering food\n- General questions\n\nWhat would you like to do?",
                    Intent = "Help",
                    DialogState = "ElicitIntent",
                    Slots = new Dictionary<string, string>(),
                    SessionAttributes = new Dictionary<string, string>()
                },

            var msg when msg.Contains("thank") =>
                new ConversationResponse
                {
                    SessionId = sessionId,
                    Message = "You're welcome! Is there anything else I can help you with?",
                    Intent = "Thanks",
                    DialogState = "ElicitIntent",
                    Slots = new Dictionary<string, string>(),
                    SessionAttributes = new Dictionary<string, string>()
                },

            var msg when msg.Contains("bye") || msg.Contains("goodbye") =>
                new ConversationResponse
                {
                    SessionId = sessionId,
                    Message = "Goodbye! Have a great day!",
                    Intent = "Goodbye",
                    DialogState = "Fulfilled",
                    Slots = new Dictionary<string, string>(),
                    SessionAttributes = new Dictionary<string, string>()
                },

            _ => new ConversationResponse
            {
                SessionId = sessionId,
                Message = "I'm not sure I understand. Could you please rephrase that? You can ask me about weather, appointments, food orders, or say 'help' for more options.",
                Intent = "Fallback",
                DialogState = "Failed",
                Slots = new Dictionary<string, string>(),
                SessionAttributes = new Dictionary<string, string>()
            }
        };
    }

    private async Task<string> SimulateAudioTranscription(IFormFile audioFile)
    {
        // Simulate audio transcription processing
        await Task.Delay(1000);
        
        // Return sample transcriptions based on common queries
        var sampleTranscriptions = new[]
        {
            "Hello, how are you today?",
            "What's the weather like?",
            "Can you help me book an appointment?",
            "I'd like to order some food",
            "Thank you for your help",
            "Can you help me with something?"
        };

        var random = new Random();
        return sampleTranscriptions[random.Next(sampleTranscriptions.Length)];
    }
}

public class LexChatRequest
{
    [Required]
    [StringLength(1000, MinimumLength = 1)]
    public string Message { get; set; } = string.Empty;

    [Required]
    public string SessionId { get; set; } = string.Empty;

    public Dictionary<string, string> SessionAttributes { get; set; } = new();
}

public class SpeechChatRequest
{
    [Required]
    public string SessionId { get; set; } = string.Empty;

    public Dictionary<string, string> SessionAttributes { get; set; } = new();
}

public class ConversationResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Intent { get; set; } = string.Empty;
    public string DialogState { get; set; } = string.Empty;
    public Dictionary<string, string> Slots { get; set; } = new();
    public Dictionary<string, string> SessionAttributes { get; set; } = new();
    public string? TranscribedText { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ConversationTurn
{
    public DateTime Timestamp { get; set; }
    public string UserMessage { get; set; } = string.Empty;
    public string BotResponse { get; set; } = string.Empty;
    public string Intent { get; set; } = string.Empty;
}

public class ConversationHistoryResponse
{
    public string SessionId { get; set; } = string.Empty;
    public List<ConversationTurn> ConversationHistory { get; set; } = new();
    public int TotalTurns { get; set; }
}