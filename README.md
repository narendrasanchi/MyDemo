# AWS AI Services Demo Project

A comprehensive learning project showcasing integration with various AWS AI services using .NET 8 Web API backend and Angular 19 frontend.

## 🚀 Features

This project demonstrates the following AWS AI services:

- **Amazon Polly** - Text-to-Speech conversion
- **Amazon Transcribe** - Speech-to-Text transcription  
- **Amazon Translate** - Language translation
- **Amazon Comprehend** - Sentiment analysis & entity detection
- **Amazon Rekognition** - Image analysis (detect labels, faces, moderation)
- **Amazon Lex** - Simple chatbot integration
- **Amazon Bedrock** - Foundation model text generation
- **AWS Cognito** - User authentication

## 🏗️ Architecture

### Backend (.NET 8 Web API)
- **Controllers**: Separate controllers for each AWS AI service
- **Services**: Business logic layer with service interfaces
- **Models**: DTOs for request/response handling
- **Infrastructure**: AWS SDK integration and configuration

### Frontend (Angular 19)
- **Dashboard**: Overview of all available AI services
- **Components**: Individual components for each AI service
- **Services**: HTTP services for API communication
- **Models**: TypeScript interfaces for type safety
- **Routing**: Navigation between different AI service pages

## 📋 Prerequisites

