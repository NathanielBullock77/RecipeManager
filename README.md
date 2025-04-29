# Recipe Manager MAUI Application

This is a .NET MAUI Recipe Manager application with a clean architecture approach.

## Project Structure

- **RecipeManager.Core.Models**: Contains the core domain models
- **RecipeManager.Infrastructure.Persistence.Repositories**: Contains repository interfaces
- **RecipeManager.Infrastructure.Persistence.Implementation.InMemory**: Contains in-memory implementations of repositories
- **RecipeManager.Application.Services**: Contains service interfaces
- **RecipeManager.Application.Implementation**: Contains service implementations
- **RecipeManager.MAUI**: MAUI UI application

## Prerequisites

- .NET 7.0 SDK or newer
- .NET MAUI workload installed

## Setup

1. Install the .NET MAUI workload if you haven't already:

```bash
dotnet workload install maui
```

2. Restore dependencies:

```bash
dotnet restore
```

3. Build the solution:

```bash
dotnet build
```

## Running the Application

### Windows

```bash
dotnet run --project RecipeManager.MAUI -f net7.0-windows10.0.19041.0
```

### Android (requires Android SDK installed)

```bash
dotnet run --project RecipeManager.MAUI -f net7.0-android
```

### iOS (requires Mac with Xcode installed)

```bash
dotnet run --project RecipeManager.MAUI -f net7.0-ios
```

## Test User

The application comes pre-seeded with a test user:
- Username: testuser
- Password: your_test_password
- Email: test@example.com
- Dietary Preferences: Vegetarian

## Features

- User authentication (login/register)
- Recipe management (browse, add, edit, delete)
- Meal planning
- Shopping list generation
- Dietary preferences

## Architecture

This application follows a clean architecture approach:
- Core domain models are independent of any other layers
- Repository interfaces define data access methods
- Application services implement business logic
- MAUI UI interacts with application services 