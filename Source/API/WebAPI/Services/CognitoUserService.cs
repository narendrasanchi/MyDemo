using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Options;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class CognitoUserService : ICognitoUserService
    {
        private readonly AmazonCognitoIdentityProviderClient _cognitoClient;
        private readonly AwsSettings _awsSettings;
        private readonly ILogger<CognitoUserService> _logger;

        public CognitoUserService(
            AmazonCognitoIdentityProviderClient cognitoClient, 
            IOptions<AwsSettings> awsSettings,
            ILogger<CognitoUserService> logger)
        {
            _cognitoClient = cognitoClient;
            _awsSettings = awsSettings.Value;
            _logger = logger;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserRequest request)
        {
            try
            {
                var attributes = new List<AttributeType>
                {
                    new AttributeType { Name = "email", Value = request.Email },
                    new AttributeType { Name = "email_verified", Value = "false" }
                };

                if (!string.IsNullOrEmpty(request.PhoneNumber))
                {
                    attributes.Add(new AttributeType { Name = "phone_number", Value = request.PhoneNumber });
                    attributes.Add(new AttributeType { Name = "phone_number_verified", Value = "false" });
                }

                if (!string.IsNullOrEmpty(request.GivenName))
                    attributes.Add(new AttributeType { Name = "given_name", Value = request.GivenName });

                if (!string.IsNullOrEmpty(request.FamilyName))
                    attributes.Add(new AttributeType { Name = "family_name", Value = request.FamilyName });

                var createUserRequest = new AdminCreateUserRequest
                {
                    UserPoolId = _awsSettings.Cognito.UserPoolId,
                    Username = request.Username,
                    UserAttributes = attributes,
                    TemporaryPassword = request.Password,
                    MessageAction = MessageActionType.SUPPRESS
                };

                var response = await _cognitoClient.AdminCreateUserAsync(createUserRequest);

                // Set permanent password
                var setPasswordRequest = new AdminSetUserPasswordRequest
                {
                    UserPoolId = _awsSettings.Cognito.UserPoolId,
                    Username = request.Username,
                    Password = request.Password,
                    Permanent = true
                };

                await _cognitoClient.AdminSetUserPasswordAsync(setPasswordRequest);

                _logger.LogInformation("User {Username} created successfully", request.Username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user {Username}", request.Username);
                return false;
            }
        }

        public async Task<List<UserResponse>> GetUsersAsync()
        {
            try
            {
                var users = new List<UserResponse>();
                string? paginationToken = null;

                do
                {
                    var listUsersRequest = new ListUsersRequest
                    {
                        UserPoolId = _awsSettings.Cognito.UserPoolId,
                        Limit = 60,
                        PaginationToken = paginationToken
                    };

                    var response = await _cognitoClient.ListUsersAsync(listUsersRequest);

                    foreach (var user in response.Users)
                    {
                        users.Add(MapUserToResponse(user));
                    }

                    paginationToken = response.PaginationToken;
                } while (!string.IsNullOrEmpty(paginationToken));

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return new List<UserResponse>();
            }
        }

        public async Task<UserResponse?> GetUserAsync(string username)
        {
            try
            {
                var getUserRequest = new AdminGetUserRequest
                {
                    UserPoolId = _awsSettings.Cognito.UserPoolId,
                    Username = username
                };

                var response = await _cognitoClient.AdminGetUserAsync(getUserRequest);

                return MapUserToResponse(response);
            }
            catch (UserNotFoundException)
            {
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {Username}", username);
                return null;
            }
        }

        public async Task<bool> UpdateUserAsync(string username, UpdateUserRequest request)
        {
            try
            {
                var attributes = new List<AttributeType>();

                if (!string.IsNullOrEmpty(request.Email))
                    attributes.Add(new AttributeType { Name = "email", Value = request.Email });

                if (!string.IsNullOrEmpty(request.PhoneNumber))
                    attributes.Add(new AttributeType { Name = "phone_number", Value = request.PhoneNumber });

                if (!string.IsNullOrEmpty(request.GivenName))
                    attributes.Add(new AttributeType { Name = "given_name", Value = request.GivenName });

                if (!string.IsNullOrEmpty(request.FamilyName))
                    attributes.Add(new AttributeType { Name = "family_name", Value = request.FamilyName });

                if (attributes.Count > 0)
                {
                    var updateUserRequest = new AdminUpdateUserAttributesRequest
                    {
                        UserPoolId = _awsSettings.Cognito.UserPoolId,
                        Username = username,
                        UserAttributes = attributes
                    };

                    await _cognitoClient.AdminUpdateUserAttributesAsync(updateUserRequest);
                }

                _logger.LogInformation("User {Username} updated successfully", username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {Username}", username);
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            try
            {
                var deleteUserRequest = new AdminDeleteUserRequest
                {
                    UserPoolId = _awsSettings.Cognito.UserPoolId,
                    Username = username
                };

                await _cognitoClient.AdminDeleteUserAsync(deleteUserRequest);

                _logger.LogInformation("User {Username} deleted successfully", username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {Username}", username);
                return false;
            }
        }

        private static UserResponse MapUserToResponse(UserType user)
        {
            var response = new UserResponse
            {
                Username = user.Username,
                UserStatus = user.UserStatus.Value,
                CreationDate = user.UserCreateDate,
                LastModifiedDate = user.UserLastModifiedDate
            };

            foreach (var attr in user.Attributes)
            {
                switch (attr.Name)
                {
                    case "email":
                        response.Email = attr.Value;
                        break;
                    case "email_verified":
                        response.EmailVerified = bool.Parse(attr.Value);
                        break;
                    case "phone_number":
                        response.PhoneNumber = attr.Value;
                        break;
                    case "phone_number_verified":
                        response.PhoneVerified = bool.Parse(attr.Value);
                        break;
                    case "given_name":
                        response.GivenName = attr.Value;
                        break;
                    case "family_name":
                        response.FamilyName = attr.Value;
                        break;
                }
            }

            return response;
        }

        private static UserResponse MapUserToResponse(AdminGetUserResponse response)
        {
            var userResponse = new UserResponse
            {
                Username = response.Username,
                UserStatus = response.UserStatus.Value
            };

            foreach (var attr in response.UserAttributes)
            {
                switch (attr.Name)
                {
                    case "email":
                        userResponse.Email = attr.Value;
                        break;
                    case "email_verified":
                        userResponse.EmailVerified = bool.Parse(attr.Value);
                        break;
                    case "phone_number":
                        userResponse.PhoneNumber = attr.Value;
                        break;
                    case "phone_number_verified":
                        userResponse.PhoneVerified = bool.Parse(attr.Value);
                        break;
                    case "given_name":
                        userResponse.GivenName = attr.Value;
                        break;
                    case "family_name":
                        userResponse.FamilyName = attr.Value;
                        break;
                }
            }

            return userResponse;
        }
    }
}