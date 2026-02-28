🍽️ Restaurant Management API

A secure and modular Restaurant Management System API built with ASP.NET Core 8 Web API, implementing JWT authentication, refresh tokens, role-based authorization, and ownership validation.
This project follows a layered architecture and supports restaurant operations such as orders, products, cart management, loyalty points, payments, and more.

🚀 Tech Stack
Framework: ASP.NET Core 8 Web API
Language: C#
Database: Microsoft SQL Server
Data Access: ADO.NET (No Entity Framework)
Authentication: JWT (Access + Refresh Tokens)
Authorization: Role-Based + Ownership Validation
API Documentation: Swagger

🏗️ Architecture
The project follows a clean layered structure:

RestaurantApi
│
├── API          → Controllers, Middleware, Authentication
├── Business     → Business Logic & Services
├── Data         → Database Access (ADO.NET)
├── DTOs         → Data Transfer Objects
Design Principles
Separation of concerns
Business logic isolated from controllers
Secure authentication and authorization
Ownership validation for sensitive resources
Environment-based configuration

🔐 Authentication & Authorization
Authentication is handled using JWT (JSON Web Tokens).

Features
User Registration
User Login
Refresh Token Endpoint
Role-Based Authorization
Supported Roles
Customer
Admin
Staff
Driver

Ownership Rules
Users can only access and modify their own data
Admin has full system access
Role-based restrictions applied to endpoints

JWT Secret Configuration
The JWT secret is stored using a Windows environment variable:
setx JWT_SECRET_KEY "VeryLongRandomSecretKey1234567890"
After setting the variable, restart Visual Studio.

📦 Core Modules
The API includes:
Orders
Products (Meals, Desserts, Drinks)
Categories
Addons
Product Addons
Cart
Payments
Order Statuses
Loyalty Points
Locations
Shifts
Settings
User Roles

📖 API Documentation
Swagger is enabled.
After running the project:

https://localhost:5001/swagger
Port may vary depending on your launch settings.
You can test all endpoints directly from Swagger UI.

🧪 Testing
Tested manually using Swagger UI
All secured endpoints require a valid JWT token
Future improvement: Add unit and integration testing.

🔒 Security Features
JWT Authentication
Refresh Token mechanism
Role-based authorization
Ownership validation
Protected endpoints
Environment-based secret storage

📌 Future Improvements
Add Unit Testing (xUnit)
Add Integration Testing
Implement Logging (Serilog)
Add Docker support
CI/CD pipeline
Cloud deployment (Azure / AWS)

👨‍💻 Author
Adam Hagos
Backend Developer | ASP.NET Core
Focused on building secure and scalable RESTful APIs.
