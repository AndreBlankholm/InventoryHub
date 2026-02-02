# Full-Stack Blazor WebAssembly + .NET 8 Minimal API

A complete full-stack application demonstrating modern .NET development patterns with Blazor WebAssembly client consuming a .NET 8 Minimal API server.

## 🚀 Project Structure

```
FullStackAppcd/
├── ClientAppdotnet/          # Blazor WebAssembly Client
│   ├── Pages/
│   │   └── FetchProducts.razor    # Product display component
│   ├── Layout/               # Application layout components
│   └── Program.cs           # Client configuration & HttpClient setup
├── ServerApp/               # .NET 8 Minimal API Server
│   └── Program.cs          # API endpoints & validation
├── REFLECTION.md           # 📖 Development insights & Copilot learnings
└── README.md              # This file
```

## 🛠️ Technology Stack

- **Frontend**: Blazor WebAssembly (.NET 8)
- **Backend**: ASP.NET Core Minimal API (.NET 8)
- **Validation**: Data Annotations with server-side validation
- **Error Handling**: RFC 7807 Problem Details
- **CORS**: Cross-origin resource sharing enabled
- **JSON**: Industry-standard camelCase serialization

## ⚡ Features

### API Features
- ✅ **RESTful Endpoints** - GET `/api/productlist`, POST `/api/products`
- ✅ **Data Validation** - Server-side validation with detailed error messages
- ✅ **Typed Models** - Strongly-typed Product and Category classes
- ✅ **Industry Standards** - RFC 7807 Problem Details, proper HTTP status codes
- ✅ **CORS Enabled** - Cross-origin support for Blazor client

### Client Features
- ✅ **Real-time Data Fetching** - Consumes API with HttpClient
- ✅ **Error Handling** - Displays validation errors and connection issues
- ✅ **Responsive UI** - Bootstrap-based responsive design
- ✅ **Type Safety** - Strongly-typed models matching server

## 🏃‍♂️ Quick Start

### Prerequisites
- .NET 8 SDK or later
- Visual Studio 2022 or VS Code

### Running the Application

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd FullStackAppcd
   ```

2. **Start the API Server** (Terminal 1)
   ```bash
   cd ServerApp
   dotnet run
   ```
   Server runs on: `http://localhost:5039`

3. **Start the Blazor Client** (Terminal 2)
   ```bash
   cd ClientAppdotnet
   dotnet run
   ```
   Client runs on: `http://localhost:5128`

4. **Access the Application**
   - Open browser to `http://localhost:5128`
   - Navigate to "Fetch Products" to see API integration

## 📊 API Endpoints

### GET `/api/productlist`
Returns all products with categories.

**Response Example:**
```json
[
  {
    "id": 1,
    "name": "Laptop",
    "price": 1200.50,
    "stock": 25,
    "category": {
      "id": 101,
      "name": "Electronics"
    }
  }
]
```

### POST `/api/products`
Creates a new product with validation.

**Request Body:**
```json
{
  "id": 3,
  "name": "Mouse",
  "price": 25.99,
  "stock": 50,
  "category": {
    "id": 102,
    "name": "Accessories"
  }
}
```

## 🔧 Configuration Details

### HttpClient Setup (Client)
```csharp
// Points to API server
builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri("http://localhost:5039/") 
});
```

### CORS Policy (Server)
```csharp
// Allows cross-origin requests from Blazor client
options.AddPolicy("BlazorPolicy", policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});
```

## 🐛 Troubleshooting

### Common Issues
- **Port conflicts**: Ensure ports 5039 (server) and 5128 (client) are available
- **CORS errors**: Verify server CORS policy is enabled
- **API not found**: Check that server is running before starting client
- **JSON errors**: Ensure client models match server response structure

### Quick Diagnostic Checklist
1. ✅ Are both applications running?
2. ✅ Do API endpoints match between client and server?
3. ✅ Is CORS configured properly?
4. ✅ Is HttpClient pointing to correct server address?
5. ✅ Are data models compatible for JSON serialization?

## 📖 Development Insights

> **Want to learn about the development process?**
> 
> 👉 **Check out [REFLECTION.md](./REFLECTION.md)** for detailed insights on:
> - How GitHub Copilot assisted in building this application
> - Challenges encountered and solutions implemented
> - Lessons learned about full-stack development with AI assistance
> - Best practices for systematic problem-solving

## 🎯 Learning Objectives

This project demonstrates:
- **Modern .NET Architecture** - Separation of concerns between client and API
- **Industry Standards** - Following HTTP, JSON, and REST conventions
- **Error Handling** - Comprehensive validation and error reporting
- **Type Safety** - End-to-end type safety from API to UI
- **AI-Assisted Development** - Effective use of GitHub Copilot for full-stack development

## 📄 License

This project is for educational purposes and demonstration of modern .NET development patterns.

---

**Built with .NET 8 | Enhanced with GitHub Copilot | February 2, 2026**