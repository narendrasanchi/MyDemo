using AWSDemo;
using DesignPatternsDemo;
using Amazon.CognitoIdentityProvider;
using WebAPI.Models;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure AWS Settings
builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AWS"));

// Add AWS Cognito Client
builder.Services.AddSingleton<AmazonCognitoIdentityProviderClient>();

// Add Cognito User Service
builder.Services.AddScoped<ICognitoUserService, CognitoUserService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:4200") // Angular default port
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

builder.Services.AddDesignPatternServices();
builder.Services.AddDynamoDbServiceRegistration();
//builder.Services.RegisterAbstractFactoryClass();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
