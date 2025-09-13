using AWSDemo;
using DesignPatternsDemo;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Polly;
using Amazon.TranscribeService;
using Amazon.Translate;
using Amazon.Comprehend;
using Amazon.Rekognition;
using Amazon.LexRuntimeV2;
using Amazon.BedrockRuntime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "AWS AI Services Demo API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new()
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new() { Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// Configure AWS services
var awsOptions = new AWSOptions
{
    Region = Amazon.RegionEndpoint.GetBySystemName(builder.Configuration["AWS:Region"] ?? "us-east-1"),
    Profile = builder.Configuration["AWS:Profile"] ?? "default"
};
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonPolly>();
builder.Services.AddAWSService<IAmazonTranscribeService>();
builder.Services.AddAWSService<IAmazonTranslate>();
builder.Services.AddAWSService<IAmazonComprehend>();
builder.Services.AddAWSService<IAmazonRekognition>();
builder.Services.AddAWSService<IAmazonLexRuntimeV2>();
builder.Services.AddAWSService<IAmazonBedrockRuntime>();

// Configure JWT Authentication
var cognitoConfig = builder.Configuration.GetSection("Cognito");
var userPoolId = cognitoConfig["UserPoolId"];
var region = cognitoConfig["Region"];

if (!string.IsNullOrEmpty(userPoolId) && !string.IsNullOrEmpty(region) && 
    userPoolId != "your-user-pool-id")
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });
}

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>() ?? 
            new[] { "http://localhost:4200" };
        
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDesignPatternServices();
builder.Services.AddDynamoDbServiceRegistration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AWS AI Services Demo API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the root
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
