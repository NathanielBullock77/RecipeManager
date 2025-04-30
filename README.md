# Recipe Manager

A cross-platform recipe management application built with .NET MAUI and clean architecture principles.

## 🍽️ Features

- **Recipe Management**: Create, edit, browse, and delete your favorite recipes
- **User Accounts**: Secure login/registration system with personalized recipe collections
- **Meal Planning**: Plan your meals for the week/month with an intuitive calendar interface
- **Shopping Lists**: Automatically generate shopping lists from your meal plans
- **Dietary Preferences**: Filter recipes based on dietary tags (Vegetarian, Vegan, Gluten-Free, etc.)
- **Cross-Platform**: Works seamlessly on Windows, macOS, iOS, and Android

 
 ## 📚 Project Structure

```
RecipeManager/
├── src/
│   ├── RecipeManager.Core.Models/         # Domain entities
│   ├── RecipeManager.Application.Services/ # Business logic layer
│   ├── RecipeManager.Infrastructure.Persistence/ # Data access layer
│   └── RecipeManager.MAUI/                # UI layer
├── tests/
│   ├── RecipeManager.Core.Tests/
│   ├── RecipeManager.Application.Tests/
│   └── RecipeManager.Integration.Tests/
└── tools/                                 # Utility scripts
```

## 🏗️ Architecture

Recipe Manager follows clean architecture principles with distinct layers:

### Core Domain Models
- **Recipe**: Contains title, description, cooking time, ingredients, instructions, dietary tags
- **User**: Stores account information, dietary preferences, and favorite recipes
- **DietaryTag**: Enum of dietary restrictions (Vegetarian, Vegan, GlutenFree, DairyFree, NutFree)
- **Ingredient**: Represents recipe ingredients with name and quantity
- **MealPlanEntry**: Links users to recipes for specific dates

### Architecture Layers
1. **Core Models** (.Core.Models): Domain entities
2. **Repository Layer** (.Infrastructure.Persistence.Repositories):
   - Interface definitions for data access
   - InMemory implementation for testing
   - SQLite implementation for production
3. **Application Services** (.Application.Services): 
   - Business logic implementation
   - Service interfaces for dependency injection
4. **UI Layer** (.MAUI): 
   - Cross-platform UI using .NET MAUI
   - MVVM pattern implementation

## 📋 Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0) or newer
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (17.3 or higher) with the .NET MAUI workload installed
- For iOS development: Mac with Xcode 14 or newer
- For Android development: Android SDK with API level 30+

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/recipe-manager.git
   cd recipe-manager
   ```

2. Open the solution in Visual Studio 2022:
   ```
   RecipeManager.sln
   ```

3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

4. Build the solution:
   ```bash
   dotnet build
   ```

5. Run the application:
   ```bash
   dotnet run --project src/RecipeManager.MAUI
   ```

On first run, the application will initialize the SQLite database with sample data including a test user and recipes.

## 📱 Deployment

### Android
```bash
dotnet publish -f net7.0-android -c Release
```

### iOS
```bash
dotnet publish -f net7.0-ios -c Release
```

### Windows
```bash
dotnet publish -f net7.0-windows10.0.19041.0 -c Release
```

### macOS
```bash
dotnet publish -f net7.0-maccatalyst -c Release
```

## 🧪 Testing

```bash
dotnet test
```

## Test User

The application comes pre-seeded with a test user:
- Username: testuser
- Password: your_test_password
- Email: test@example.com
- Dietary Preferences: Vegetarian



## 🛠️ Built With

- [.NET MAUI](https://docs.microsoft.com/en-us/dotnet/maui/) - Cross-platform UI framework
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) - ORM for data access
- [SQLite](https://www.sqlite.org/) - Embedded database
