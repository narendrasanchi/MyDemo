using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Models;
using Common;
using Common.Enum;
using Microsoft.Extensions.Logging;

namespace AWSDemo.AIServices.Services
{
    public class CognitoService : ICognitoService
    {
        private readonly AmazonCognitoIdentityProviderClient _cognitoClient;
        private readonly ILogger<CognitoService> _logger;
        private readonly string _userPoolId;
        private readonly string _clientId;

        public CognitoService(
            AmazonCognitoIdentityProviderClient cognitoClient, 
            ILogger<CognitoService> logger,
            string userPoolId,
            string clientId)
        {
            _cognitoClient = cognitoClient;
            _logger = logger;
            _userPoolId = userPoolId;
            _clientId = clientId;
        }

        public async Task<Response<UserRegistrationResponse>> RegisterUserAsync(UserRegistrationRequest request)
        {
            try
            {
                var signUpRequest = new SignUpRequest
                {
                    ClientId = _clientId,
                    Username = request.Username,
                    Password = request.Password,
                    UserAttributes = new List<AttributeType>
                    {
                        new AttributeType { Name = "email", Value = request.Email }
                    }
                };

                if (!string.IsNullOrEmpty(request.PhoneNumber))
                {
                    signUpRequest.UserAttributes.Add(new AttributeType { Name = "phone_number", Value = request.PhoneNumber });
                }

                foreach (var attr in request.UserAttributes)
                {
                    signUpRequest.UserAttributes.Add(new AttributeType { Name = attr.Key, Value = attr.Value });
                }

                var response = await _cognitoClient.SignUpAsync(signUpRequest);

                var result = new UserRegistrationResponse
                {
                    UserSub = response.UserSub,
                    Success = true,
                    EmailVerificationRequired = !response.UserConfirmed,
                    PhoneVerificationRequired = false // This would depend on your pool configuration
                };

                return new Response<UserRegistrationResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");
                return new Response<UserRegistrationResponse>(
                    new UserRegistrationResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<Models.ConfirmSignUpResponse>> ConfirmSignUpAsync(Models.ConfirmSignUpRequest request)
        {
            try
            {
                var confirmRequest = new Amazon.CognitoIdentityProvider.Model.ConfirmSignUpRequest
                {
                    ClientId = _clientId,
                    Username = request.Username,
                    ConfirmationCode = request.ConfirmationCode
                };

                await _cognitoClient.ConfirmSignUpAsync(confirmRequest);

                var result = new Models.ConfirmSignUpResponse
                {
                    Success = true
                };

                return new Response<Models.ConfirmSignUpResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming sign up");
                return new Response<Models.ConfirmSignUpResponse>(
                    new Models.ConfirmSignUpResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<UserLoginResponse>> LoginAsync(UserLoginRequest request)
        {
            try
            {
                var authRequest = new InitiateAuthRequest
                {
                    ClientId = _clientId,
                    AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                    AuthParameters = new Dictionary<string, string>
                    {
                        { "USERNAME", request.Username },
                        { "PASSWORD", request.Password }
                    }
                };

                var response = await _cognitoClient.InitiateAuthAsync(authRequest);

                if (response.ChallengeName != null)
                {
                    var result = new UserLoginResponse
                    {
                        Success = false,
                        ChallengeName = response.ChallengeName.Value,
                        Session = response.Session,
                        ErrorMessage = "Authentication challenge required"
                    };
                    return new Response<UserLoginResponse>(result, ResponseCode.BadRequest);
                }

                var loginResult = new UserLoginResponse
                {
                    AccessToken = response.AuthenticationResult.AccessToken,
                    IdToken = response.AuthenticationResult.IdToken,
                    RefreshToken = response.AuthenticationResult.RefreshToken,
                    ExpiresIn = response.AuthenticationResult.ExpiresIn,
                    Success = true
                };

                return new Response<UserLoginResponse>(loginResult, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return new Response<UserLoginResponse>(
                    new UserLoginResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<Models.ForgotPasswordResponse>> ForgotPasswordAsync(Models.ForgotPasswordRequest request)
        {
            try
            {
                var forgotPasswordRequest = new Amazon.CognitoIdentityProvider.Model.ForgotPasswordRequest
                {
                    ClientId = _clientId,
                    Username = request.Username
                };

                var response = await _cognitoClient.ForgotPasswordAsync(forgotPasswordRequest);

                var result = new Models.ForgotPasswordResponse
                {
                    CodeDeliveryDetails = response.CodeDeliveryDetails?.DeliveryMedium?.Value ?? string.Empty,
                    Success = true
                };

                return new Response<Models.ForgotPasswordResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating forgot password");
                return new Response<Models.ForgotPasswordResponse>(
                    new Models.ForgotPasswordResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<Models.ConfirmForgotPasswordResponse>> ConfirmForgotPasswordAsync(Models.ConfirmForgotPasswordRequest request)
        {
            try
            {
                var confirmRequest = new Amazon.CognitoIdentityProvider.Model.ConfirmForgotPasswordRequest
                {
                    ClientId = _clientId,
                    Username = request.Username,
                    ConfirmationCode = request.ConfirmationCode,
                    Password = request.NewPassword
                };

                await _cognitoClient.ConfirmForgotPasswordAsync(confirmRequest);

                var result = new Models.ConfirmForgotPasswordResponse
                {
                    Success = true
                };

                return new Response<Models.ConfirmForgotPasswordResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming forgot password");
                return new Response<Models.ConfirmForgotPasswordResponse>(
                    new Models.ConfirmForgotPasswordResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            try
            {
                var refreshRequest = new InitiateAuthRequest
                {
                    ClientId = _clientId,
                    AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
                    AuthParameters = new Dictionary<string, string>
                    {
                        { "REFRESH_TOKEN", request.RefreshToken }
                    }
                };

                var response = await _cognitoClient.InitiateAuthAsync(refreshRequest);

                var result = new RefreshTokenResponse
                {
                    AccessToken = response.AuthenticationResult.AccessToken,
                    IdToken = response.AuthenticationResult.IdToken,
                    ExpiresIn = response.AuthenticationResult.ExpiresIn,
                    Success = true
                };

                return new Response<RefreshTokenResponse>(result, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return new Response<RefreshTokenResponse>(
                    new RefreshTokenResponse { Success = false, ErrorMessage = ex.Message },
                    ResponseCode.BadRequest);
            }
        }

        public async Task<Response<bool>> SignOutAsync(string accessToken)
        {
            try
            {
                var signOutRequest = new GlobalSignOutRequest
                {
                    AccessToken = accessToken
                };

                await _cognitoClient.GlobalSignOutAsync(signOutRequest);
                return new Response<bool>(true, ResponseCode.Ok);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error signing out");
                return new Response<bool>(false, ResponseCode.BadRequest);
            }
        }
    }
}