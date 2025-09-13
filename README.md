# AWS Cognito User Management Demo

A complete demo application built with .NET 8 Web API and Angular 19 that integrates with AWS Cognito for user management.

## Features

### Backend (.NET 8 Web API)
- ✅ Swagger-enabled REST API
- ✅ AWS Cognito integration using AWSSDK.CognitoIdentityProvider
- ✅ Complete user management operations:
  - `POST /api/users/register` - Register new user
  - `GET /api/users` - List all users
  - `GET /api/users/{username}` - Get specific user
  - `PUT /api/users/{username}` - Update user attributes
  - `DELETE /api/users/{username}` - Delete user
- ✅ Proper error handling and validation
- ✅ CORS configuration for Angular frontend

### Frontend (Angular 19)
- ✅ Angular 19 with Material UI design system
- ✅ Responsive user interface components:
  - **RegisterUserComponent** - User registration form with validation
  - **UserListComponent** - Data table with Edit/Delete actions
  - **EditUserComponent** - User profile update form
- ✅ Complete TypeScript service layer (UserService)
- ✅ Form validation and error handling
- ✅ Success/error notifications
- ✅ Routing and navigation

## Screenshots

### API Documentation (Swagger UI)
![Swagger API](https://github.com/user-attachments/assets/94ada249-1dab-44a8-82c1-e22fb2c169f7)

### User Management Interface
![User List](https://github.com/user-attachments/assets/46041073-c341-4d2a-b962-d30b8999c561)

### User Registration Form
![User Registration](https://github.com/user-attachments/assets/797a063a-7dc3-49ac-bb2d-39611b7a8e88)

## Prerequisites

- .NET 8 SDK
- Node.js (v16 or higher)
- AWS Account with Cognito User Pool configured

## Setup Instructions

### 1. AWS Cognito Configuration

1. **Create AWS Cognito User Pool:**
   ```bash
   # Using AWS CLI (optional)
   aws cognito-idp create-user-pool \
     --pool-name "MyDemoUserPool" \
     --policies PasswordPolicy='{MinimumLength=8,RequireUppercase=true,RequireLowercase=true,RequireNumbers=true,RequireSymbols=false}' \
     --auto-verified-attributes email
   ```

2. **Create User Pool Client:**
   ```bash
   aws cognito-idp create-user-pool-client \
     --user-pool-id <your-user-pool-id> \
     --client-name "MyDemoClient" \
     --generate-secret
   ```

3. **Note down the following values:**
   - User Pool ID
   - Client ID  
   - Client Secret
   - AWS Region

### 2. Backend Setup (.NET 8 Web API)

1. **Navigate to the Web API directory:**
   ```bash
   cd Source/API/WebAPI
   ```

2. **Update `appsettings.json` with your AWS Cognito credentials:**
   ```json
   {
     "AWS": {
       "Region": "us-east-1",
       "Cognito": {
         "UserPoolId": "your-actual-user-pool-id",
         "ClientId": "your-actual-client-id",
         "ClientSecret": "your-actual-client-secret"
       }
     }
   }
   ```

3. **Restore dependencies and run:**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

4. **Verify API is running:**
   - API: http://localhost:5141
   - Swagger UI: http://localhost:5141/swagger

### 3. Frontend Setup (Angular 19)

1. **Navigate to the Angular app directory:**
   ```bash
   cd user-management-app
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Start the development server:**
   ```bash
   ng serve
   ```

4. **Access the application:**
   - Frontend: http://localhost:4200

## Project Structure

```
MyDemo/
├── Source/API/WebAPI/                 # .NET 8 Web API
│   ├── Controllers/UsersController.cs # User management endpoints
│   ├── Services/CognitoUserService.cs # AWS Cognito integration
│   ├── Models/                        # Request/Response models
│   └── appsettings.json              # AWS configuration
├── user-management-app/               # Angular 19 Frontend
│   ├── src/app/components/           # UI Components
│   │   ├── register-user/            # Registration form
│   │   ├── user-list/               # User data table
│   │   └── edit-user/               # User edit form
│   ├── src/app/services/            # API service layer
│   └── src/app/models/              # TypeScript models
└── README.md
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/users` | Get all users from Cognito |
| GET | `/api/users/{username}` | Get specific user details |
| POST | `/api/users/register` | Register new user in Cognito |
| PUT | `/api/users/{username}` | Update user attributes |
| DELETE | `/api/users/{username}` | Delete user from Cognito |

## Usage Workflow

1. **Register a New User:**
   - Navigate to registration form
   - Fill in user details (username, email, password, etc.)
   - Submit form → User created in AWS Cognito

2. **View All Users:**
   - Navigate to user list
   - View all users in Material table format
   - See user status, email, phone, etc.

3. **Edit User:**
   - Click edit button on any user
   - Update user attributes
   - Save changes → Updates in AWS Cognito

4. **Delete User:**
   - Click delete button on any user
   - Confirm deletion → Removes from AWS Cognito

## Technologies Used

### Backend
- .NET 8 Web API
- AWSSDK.CognitoIdentityProvider
- Swashbuckle.AspNetCore (Swagger)

### Frontend  
- Angular 19
- Angular Material UI
- TypeScript
- RxJS

## Development Notes

- Frontend runs on port 4200 by default
- Backend API runs on port 5141 by default  
- CORS is configured to allow requests from localhost:4200
- All API responses include proper HTTP status codes and error messages
- Form validation is implemented on both frontend and backend
- The application gracefully handles AWS service errors

## Security Considerations

- Passwords must meet AWS Cognito policy requirements (8+ characters)
- Email addresses are validated
- User input is sanitized and validated
- AWS Cognito handles secure user authentication
- API includes proper error handling without exposing sensitive information

## Troubleshooting

1. **API Connection Issues:**
   - Verify backend is running on port 5141
   - Check CORS configuration
   - Verify AWS credentials in appsettings.json

2. **AWS Cognito Errors:**
   - Check User Pool ID and Client ID are correct
   - Verify AWS region matches your Cognito setup
   - Ensure IAM permissions for Cognito operations

3. **Frontend Build Issues:**
   - Run `npm install` to ensure all dependencies are installed
   - Check Node.js version compatibility
   - Clear npm cache if needed: `npm cache clean --force`