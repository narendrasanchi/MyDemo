using Amazon.BedrockRuntime;
using Amazon.CognitoIdentityProvider;
using Amazon.Comprehend;
using Amazon.LexRuntimeV2;
using Amazon.Polly;
using Amazon.Rekognition;
using Amazon.TranscribeService;
using Amazon.Translate;
using AWSDemo.AIServices.Contracts;
using AWSDemo.AIServices.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AWSDemo
{
    public static class AIServicesRegistration
    {
        public static IServiceCollection AddAWSAIServices(this IServiceCollection services, IConfiguration configuration)
        {
            // AWS AI Service Clients
            services.AddSingleton<AmazonPollyClient>();
            services.AddSingleton<AmazonTranscribeServiceClient>();
            services.AddSingleton<AmazonTranslateClient>();
            services.AddSingleton<AmazonComprehendClient>();
            services.AddSingleton<AmazonRekognitionClient>();
            services.AddSingleton<AmazonLexRuntimeV2Client>();
            services.AddSingleton<AmazonBedrockRuntimeClient>();
            services.AddSingleton<AmazonCognitoIdentityProviderClient>();

            // Register AWS AI Services
            services.AddScoped<IPollyService, PollyService>();
            services.AddScoped<ITranscribeService, TranscribeService>();
            services.AddScoped<ITranslateService, TranslateService>();
            services.AddScoped<IComprehendService, ComprehendService>();
            services.AddScoped<IRekognitionService, RekognitionService>();
            services.AddScoped<ILexService, LexService>();
            services.AddScoped<IBedrockService, BedrockService>();

            // Register Cognito Service with configuration
            services.AddScoped<ICognitoService>(provider =>
            {
                var cognitoClient = provider.GetRequiredService<AmazonCognitoIdentityProviderClient>();
                var logger = provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CognitoService>>();
                var userPoolId = configuration["AWS:Cognito:UserPoolId"] ?? string.Empty;
                var clientId = configuration["AWS:Cognito:ClientId"] ?? string.Empty;
                
                return new CognitoService(cognitoClient, logger, userPoolId, clientId);
            });

            return services;
        }
    }
}