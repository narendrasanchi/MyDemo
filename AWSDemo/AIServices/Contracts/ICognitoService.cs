using AWSDemo.AIServices.Models;
using Common;

namespace AWSDemo.AIServices.Contracts
{
    public interface ICognitoService
    {
        Task<Response<Models.UserRegistrationResponse>> RegisterUserAsync(Models.UserRegistrationRequest request);
        Task<Response<Models.ConfirmSignUpResponse>> ConfirmSignUpAsync(Models.ConfirmSignUpRequest request);
        Task<Response<Models.UserLoginResponse>> LoginAsync(Models.UserLoginRequest request);
        Task<Response<Models.ForgotPasswordResponse>> ForgotPasswordAsync(Models.ForgotPasswordRequest request);
        Task<Response<Models.ConfirmForgotPasswordResponse>> ConfirmForgotPasswordAsync(Models.ConfirmForgotPasswordRequest request);
        Task<Response<Models.RefreshTokenResponse>> RefreshTokenAsync(Models.RefreshTokenRequest request);
        Task<Response<bool>> SignOutAsync(string accessToken);
    }
}