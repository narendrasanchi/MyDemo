using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Common.Enum;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace AWSDemo.AIServices.Services
{
    public class BedrockService : IBedrockService
    {
        private readonly AmazonBedrockRuntimeClient _bedrockClient;
        private readonly ILogger<BedrockService> _logger;

        public BedrockService(AmazonBedrockRuntimeClient bedrockClient, ILogger<BedrockService> logger)
        {
            _bedrockClient = bedrockClient;
            _logger = logger;
        }

        public async Task<Response<TextGenerationResponse>> GenerateTextAsync(TextGenerationRequest request)
        {
            try
            {
                // Create the request body based on the model
                var requestBody = CreateTextGenerationRequestBody(request);
                var requestBodyJson = JsonSerializer.Serialize(requestBody);

                var invokeModelRequest = new InvokeModelRequest
                {
                    ModelId = request.ModelId,
                    Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBodyJson))
                };

                var response = await _bedrockClient.InvokeModelAsync(invokeModelRequest);

                using var reader = new StreamReader(response.Body);
                var responseBody = await reader.ReadToEndAsync();
                var responseJson = JsonDocument.Parse(responseBody);

                var generatedText = ExtractGeneratedText(responseJson, request.ModelId);
                var tokensUsed = ExtractTokensUsed(responseJson, request.ModelId);

                var result = new TextGenerationResponse
                {
                    GeneratedText = generatedText,
                    TokensUsed = tokensUsed,
                    Success = true
                };

                return new Response<TextGenerationResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating text with Bedrock");
                return new Response<TextGenerationResponse>(
                    new TextGenerationResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<ImageGenerationResponse>> GenerateImageAsync(ImageGenerationRequest request)
        {
            try
            {
                var requestBody = new
                {
                    textToImageParams = new
                    {
                        text = request.Prompt
                    },
                    taskType = "TEXT_IMAGE",
                    imageGenerationConfig = new
                    {
                        numberOfImages = 1,
                        height = request.Height,
                        width = request.Width,
                        quality = request.Quality
                    }
                };

                var requestBodyJson = JsonSerializer.Serialize(requestBody);
                var invokeModelRequest = new InvokeModelRequest
                {
                    ModelId = request.ModelId,
                    Body = new MemoryStream(Encoding.UTF8.GetBytes(requestBodyJson))
                };

                var response = await _bedrockClient.InvokeModelAsync(invokeModelRequest);

                using var reader = new StreamReader(response.Body);
                var responseBody = await reader.ReadToEndAsync();
                var responseJson = JsonDocument.Parse(responseBody);

                // Extract the base64 encoded image
                var imageBase64 = responseJson.RootElement
                    .GetProperty("images")[0]
                    .GetString();

                var imageData = Convert.FromBase64String(imageBase64);

                var result = new ImageGenerationResponse
                {
                    ImageData = imageData,
                    ContentType = "image/png",
                    Success = true
                };

                return new Response<ImageGenerationResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating image with Bedrock");
                return new Response<ImageGenerationResponse>(
                    new ImageGenerationResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<ListModelsResponse>> GetAvailableModelsAsync()
        {
            try
            {
                // Note: BedrockRuntime doesn't have a ListModels API
                // This would typically come from Bedrock service, not runtime
                // For now, return a static list of common models
                var models = new List<BedrockModelInfo>
                {
                    new() { ModelId = "anthropic.claude-3-sonnet-20240229-v1:0", ModelName = "Claude 3 Sonnet", Provider = "Anthropic", IsActive = true },
                    new() { ModelId = "anthropic.claude-3-haiku-20240307-v1:0", ModelName = "Claude 3 Haiku", Provider = "Anthropic", IsActive = true },
                    new() { ModelId = "amazon.titan-text-express-v1", ModelName = "Titan Text Express", Provider = "Amazon", IsActive = true },
                    new() { ModelId = "amazon.titan-image-generator-v1", ModelName = "Titan Image Generator", Provider = "Amazon", IsActive = true },
                    new() { ModelId = "meta.llama2-70b-chat-v1", ModelName = "Llama 2 70B Chat", Provider = "Meta", IsActive = true }
                };

                var result = new ListModelsResponse
                {
                    Models = models,
                    Success = true
                };

                return new Response<ListModelsResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available models");
                return new Response<ListModelsResponse>(
                    new ListModelsResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        private object CreateTextGenerationRequestBody(TextGenerationRequest request)
        {
            // Different models have different request formats
            if (request.ModelId.Contains("anthropic.claude"))
            {
                return new
                {
                    anthropic_version = "bedrock-2023-05-31",
                    max_tokens = request.MaxTokens,
                    messages = new[]
                    {
                        new { role = "user", content = request.Prompt }
                    },
                    temperature = request.Temperature,
                    top_p = request.TopP
                };
            }
            else if (request.ModelId.Contains("amazon.titan"))
            {
                return new
                {
                    inputText = request.Prompt,
                    textGenerationConfig = new
                    {
                        maxTokenCount = request.MaxTokens,
                        temperature = request.Temperature,
                        topP = request.TopP
                    }
                };
            }
            else
            {
                // Default format
                return new
                {
                    prompt = request.Prompt,
                    max_tokens = request.MaxTokens,
                    temperature = request.Temperature,
                    top_p = request.TopP
                };
            }
        }

        private string ExtractGeneratedText(JsonDocument responseJson, string modelId)
        {
            try
            {
                if (modelId.Contains("anthropic.claude"))
                {
                    return responseJson.RootElement
                        .GetProperty("content")[0]
                        .GetProperty("text")
                        .GetString() ?? string.Empty;
                }
                else if (modelId.Contains("amazon.titan"))
                {
                    return responseJson.RootElement
                        .GetProperty("results")[0]
                        .GetProperty("outputText")
                        .GetString() ?? string.Empty;
                }
                else
                {
                    // Try common response formats
                    if (responseJson.RootElement.TryGetProperty("generation", out var generation))
                    {
                        return generation.GetString() ?? string.Empty;
                    }
                    if (responseJson.RootElement.TryGetProperty("text", out var text))
                    {
                        return text.GetString() ?? string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error extracting generated text from response");
            }

            return string.Empty;
        }

        private int ExtractTokensUsed(JsonDocument responseJson, string modelId)
        {
            try
            {
                if (modelId.Contains("anthropic.claude"))
                {
                    if (responseJson.RootElement.TryGetProperty("usage", out var usage))
                    {
                        return usage.GetProperty("output_tokens").GetInt32();
                    }
                }
                else if (modelId.Contains("amazon.titan"))
                {
                    if (responseJson.RootElement.TryGetProperty("results", out var results))
                    {
                        return results[0].GetProperty("tokenCount").GetInt32();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error extracting token count from response");
            }

            return 0;
        }
    }
}