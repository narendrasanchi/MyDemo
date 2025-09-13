# AWS AI Services Demo

A comprehensive demo application showcasing various AWS AI services through a modern web interface built with .NET 8 Web API and Angular 19.

![AWS AI Services API](https://github.com/user-attachments/assets/453e318f-322d-41b3-bda5-8f948eada634)

![AWS AI Services Frontend Dashboard](https://github.com/user-attachments/assets/680164a1-a36f-4adf-820f-8c3b3bd5f14a)

## Features

### AWS AI Services Integrated
- **Amazon Polly** - Text-to-Speech conversion with multiple voices
- **Amazon Transcribe** - Speech-to-Text conversion with language support
- **Amazon Translate** - Text translation between multiple languages
- **Amazon Comprehend** - Sentiment analysis, entity detection, and text insights
- **Amazon Rekognition** - Image analysis, face detection, and content moderation
- **Amazon Lex** - Conversational chatbot interface
- **Amazon Bedrock** - AI text generation with multiple models (Claude, Titan, etc.)

### Technical Stack
- **Backend**: .NET 8 Web API with comprehensive AWS SDK integration
- **Frontend**: Angular 19 with Material Design components
- **Authentication**: AWS Cognito (JWT-based, configurable)
- **Containerization**: Docker and Docker Compose
- **Documentation**: Complete Swagger/OpenAPI documentation

## Architecture

```
┌─────────────────┐    HTTP/REST    ┌──────────────────┐    AWS SDK    ┌─────────────────┐
│   Angular 19    │◄───────────────►│  .NET 8 Web API  │◄──────────────►│   AWS Services  │
│   Frontend      │                 │    Backend       │               │                 │
└─────────────────┘                 └──────────────────┘               └─────────────────┘
        │                                    │
        │                                    │
        ▼                                    ▼
┌─────────────────┐                 ┌──────────────────┐
│   Material UI   │                 │  Swagger/OpenAPI │
│   Dashboard     │                 │  Documentation   │
└─────────────────┘                 └──────────────────┘
```

## Quick Start

### Prerequisites
- .NET 8 SDK
- Node.js 20.x and npm
- Docker and Docker Compose
- AWS Account with configured credentials

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd MyDemo
   ```

2. **Configure AWS Credentials**
   ```bash
   # Configure AWS CLI with your credentials
   aws configure
   
   # Or set environment variables
   export AWS_ACCESS_KEY_ID=your-access-key
   export AWS_SECRET_ACCESS_KEY=your-secret-key
   export AWS_DEFAULT_REGION=us-east-1
   ```

3. **Run Backend (.NET 8 Web API)**
   ```bash
   cd Source/API/WebAPI
   dotnet restore
   dotnet run
   # API will be available at http://localhost:5141
   # Swagger documentation at http://localhost:5141
   ```

4. **Run Frontend (Angular 19)**
   ```bash
   cd frontend
   npm install
   npm start
   # Frontend will be available at http://localhost:4200
   ```

### Docker Deployment

1. **Build and run with Docker Compose**
   ```bash
   docker-compose up --build
   ```
   
2. **Access the application**
   - Frontend: http://localhost:4200
   - Backend API: http://localhost:5141
   - API Documentation: http://localhost:5141

## AWS Cognito Setup (Optional)

### Prerequisites
- AWS Account with Cognito service access
- AWS CLI configured with appropriate permissions

### Steps

1. **Create User Pool**
   ```bash
   aws cognito-idp create-user-pool \
     --pool-name "aws-ai-demo-pool" \
     --policies "PasswordPolicy={MinimumLength=8,RequireUppercase=true,RequireLowercase=true,RequireNumbers=true,RequireSymbols=false}" \
     --region us-east-1
   ```

2. **Create User Pool Client**
   ```bash
   aws cognito-idp create-user-pool-client \
     --user-pool-id <your-user-pool-id> \
     --client-name "aws-ai-demo-client" \
     --no-generate-secret \
     --region us-east-1
   ```

3. **Update Configuration**
   
   Update `Source/API/WebAPI/appsettings.json`:
   ```json
   {
     "Cognito": {
       "UserPoolId": "your-actual-user-pool-id",
       "ClientId": "your-actual-client-id",
       "Region": "us-east-1"
     }
   }
   ```

4. **Update Docker Compose** (if using containers)
   
   Update environment variables in `docker-compose.yml`:
   ```yaml
   environment:
     - Cognito__UserPoolId=your-actual-user-pool-id
     - Cognito__ClientId=your-actual-client-id
   ```

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration

### Text-to-Speech (Polly)
- `POST /api/Polly/text-to-speech` - Convert text to speech
- `GET /api/Polly/voices` - Get available voices
- `GET /api/Polly/languages` - Get supported languages

### Speech-to-Text (Transcribe)
- `POST /api/Transcribe/transcribe` - Transcribe audio file
- `GET /api/Transcribe/job/{jobName}` - Get transcription job status
- `GET /api/Transcribe/supported-languages` - Get supported languages

### Translation (Translate)
- `POST /api/Translate/translate` - Translate text
- `POST /api/Translate/detect-language` - Detect text language
- `GET /api/Translate/supported-languages` - Get supported languages
- `POST /api/Translate/batch-translate` - Batch translate multiple texts

### Text Analysis (Comprehend)
- `POST /api/Comprehend/sentiment` - Analyze sentiment
- `POST /api/Comprehend/entities` - Detect entities
- `POST /api/Comprehend/key-phrases` - Extract key phrases
- `POST /api/Comprehend/language` - Detect dominant language
- `POST /api/Comprehend/comprehensive-analysis` - Complete text analysis

### Image Analysis (Rekognition)
- `POST /api/Rekognition/detect-labels` - Detect objects and labels
- `POST /api/Rekognition/detect-faces` - Detect and analyze faces
- `POST /api/Rekognition/detect-moderation` - Content moderation analysis
- `POST /api/Rekognition/detect-text` - Extract text from images

### Chatbot (Lex)
- `POST /api/Lex/recognize-text` - Send text message to bot
- `POST /api/Lex/recognize-speech` - Send audio message to bot
- `POST /api/Lex/start-conversation` - Start new conversation
- `GET /api/Lex/session/{sessionId}/history` - Get conversation history

### AI Text Generation (Bedrock)
- `POST /api/Bedrock/generate-text` - Generate text with any model
- `POST /api/Bedrock/generate-claude` - Generate text with Claude
- `POST /api/Bedrock/generate-titan` - Generate text with Titan
- `POST /api/Bedrock/chat` - Chat completion
- `GET /api/Bedrock/models` - Get available models
- `POST /api/Bedrock/embeddings` - Generate text embeddings

## Frontend Components

### Dashboard
Modern Material Design dashboard with cards for each AWS AI service.

### Service Components
- **Text-to-Speech**: Input text, select voice, generate and play audio
- **Speech-to-Text**: Upload audio files, get transcription with language detection
- **Translation**: Text input with source/target language selection
- **Image Analysis**: Image upload with object/face/text detection results
- **Sentiment Analysis**: Text analysis with sentiment scores and entity extraction
- **Chatbot**: Interactive chat interface with conversation history
- **Text Generation**: AI text generation with multiple model options

## Configuration

### Backend Configuration
Located in `Source/API/WebAPI/appsettings.json`:

```json
{
  "AWS": {
    "Region": "us-east-1",
    "Profile": "default"
  },
  "Cognito": {
    "UserPoolId": "your-user-pool-id",
    "ClientId": "your-client-id",
    "Region": "us-east-1"
  },
  "CORS": {
    "AllowedOrigins": [
      "http://localhost:4200",
      "http://localhost:3000"
    ]
  }
}
```

### Frontend Configuration
Located in `frontend/src/environments/`:

- `environment.ts` - Development settings
- `environment.prod.ts` - Production settings

## Development

### Backend Development
```bash
cd Source/API/WebAPI
dotnet watch run
```

### Frontend Development
```bash
cd frontend
npm run start
# or for hot reload
ng serve --open
```

### Building for Production

#### Backend
```bash
cd Source/API/WebAPI
dotnet publish -c Release -o ./publish
```

#### Frontend
```bash
cd frontend
npm run build --prod
```

## Testing

### Run Backend Tests
```bash
dotnet test
```

### Run Frontend Tests
```bash
cd frontend
npm test
```

## Troubleshooting

### Common Issues

1. **AWS Credentials Not Found**
   - Ensure AWS CLI is configured: `aws configure`
   - Verify credentials: `aws sts get-caller-identity`
   - Check environment variables

2. **CORS Issues**
   - Verify frontend URL in `appsettings.json` CORS configuration
   - Check browser console for specific CORS errors

3. **Docker Build Issues**
   - Ensure Docker is running
   - Check for sufficient disk space
   - Verify all files are present in build context

4. **API Connection Issues**
   - Verify backend is running on correct port
   - Check firewall settings
   - Verify environment configuration

### Logs
- Backend logs: Console output when running `dotnet run`
- Frontend logs: Browser console (F12 Developer Tools)
- Docker logs: `docker-compose logs -f`

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues and questions:
1. Check the troubleshooting section
2. Review AWS service documentation
3. Open an issue in the repository

## Credits

- Built with .NET 8 and Angular 19
- Uses AWS SDK for .NET and AWS Amplify
- Material Design components from Angular Material
- Containerization with Docker

---

**Note**: This is a demo application for educational and testing purposes. For production use, implement proper security measures, error handling, and monitoring.