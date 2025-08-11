# AI Assistant API

A .NET 8 Web API that provides AI-powered chat functionality using Google's Gemini API, with user authentication and conversation memory management.

## Features

- 🤖 **AI Chat Integration**: Powered by Google's Gemini 1.5 Flash model
- 🔐 **JWT Authentication**: Secure user registration and login
- 💾 **Conversation Memory**: Persistent storage for authenticated users and in-memory storage for anonymous users
- 📝 **Input Validation**: FluentValidation for request validation
- 📚 **Swagger Documentation**: Interactive API documentation
- 🏗️ **Clean Architecture**: Separation of concerns with services and interfaces

## Prerequisites

- .NET 8 SDK
- Google Gemini API Key
- Visual Studio 2022 or VS Code

## Configuration

### 1. API Keys Setup

Create or update your `appsettings.json` file with the following configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "GeminiApiKey": "YOUR_GEMINI_API_KEY_HERE",
  "jwt": {
    "key": "YOUR_SUPER_SECRET_JWT_KEY_AT_LEAST_128_BITS_LONG",
    "Issuer": "JwtAuthDemo",
    "Audience": "JwtAuthDemoUser",
    "expiresInMinutes": 60
  }
}
```

### 2. JWT Key Generation

For production, generate a secure JWT key:

```bash
# Generate a secure random key (32 bytes = 256 bits)
openssl rand -base64 32
```

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd AiAssistantApi
```

### 2. Install Dependencies

```bash
dotnet restore
```

### 3. Run the Application

```bash
dotnet run
```

The API will be available at:
- **HTTP**: http://localhost:5025
- **HTTPS**: https://localhost:7009
- **Swagger UI**: https://localhost:7009/swagger

## API Endpoints

### Authentication

#### Register User
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "your_username",
  "password": "your_password",
  "email": "your_email@example.com"
}
```

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "your_username",
  "password": "your_password"
}
```

### AI Chat

#### Send Message (Authenticated)
```http
POST /api/gemini/ask
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "prompt": "What is the weather like today?"
}
```

#### Send Message (Anonymous)
```http
POST /api/gemini/ask
X-Anonymous-User-Id: ANON_12345
Content-Type: application/json

{
  "prompt": "What is the weather like today?"
}
```

## Architecture

### Project Structure

```
AiAssistantApi/
├── Controllers/
│   ├── AuthController.cs          # Authentication endpoints
│   └── GeminiController.cs        # AI chat endpoints
├── Models/
│   ├── User.cs                    # User entity
│   └── Dtos/
│       ├── GeminiRequest.cs       # Chat request DTO
│       ├── GeminiResponse.cs      # Chat response DTO
│       └── Auth/
│           ├── LoginRequest.cs    # Login request DTO
│           └── RegisterRequest.cs # Registration request DTO
├── Services/
│   ├── Interfaces/
│   │   ├── IGeminiService.cs      # Gemini service interface
│   │   └── IPromptMemoryService.cs # Memory service interface
│   ├── Auth/
│   │   └── AuthService.cs         # Authentication logic
│   ├── GeminiService.cs           # Gemini API integration
│   ├── PromptMemoryService.cs     # Memory coordination
│   ├── InMemoryService.cs         # Anonymous user storage
│   └── FileMemoryService.cs       # Authenticated user storage
├── Validators/
│   └── GeminiValidator.cs         # Request validation
└── Program.cs                     # Application configuration
```

### Key Components

#### Authentication Service
- User registration and login
- JWT token generation and validation
- File-based user storage (users.json)

#### Memory Management
- **InMemoryService**: Temporary storage for anonymous users
- **FileMemoryService**: Persistent JSON storage for authenticated users
- **PromptMemoryService**: Coordinates between storage types based on user type

#### AI Integration
- Google Gemini 1.5 Flash model integration
- Context-aware conversations using conversation history
- Error handling for API failures

## Usage Examples

### 1. Register a New User

```bash
curl -X POST "https://localhost:7009/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "secure_password",
    "email": "john@example.com"
  }'
```

### 2. Login and Get Token

```bash
curl -X POST "https://localhost:7009/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "john_doe",
    "password": "secure_password"
  }'
```

### 3. Send AI Message (Authenticated)

```bash
curl -X POST "https://localhost:7009/api/gemini/ask" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "prompt": "Explain quantum computing in simple terms"
  }'
```

### 4. Send AI Message (Anonymous)

```bash
curl -X POST "https://localhost:7009/api/gemini/ask" \
  -H "X-Anonymous-User-Id: ANON_12345" \
  -H "Content-Type: application/json" \
  -d '{
    "prompt": "What is machine learning?"
  }'
```

## Security Considerations

### Production Deployment

1. **Environment Variables**: Store sensitive configuration in environment variables
2. **HTTPS Only**: Always use HTTPS in production
3. **Strong JWT Keys**: Use cryptographically secure random keys
4. **API Key Security**: Secure your Gemini API key
5. **Input Validation**: All inputs are validated using FluentValidation
6. **Rate Limiting**: Consider implementing rate limiting for production

### Security Headers

The application includes standard security headers and JWT authentication middleware.

## Error Handling

The API provides comprehensive error handling:

- **400 Bad Request**: Invalid input data
- **401 Unauthorized**: Missing or invalid authentication
- **500 Internal Server Error**: Server-side errors

All errors include descriptive messages and appropriate HTTP status codes.

## Development

### Adding New Features

1. Create DTOs in `Models/Dtos/`
2. Add validation in `Validators/`
3. Implement services in `Services/`
4. Create controllers in `Controllers/`
5. Register services in `Program.cs`

### Testing

```bash
# Run tests
dotnet test

# Build the project
dotnet build

# Run with specific environment
dotnet run --environment Development
```

## Dependencies

- **.NET 8**: Latest LTS version
- **FluentValidation**: Input validation
- **JWT Bearer**: Authentication
- **Swashbuckle**: API documentation
- **Newtonsoft.Json**: JSON serialization

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support and questions:
- Create an issue in the repository
- Check the Swagger documentation at `/swagger`
- Review the API endpoints documentation above

---

**Note**: This is a development/demo application. For production use, implement additional security measures, proper logging, monitoring, and database storage. 