- **.NET 8 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 20+** - [Download here](https://nodejs.org/)
- **Angular CLI 19** - Install with `npm install -g @angular/cli@19`
- **Docker & Docker Compose** - [Download here](https://docs.docker.com/get-docker/)
- **AWS Account** with proper IAM permissions for AI services

## 🔧 AWS Setup

### 1. AWS Credentials
Configure your AWS credentials using one of these methods:

#### Option A: AWS CLI
```bash
aws configure
```

#### Option B: Environment Variables
```bash
export AWS_ACCESS_KEY_ID=your_access_key
export AWS_SECRET_ACCESS_KEY=your_secret_key
export AWS_DEFAULT_REGION=us-east-1
```

#### Option C: IAM Roles (Recommended for production)
Configure IAM roles with appropriate permissions when running on AWS infrastructure.

### 2. Required AWS Services Setup

#### Cognito User Pool
1. Create a User Pool in AWS Cognito
2. Note the User Pool ID and Client ID
3. Configure app client settings

#### S3 Bucket (for Transcribe)
1. Create an S3 bucket for audio file uploads
2. Configure proper CORS policy

#### Lex Bot (Optional)
1. Create a Lex bot using the V2 console
2. Note the Bot ID and Alias ID

### 3. IAM Permissions
Ensure your AWS credentials have permissions for:
- Amazon Polly
- Amazon Transcribe
- Amazon Translate
- Amazon Comprehend
- Amazon Rekognition
- Amazon Lex (if using chatbot)
- Amazon Bedrock (if using text generation)
- AWS Cognito
- Amazon S3 (for file uploads)

## 🚀 Getting Started

### Option 1: Docker Compose (Recommended)

1. **Clone the repository**
```bash
git clone <repository-url>
cd MyDemo
```

2. **Configure AWS settings**
Update the environment variables in `docker-compose.yml`:
```yaml
environment:
  - AWS__Cognito__UserPoolId=YOUR_USER_POOL_ID
  - AWS__Cognito__ClientId=YOUR_CLIENT_ID
  - AWS__S3__BucketName=YOUR_S3_BUCKET_NAME
  - AWS__Lex__BotId=YOUR_BOT_ID
  - AWS__Lex__BotAliasId=YOUR_BOT_ALIAS_ID
```

3. **Run the application**
```bash
docker-compose up --build
```

4. **Access the application**
- Frontend: http://localhost:4200
- Backend API: http://localhost:7109
- Swagger UI: http://localhost:7109/swagger

### Option 2: Manual Setup

#### Backend Setup
1. **Navigate to the API directory**
```bash
cd Source/API/WebAPI
```

2. **Update configuration**
Edit `appsettings.json` with your AWS settings:
```json
{
  "AWS": {
    "Region": "us-east-1",
    "Cognito": {
      "UserPoolId": "YOUR_USER_POOL_ID",
      "ClientId": "YOUR_CLIENT_ID"
    },
    "S3": {
      "BucketName": "YOUR_S3_BUCKET_NAME"
    },
    "Lex": {
      "BotId": "YOUR_BOT_ID",
      "BotAliasId": "YOUR_BOT_ALIAS_ID"
    }
  }
}
```

3. **Run the API**
```bash
dotnet restore
dotnet run
```

#### Frontend Setup
1. **Navigate to the frontend directory**
```bash
cd aws-ai-frontend
```

2. **Install dependencies**
```bash
npm install
```

3. **Update API base URL**
Edit the API base URL in service files if needed (default: `https://localhost:7109/api`)

4. **Run the frontend**
```bash
npm start
```

5. **Access the application**
- Frontend: http://localhost:4200
- Backend API: https://localhost:7109

## 🎯 Usage Guide

### 1. User Registration & Login
- Navigate to the Register page to create a new account
- Use the Login page to authenticate with AWS Cognito
- Once logged in, access all AI service features

### 2. Text-to-Speech (Polly)
- Enter text in the input field
- Select a voice from the available options
- Click "Generate Speech" to download the audio file

### 3. Sentiment Analysis (Comprehend)
- Enter text to analyze
- View sentiment (Positive, Negative, Neutral, Mixed)
- See detailed sentiment scores and detected entities

### 4. Image Analysis (Rekognition)
- Upload an image file
- Choose analysis type (Labels, Faces, Text, Moderation)
- View detected objects, faces, or text with confidence scores

### 5. Translation (Translate)
- Enter text to translate
- Select source and target languages
- View translated text

### 6. Text Generation (Bedrock)
- Enter a prompt
- Select a foundation model
- Configure generation parameters
- View generated text

### 7. Chatbot (Lex)
- Start a conversation with the chatbot
- Send messages and receive responses
- View conversation history

## 🐳 Docker Commands

### Build and run all services
```bash
docker-compose up --build
```

### Run in background
```bash
docker-compose up -d
```

### View logs
```bash
docker-compose logs -f
```

### Stop all services
```bash
docker-compose down
```

### Rebuild specific service
```bash
docker-compose build api
docker-compose build frontend
```

## 🔧 Development

### Backend Development
```bash
cd Source/API/WebAPI
dotnet watch run
```

### Frontend Development
```bash
cd aws-ai-frontend
npm start
```

### Running Tests
```bash
# Backend tests
dotnet test

# Frontend tests
cd aws-ai-frontend
npm test
```

## 📂 Project Structure

```
MyDemo/
├── Source/API/WebAPI/              # .NET 8 Web API
│   ├── Controllers/                # API Controllers
│   ├── Program.cs                 # Application entry point
│   └── appsettings.json           # Configuration
├── AWSDemo/                       # AWS Services Integration
│   ├── AIServices/
│   │   ├── Contracts/             # Service interfaces
│   │   ├── Models/                # DTOs and models
│   │   └── Services/              # Service implementations
│   └── AIServicesRegistration.cs  # DI registration
├── aws-ai-frontend/               # Angular 19 Frontend
│   ├── src/app/
│   │   ├── ai-services/           # AI service components
│   │   ├── auth/                  # Authentication components
│   │   ├── dashboard/             # Main dashboard
│   │   ├── models/                # TypeScript models
│   │   └── services/              # HTTP services
│   └── package.json
├── docker-compose.yml             # Docker Compose configuration
├── Dockerfile.api                 # Backend Docker image
├── Dockerfile.frontend            # Frontend Docker image
└── README.md                      # This file
```

## 🔐 Security Considerations

- **AWS Credentials**: Never commit AWS credentials to source control
- **CORS**: Configured for development; adjust for production
- **Authentication**: JWT tokens are stored in localStorage (consider more secure alternatives for production)
- **HTTPS**: Use HTTPS in production environments
- **Environment Variables**: Use environment-specific configuration

## 🚀 Deployment

### AWS Elastic Beanstalk
1. Create an Elastic Beanstalk application
2. Deploy the .NET API using the deployment package
3. Configure environment variables for AWS services

### AWS ECS
1. Create ECS cluster
2. Build and push Docker images to ECR
3. Create task definitions and services
4. Configure load balancer and security groups

### GitHub Actions CI/CD
Create `.github/workflows/deploy.yml` for automated deployment:

```yaml
name: Deploy to AWS
on:
  push:
    branches: [main]
jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: 8.0.x
      - name: Build and deploy
        run: |
          dotnet build
          # Add deployment steps
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Troubleshooting

### Common Issues

1. **AWS Credentials Error**
   - Verify AWS credentials are configured correctly
   - Check IAM permissions for required services

2. **Build Errors**
   - Ensure .NET 8 SDK is installed
   - Run `dotnet restore` to restore packages

3. **Angular Build Issues**
   - Verify Node.js version (20+)
   - Clear npm cache: `npm cache clean --force`
   - Delete node_modules and reinstall: `rm -rf node_modules && npm install`

4. **Docker Issues**
   - Ensure Docker is running
   - Check port conflicts
   - Verify Docker Compose version

### Getting Help

- Check the GitHub Issues page
- Review AWS service documentation
- Check Angular and .NET documentation

## 📚 Resources

- [AWS AI Services Documentation](https://docs.aws.amazon.com/ai-services/)
- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Angular 19 Documentation](https://angular.io/docs)
- [Docker Documentation](https://docs.docker.com/)
- [AWS SDK for .NET](https://aws.amazon.com/sdk-for-net/)