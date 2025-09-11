using AWSDemo.AIServices.Models;
using Common;

namespace AWSDemo.AIServices.Contracts
{
    public interface ILexService
    {
        Task<Response<ChatResponse>> SendMessageAsync(ChatMessage message);
        Task<Response<CreateSessionResponse>> CreateSessionAsync(CreateSessionRequest request);
        Task<Response<bool>> DeleteSessionAsync(string sessionId);
    }
}