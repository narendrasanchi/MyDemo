using WebAPI.Models;

namespace WebAPI.Services
{
    public interface ICognitoUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserRequest request);
        Task<List<UserResponse>> GetUsersAsync();
        Task<bool> UpdateUserAsync(string username, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(string username);
        Task<UserResponse?> GetUserAsync(string username);
    }
}