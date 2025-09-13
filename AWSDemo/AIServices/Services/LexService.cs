using Amazon.LexRuntimeV2;
using Amazon.LexRuntimeV2.Model;
using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Common.Enum;
using Microsoft.Extensions.Logging;

namespace AWSDemo.AIServices.Services
{
    public class LexService : ILexService
    {
        private readonly AmazonLexRuntimeV2Client _lexClient;
        private readonly ILogger<LexService> _logger;

        public LexService(AmazonLexRuntimeV2Client lexClient, ILogger<LexService> logger)
        {
            _lexClient = lexClient;
            _logger = logger;
        }

        public async Task<Response<ChatResponse>> SendMessageAsync(ChatMessage message)
        {
            try
            {
                var request = new RecognizeTextRequest
                {
                    BotId = "BOTID123", // This should be configurable
                    BotAliasId = "TSTALIASID", // This should be configurable
                    LocaleId = "en_US",
                    SessionId = message.SessionId,
                    Text = message.Message
                };

                var response = await _lexClient.RecognizeTextAsync(request);

                var result = new ChatResponse
                {
                    Message = string.Join(" ", response.Messages?.Select(m => m.Content) ?? new List<string>()),
                    Success = true
                };

                return new Response<ChatResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to Lex");
                return new Response<ChatResponse>(
                    new ChatResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<CreateSessionResponse>> CreateSessionAsync(CreateSessionRequest request)
        {
            try
            {
                // Generate a unique session ID
                var sessionId = Guid.NewGuid().ToString();

                var result = new CreateSessionResponse
                {
                    SessionId = sessionId,
                    Success = true
                };

                return new Response<CreateSessionResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Lex session");
                return new Response<CreateSessionResponse>(
                    new CreateSessionResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<bool>> DeleteSessionAsync(string sessionId)
        {
            try
            {
                var request = new DeleteSessionRequest
                {
                    BotId = "BOTID123", // This should be configurable
                    BotAliasId = "TSTALIASID", // This should be configurable
                    LocaleId = "en_US",
                    SessionId = sessionId
                };

                await _lexClient.DeleteSessionAsync(request);
                return new Response<bool>(true, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Lex session");
                return new Response<bool>(false, ResponseCode.BadRequest);
            }
        }
    }
}