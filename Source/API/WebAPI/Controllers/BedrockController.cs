using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BedrockController : ControllerBase
{
    private readonly IAmazonBedrockRuntime _bedrockClient;
    private readonly ILogger<BedrockController> _logger;

    public BedrockController(IAmazonBedrockRuntime bedrockClient, ILogger<BedrockController> logger)
    {
        _bedrockClient = bedrockClient;
        _logger = logger;
    }

    [HttpPost("generate-text")]
    public async Task<IActionResult> GenerateText([FromBody] TextGenerationRequest request)
    {
        try
        {
            // For demo purposes, simulate text generation since Bedrock requires specific setup
            var response = await SimulateTextGeneration(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating text: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("generate-claude")]
    public async Task<IActionResult> GenerateWithClaude([FromBody] ClaudeGenerationRequest request)
    {
        try
        {
            // Simulate Claude API call for demo
            var response = await SimulateClaudeGeneration(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating text with Claude: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("generate-titan")]
    public async Task<IActionResult> GenerateWithTitan([FromBody] TitanGenerationRequest request)
    {
        try
        {
            // Simulate Titan API call for demo
            var response = await SimulateTitanGeneration(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating text with Titan: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("chat")]
    public async Task<IActionResult> ChatCompletion([FromBody] ChatRequest request)
    {
        try
        {
            var response = await SimulateChatCompletion(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in chat completion: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("models")]
    public IActionResult GetAvailableModels()
    {
        var models = new List<ModelInfo>
        {
            new() 
            { 
                ModelId = "anthropic.claude-3-sonnet-20240229-v1:0",
                Provider = "Anthropic",
                Name = "Claude 3 Sonnet",
                Description = "Claude 3 Sonnet is a powerful AI assistant created by Anthropic",
                InputModalities = new[] { "TEXT" },
                OutputModalities = new[] { "TEXT" },
                MaxTokens = 200000
            },
            new() 
            { 
                ModelId = "anthropic.claude-3-haiku-20240307-v1:0",
                Provider = "Anthropic",
                Name = "Claude 3 Haiku",
                Description = "Claude 3 Haiku is Anthropic's fastest, most compact model",
                InputModalities = new[] { "TEXT" },
                OutputModalities = new[] { "TEXT" },
                MaxTokens = 200000
            },
            new() 
            { 
                ModelId = "amazon.titan-text-express-v1",
                Provider = "Amazon",
                Name = "Titan Text Express",
                Description = "Amazon's text generation model optimized for various text tasks",
                InputModalities = new[] { "TEXT" },
                OutputModalities = new[] { "TEXT" },
                MaxTokens = 8000
            },
            new() 
            { 
                ModelId = "amazon.titan-text-lite-v1",
                Provider = "Amazon",
                Name = "Titan Text Lite",
                Description = "Amazon's lightweight text generation model",
                InputModalities = new[] { "TEXT" },
                OutputModalities = new[] { "TEXT" },
                MaxTokens = 4000
            },
            new() 
            { 
                ModelId = "ai21.j2-ultra-v1",
                Provider = "AI21 Labs",
                Name = "Jurassic-2 Ultra",
                Description = "AI21's flagship language model",
                InputModalities = new[] { "TEXT" },
                OutputModalities = new[] { "TEXT" },
                MaxTokens = 8191
            },
            new() 
            { 
                ModelId = "cohere.command-text-v14",
                Provider = "Cohere",
                Name = "Command",
                Description = "Cohere's generative language model optimized for business use cases",
                InputModalities = new[] { "TEXT" },
                OutputModalities = new[] { "TEXT" },
                MaxTokens = 4096
            }
        };

        return Ok(new AvailableModelsResponse { Models = models });
    }

    [HttpPost("embeddings")]
    public async Task<IActionResult> GenerateEmbeddings([FromBody] EmbeddingRequest request)
    {
        try
        {
            // Simulate embeddings generation
            var response = await SimulateEmbeddingsGeneration(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating embeddings: {Message}", ex.Message);
            return BadRequest(new { Error = ex.Message });
        }
    }

    private async Task<TextGenerationResponse> SimulateTextGeneration(TextGenerationRequest request)
    {
        await Task.Delay(1500); // Simulate processing time

        var responses = new Dictionary<string, string[]>
        {
            ["story"] = new[] 
            { 
                "Once upon a time, in a digital realm where algorithms danced and data flowed like rivers, there lived a small AI model named Spark. Spark had always dreamed of helping humans solve complex problems and create beautiful things.",
                "In the year 2045, humanity had learned to live in harmony with artificial intelligence. The story begins on a Tuesday morning when Dr. Sarah Chen discovered an unusual pattern in the quantum neural networks.",
                "The old lighthouse keeper had seen many storms, but nothing quite like the technological tempest that was approaching. As he polished the beacon's lens, he wondered how the world would look when the dust settled."
            },
            ["explain"] = new[] 
            { 
                "Let me break this down for you in simple terms. The concept you're asking about involves multiple interconnected components that work together to create a comprehensive system.",
                "To explain this effectively, I'll start with the fundamentals and build up to the more complex aspects. Think of it as constructing a building - we need a solid foundation before adding the upper floors.",
                "This is a fascinating topic that touches on several important principles. The key to understanding it lies in recognizing the patterns and relationships between different elements."
            },
            ["code"] = new[] 
            { 
                "Here's a clean and efficient approach to solve this problem:\n\n```python\ndef solution(input_data):\n    # Process the input data\n    result = []\n    for item in input_data:\n        processed_item = transform(item)\n        result.append(processed_item)\n    return result\n```",
                "I'll provide you with a robust implementation that follows best practices:\n\n```javascript\nclass DataProcessor {\n    constructor(options) {\n        this.options = options;\n    }\n    \n    process(data) {\n        return data.map(item => this.transform(item));\n    }\n}```",
                "Let me show you a solution that balances performance and readability:\n\n```csharp\npublic class DataHandler\n{\n    public async Task<List<T>> ProcessAsync<T>(IEnumerable<T> data)\n    {\n        var tasks = data.Select(item => ProcessItemAsync(item));\n        return (await Task.WhenAll(tasks)).ToList();\n    }\n}```"
            }
        };

        var category = DetermineCategory(request.Prompt);
        var possibleResponses = responses.ContainsKey(category) ? responses[category] : new[] 
        {
            "Thank you for your question. Based on your prompt, I can provide insights and generate relevant content that addresses your specific needs and requirements.",
            "I understand what you're looking for. Let me provide a comprehensive response that covers the key aspects of your inquiry and offers practical solutions.",
            "That's an interesting prompt. I'll generate content that is both informative and engaging, tailored to meet your specific requirements and context."
        };

        var random = new Random();
        var selectedResponse = possibleResponses[random.Next(possibleResponses.Length)];

        return new TextGenerationResponse
        {
            GeneratedText = selectedResponse,
            ModelId = request.ModelId,
            TokensUsed = selectedResponse.Length / 4, // Rough estimate
            FinishReason = "stop",
            Metadata = new Dictionary<string, object>
            {
                ["temperature"] = request.Temperature,
                ["max_tokens"] = request.MaxTokens,
                ["processing_time_ms"] = 1500
            }
        };
    }

    private async Task<ClaudeGenerationResponse> SimulateClaudeGeneration(ClaudeGenerationRequest request)
    {
        await Task.Delay(1200);

        var responses = new[]
        {
            "I'd be happy to help you with that. As Claude, I aim to provide thoughtful, accurate, and helpful responses while being honest about my limitations.",
            "That's a great question! Let me think through this carefully and provide you with a comprehensive answer that addresses your specific needs.",
            "I appreciate you asking me about this topic. Based on my understanding, I can offer several perspectives and practical suggestions."
        };

        var random = new Random();
        var response = responses[random.Next(responses.Length)];

        return new ClaudeGenerationResponse
        {
            GeneratedText = response,
            ModelId = "anthropic.claude-3-sonnet-20240229-v1:0",
            TokensUsed = response.Length / 4,
            FinishReason = "end_turn",
            Usage = new UsageInfo
            {
                InputTokens = request.Messages.Sum(m => m.Content.Length / 4),
                OutputTokens = response.Length / 4
            }
        };
    }

    private async Task<TitanGenerationResponse> SimulateTitanGeneration(TitanGenerationRequest request)
    {
        await Task.Delay(1000);

        var response = "Amazon Titan provides reliable and efficient text generation capabilities. This response demonstrates the model's ability to understand context and generate relevant content based on your specific requirements.";

        return new TitanGenerationResponse
        {
            GeneratedText = response,
            ModelId = "amazon.titan-text-express-v1",
            TokensUsed = response.Length / 4,
            FinishReason = "FINISH",
            InputTextTokenCount = request.InputText.Length / 4,
            Results = new List<TitanResult>
            {
                new() { TokenCount = response.Length / 4, OutputText = response, CompletionReason = "FINISH" }
            }
        };
    }

    private async Task<ChatCompletionResponse> SimulateChatCompletion(ChatRequest request)
    {
        await Task.Delay(1000);

        var lastMessage = request.Messages.LastOrDefault();
        var userMessage = lastMessage?.Content ?? "";

        var response = GenerateContextualResponse(userMessage, request.Messages);

        return new ChatCompletionResponse
        {
            Id = Guid.NewGuid().ToString(),
            Object = "chat.completion",
            Created = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            Model = request.Model,
            Choices = new List<ChatChoice>
            {
                new()
                {
                    Index = 0,
                    Message = new ChatMessage
                    {
                        Role = "assistant",
                        Content = response
                    },
                    FinishReason = "stop"
                }
            },
            Usage = new UsageInfo
            {
                InputTokens = request.Messages.Sum(m => m.Content.Length / 4),
                OutputTokens = response.Length / 4
            }
        };
    }

    private async Task<EmbeddingResponse> SimulateEmbeddingsGeneration(EmbeddingRequest request)
    {
        await Task.Delay(500);

        var embeddings = request.Inputs.Select(input => 
        {
            // Generate mock embeddings (normally would be from the model)
            var random = new Random(input.GetHashCode()); // Deterministic based on input
            var embedding = new float[1024]; // Common embedding dimension
            for (int i = 0; i < embedding.Length; i++)
            {
                embedding[i] = (float)(random.NextDouble() * 2 - 1); // Values between -1 and 1
            }
            return embedding;
        }).ToList();

        return new EmbeddingResponse
        {
            Embeddings = embeddings,
            ModelId = request.ModelId,
            InputTokenCount = request.Inputs.Sum(i => i.Length / 4)
        };
    }

    private string DetermineCategory(string prompt)
    {
        var lower = prompt.ToLowerInvariant();
        
        if (lower.Contains("story") || lower.Contains("tale") || lower.Contains("narrative"))
            return "story";
        
        if (lower.Contains("explain") || lower.Contains("how") || lower.Contains("what") || lower.Contains("why"))
            return "explain";
        
        if (lower.Contains("code") || lower.Contains("program") || lower.Contains("function") || lower.Contains("algorithm"))
            return "code";
        
        return "general";
    }

    private string GenerateContextualResponse(string userMessage, List<ChatMessage> conversationHistory)
    {
        var lower = userMessage.ToLowerInvariant();
        
        if (lower.Contains("hello") || lower.Contains("hi"))
            return "Hello! I'm an AI assistant powered by AWS Bedrock. How can I help you today?";
        
        if (lower.Contains("help"))
            return "I'm here to help! I can assist with various tasks including answering questions, writing content, explaining concepts, helping with code, and having general conversations. What would you like to work on?";
        
        if (lower.Contains("code") || lower.Contains("programming"))
            return "I'd be happy to help with coding! I can assist with writing code, debugging, explaining programming concepts, reviewing code, and suggesting best practices. What programming task are you working on?";
        
        return "I understand your message and I'm ready to help. Could you provide more details about what you'd like me to assist you with? I can help with a wide range of tasks and questions.";
    }
}

// Request Models
public class TextGenerationRequest
{
    [Required]
    public string Prompt { get; set; } = string.Empty;
    public string ModelId { get; set; } = "amazon.titan-text-express-v1";
    public int MaxTokens { get; set; } = 1000;
    public double Temperature { get; set; } = 0.7;
    public double TopP { get; set; } = 0.9;
}

public class ClaudeGenerationRequest
{
    [Required]
    public List<ChatMessage> Messages { get; set; } = new();
    public int MaxTokens { get; set; } = 1000;
    public double Temperature { get; set; } = 0.7;
}

public class TitanGenerationRequest
{
    [Required]
    public string InputText { get; set; } = string.Empty;
    public int MaxTokenCount { get; set; } = 1000;
    public double Temperature { get; set; } = 0.7;
    public double TopP { get; set; } = 0.9;
}

public class ChatRequest
{
    [Required]
    public List<ChatMessage> Messages { get; set; } = new();
    public string Model { get; set; } = "anthropic.claude-3-sonnet-20240229-v1:0";
    public int MaxTokens { get; set; } = 1000;
    public double Temperature { get; set; } = 0.7;
}

public class EmbeddingRequest
{
    [Required]
    public List<string> Inputs { get; set; } = new();
    public string ModelId { get; set; } = "amazon.titan-embed-text-v1";
}

// Response Models
public class TextGenerationResponse
{
    public string GeneratedText { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public int TokensUsed { get; set; }
    public string FinishReason { get; set; } = string.Empty;
    public Dictionary<string, object> Metadata { get; set; } = new();
}

public class ClaudeGenerationResponse
{
    public string GeneratedText { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public int TokensUsed { get; set; }
    public string FinishReason { get; set; } = string.Empty;
    public UsageInfo Usage { get; set; } = new();
}

public class TitanGenerationResponse
{
    public string GeneratedText { get; set; } = string.Empty;
    public string ModelId { get; set; } = string.Empty;
    public int TokensUsed { get; set; }
    public string FinishReason { get; set; } = string.Empty;
    public int InputTextTokenCount { get; set; }
    public List<TitanResult> Results { get; set; } = new();
}

public class ChatCompletionResponse
{
    public string Id { get; set; } = string.Empty;
    public string Object { get; set; } = string.Empty;
    public long Created { get; set; }
    public string Model { get; set; } = string.Empty;
    public List<ChatChoice> Choices { get; set; } = new();
    public UsageInfo Usage { get; set; } = new();
}

public class EmbeddingResponse
{
    public List<float[]> Embeddings { get; set; } = new();
    public string ModelId { get; set; } = string.Empty;
    public int InputTokenCount { get; set; }
}

public class AvailableModelsResponse
{
    public List<ModelInfo> Models { get; set; } = new();
}

// Supporting Models
public class ChatMessage
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class ChatChoice
{
    public int Index { get; set; }
    public ChatMessage Message { get; set; } = new();
    public string FinishReason { get; set; } = string.Empty;
}

public class UsageInfo
{
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public int TotalTokens => InputTokens + OutputTokens;
}

public class TitanResult
{
    public int TokenCount { get; set; }
    public string OutputText { get; set; } = string.Empty;
    public string CompletionReason { get; set; } = string.Empty;
}

public class ModelInfo
{
    public string ModelId { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[] InputModalities { get; set; } = Array.Empty<string>();
    public string[] OutputModalities { get; set; } = Array.Empty<string>();
    public int MaxTokens { get; set; }
}