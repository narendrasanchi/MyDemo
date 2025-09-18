# Amazon Connect Chat Widget Angular Demo

A complete demonstration of integrating **Amazon Connect chat widget** with an Angular application, featuring real-time customer support messaging capabilities.

## 🚀 Features

- **Angular 18** application with Amazon Connect chat integration
- **Dynamic script loading** of Amazon Connect chat interface
- **Configurable environment** settings for development and production
- **Responsive design** with mobile-friendly chat widget
- **Error handling** and user feedback
- **Optional backend proxy** for advanced chat functionality
- **Toggle chat window** with smooth animations
- **TypeScript** with proper type declarations

## 📋 Prerequisites

Before running this demo, ensure you have:

1. **Node.js** (v18 or higher)
2. **npm** (v8 or higher)
3. **Angular CLI** (installed globally)
4. **Amazon Connect Instance** properly configured
5. **Chat widget snippet ID** from Amazon Connect Console
6. **CloudFront distribution** for Amazon Connect chat script (optional)

## 🛠️ Quick Setup

### 1. Clone and Install Dependencies

```bash
# Navigate to the Angular application
cd connect-chat-demo

# Install dependencies
npm install
```

### 2. Configure Environment Variables

Update the environment files with your Amazon Connect settings:

**`src/environments/environment.ts`** (Development):
```typescript
export const environment = {
  production: false,
  connectWidgetUrl: 'https://your-cloudfront-url.net/amazon-connect-chat-interface-client.js',
  snippetId: 'your-snippet-id-from-amazon-connect',
  connectInstanceUrl: 'https://your-instance.awsapps.com/connect',
  backendApiUrl: 'http://localhost:3000/api'
};
```

**`src/environments/environment.prod.ts`** (Production):
```typescript
export const environment = {
  production: true,
  connectWidgetUrl: 'https://your-production-cloudfront.net/amazon-connect-chat-interface-client.js',
  snippetId: 'your-production-snippet-id',
  connectInstanceUrl: 'https://your-production-instance.awsapps.com/connect',
  backendApiUrl: 'https://your-production-api.com/api'
};
```

### 3. Run the Application

```bash
# Start development server
npm start

# Or build for production
npm run build
```

The application will be available at `http://localhost:4200`

## 📱 How to Use

1. **Open the application** in your browser
2. **Click the chat button** in the bottom-right corner
3. **Start a conversation** with customer support
4. **Agent responses** will appear in the chat widget (agents use Amazon Connect CCP)

## 🏗️ Architecture

### Frontend (Angular)

```
src/
├── app/
│   ├── chat-widget/          # Main chat widget component
│   │   ├── chat-widget.ts    # Component logic
│   │   ├── chat-widget.html  # Template
│   │   └── chat-widget.css   # Styles
│   ├── app.ts               # Main app component
│   └── app.html             # App template
├── environments/            # Environment configurations
└── assets/                 # Static assets
```

### Key Components

- **ChatWidgetComponent**: Handles Amazon Connect script loading, chat initialization, and UI interactions
- **Environment Configuration**: Manages different settings for development/production
- **Dynamic Script Loading**: Loads Amazon Connect chat script asynchronously
- **Error Handling**: Provides user-friendly error messages

## 🔧 Optional Backend Proxy

For advanced functionality, you can use the included Node.js backend proxy:

### Setup Backend

```bash
# Navigate to backend directory
cd backend-proxy

# Install dependencies
npm install

# Copy environment file and configure
cp .env.example .env
# Edit .env with your AWS credentials and Connect settings
```

### Environment Variables for Backend

```bash
AWS_REGION=us-east-1
AWS_ACCESS_KEY_ID=your-aws-access-key-id
AWS_SECRET_ACCESS_KEY=your-aws-secret-access-key
PORT=3000
CONNECT_INSTANCE_ID=your-connect-instance-id
CONTACT_FLOW_ID=your-contact-flow-id
```

### Start Backend Server

```bash
# Start the proxy server
npm start

# Or for development with auto-reload
npm run dev
```

### Backend API Endpoints

- `GET /health` - Health check
- `POST /api/start-chat-contact` - Start a new chat session
- `POST /api/get-transcript` - Retrieve chat messages
- `POST /api/send-message` - Send a message

## 🔐 Amazon Connect Configuration

### 1. Set up Connect Instance

1. **Create Amazon Connect instance** in AWS Console
2. **Configure contact flows** for chat
3. **Enable chat** in your Connect instance
4. **Set up routing profiles** and queues

### 2. Get Snippet ID

1. Navigate to **Amazon Connect Console**
2. Go to **Customer Experience** → **Channels** → **Chat**
3. Create or edit a **chat widget configuration**
4. Copy the **snippet ID** from the configuration

### 3. CloudFront Distribution (Optional)

For production use, set up a CloudFront distribution to serve the Amazon Connect chat script:

1. Create CloudFront distribution
2. Set origin to Amazon Connect chat script URL
3. Configure caching policies
4. Update `connectWidgetUrl` in environment files

## 🎨 Customization

### Styling

The chat widget can be customized by modifying:

- `src/app/chat-widget/chat-widget.css` - Widget-specific styles
- `src/app/app.css` - Global application styles

### Configuration

You can customize the chat behavior by modifying the initialization parameters in `chat-widget.ts`:

```typescript
connect.ChatInterface.init({
  containerId: 'amazon-connect-chat-container',
  headerConfig: {
    isVisible: true,
    logoImageURL: 'your-logo-url',
    chatTitle: 'Customer Support Chat'
  },
  widgetConfig: {
    isVisible: true,
    width: '400px',
    height: '600px'
  },
  snippetId: environment.snippetId
});
```

## 🧪 Testing

### Unit Tests

```bash
cd connect-chat-demo
npm test
```

### Manual Testing

1. **Without Connect Configuration**: The app should show error messages gracefully
2. **With Connect Configuration**: Chat widget should load and connect properly
3. **Mobile Responsiveness**: Test on different screen sizes
4. **Error Scenarios**: Test network failures, invalid configurations

## 📦 Deployment

### Frontend Deployment

```bash
# Build for production
cd connect-chat-demo
npm run build

# Deploy the dist/ folder to your hosting provider
```

### Backend Deployment

The backend proxy can be deployed to:

- **AWS Lambda** with API Gateway
- **AWS ECS** or **EKS**
- **Heroku**, **Vercel**, or other platforms
- **Traditional servers** with PM2

## 🐛 Troubleshooting

### Common Issues

1. **Chat widget not loading**
   - Check `connectWidgetUrl` in environment.ts
   - Verify network connectivity
   - Check browser console for errors

2. **Invalid snippet ID error**
   - Verify snippet ID in Amazon Connect Console
   - Ensure Connect instance is properly configured
   - Check environment configuration

3. **CORS errors with backend proxy**
   - Verify CORS configuration in server.js
   - Check API endpoint URLs
   - Ensure proper AWS credentials

### Debug Mode

Enable debug logging by opening browser dev tools and checking the console for detailed error messages.

## 📚 Additional Resources

- [Amazon Connect Developer Guide](https://docs.aws.amazon.com/connect/latest/adminguide/)
- [Amazon Connect Chat SDK](https://github.com/amazon-connect/amazon-connect-chatjs)
- [Angular Documentation](https://angular.io/docs)
- [Express.js Documentation](https://expressjs.com/)

## 📄 License

This project is licensed under the MIT License. See the LICENSE file for details.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

---

## 📞 Support

If you encounter any issues or need help with Amazon Connect configuration, please refer to the AWS documentation or contact AWS Support.