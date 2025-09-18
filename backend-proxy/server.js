const express = require('express');
const cors = require('cors');
const AWS = require('aws-sdk');
require('dotenv').config();

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(cors());
app.use(express.json());

// Configure AWS
AWS.config.update({
  region: process.env.AWS_REGION || 'us-east-1',
  accessKeyId: process.env.AWS_ACCESS_KEY_ID,
  secretAccessKey: process.env.AWS_SECRET_ACCESS_KEY
});

const connect = new AWS.Connect();

// Health check endpoint
app.get('/health', (req, res) => {
  res.json({ status: 'OK', message: 'Amazon Connect Chat Proxy Server is running' });
});

// Start chat contact endpoint
app.post('/api/start-chat-contact', async (req, res) => {
  try {
    const {
      instanceId,
      contactFlowId,
      customerDisplayName = 'Customer',
      attributes = {}
    } = req.body;

    if (!instanceId || !contactFlowId) {
      return res.status(400).json({
        error: 'Missing required parameters: instanceId and contactFlowId'
      });
    }

    const params = {
      InstanceId: instanceId,
      ContactFlowId: contactFlowId,
      Attributes: attributes,
      ParticipantDetails: {
        DisplayName: customerDisplayName
      },
      ChatDurationInMinutes: 60 // Set chat duration to 60 minutes
    };

    const result = await connect.startChatContact(params).promise();
    
    res.json({
      success: true,
      contactId: result.ContactId,
      participantId: result.ParticipantId,
      participantToken: result.ParticipantToken
    });

  } catch (error) {
    console.error('Error starting chat contact:', error);
    res.status(500).json({
      error: 'Failed to start chat contact',
      details: error.message
    });
  }
});

// Get chat messages endpoint
app.post('/api/get-transcript', async (req, res) => {
  try {
    const { connectionToken, maxResults = 15 } = req.body;

    if (!connectionToken) {
      return res.status(400).json({
        error: 'Missing required parameter: connectionToken'
      });
    }

    const connectParticipant = new AWS.ConnectParticipant();
    
    const params = {
      ConnectionToken: connectionToken,
      MaxResults: maxResults,
      SortOrder: 'ASCENDING'
    };

    const result = await connectParticipant.getTranscript(params).promise();
    
    res.json({
      success: true,
      transcript: result.Transcript,
      nextToken: result.NextToken
    });

  } catch (error) {
    console.error('Error getting transcript:', error);
    res.status(500).json({
      error: 'Failed to get transcript',
      details: error.message
    });
  }
});

// Send message endpoint
app.post('/api/send-message', async (req, res) => {
  try {
    const { connectionToken, content, contentType = 'text/plain' } = req.body;

    if (!connectionToken || !content) {
      return res.status(400).json({
        error: 'Missing required parameters: connectionToken and content'
      });
    }

    const connectParticipant = new AWS.ConnectParticipant();
    
    const params = {
      ConnectionToken: connectionToken,
      Content: content,
      ContentType: contentType
    };

    const result = await connectParticipant.sendMessage(params).promise();
    
    res.json({
      success: true,
      id: result.Id,
      absoluteTime: result.AbsoluteTime
    });

  } catch (error) {
    console.error('Error sending message:', error);
    res.status(500).json({
      error: 'Failed to send message',
      details: error.message
    });
  }
});

// Error handling middleware
app.use((error, req, res, next) => {
  console.error('Server error:', error);
  res.status(500).json({
    error: 'Internal server error',
    details: error.message
  });
});

// 404 handler
app.use((req, res) => {
  res.status(404).json({
    error: 'Endpoint not found'
  });
});

app.listen(PORT, () => {
  console.log(`Amazon Connect Chat Proxy Server running on port ${PORT}`);
  console.log(`Health check available at: http://localhost:${PORT}/health`);
  console.log(`Make sure to set the following environment variables:`);
  console.log(`- AWS_REGION (default: us-east-1)`);
  console.log(`- AWS_ACCESS_KEY_ID`);
  console.log(`- AWS_SECRET_ACCESS_KEY`);
});