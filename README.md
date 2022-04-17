# Card Management Service
## L02

- Create and set up a template project
- Set up the configuration from file and environment variables
- Middleware for logging requests
- Middleware for adding and getting cards for user
- Store data in memory
- Card model:
  - Id
  - CVC
  - Pan (card number)
  - Expire (expiration date)
    - Month
    - Year
  - Name (card name, user-defined)
  - IsDefault (whether the card is default payment card)
  - UserId

## [Use multiple environments in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-5.0)
```PowerShell
$Env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run --launch-profile "Development"

$Env:ASPNETCORE_ENVIRONMENT = "Staging"
dotnet run --launch-profile "Staging"

dotnet run --no-launch-profile
```

## L03

- Added controllers for:
  - adding a card
  - deleting a card
  - getting a card or cards
  - changing card's name
- Configured swagger
- Added filters for logging requests and responses which hides card's secret data:
  - only 4 last digits of the pan is shown
  - CVC is hidden completely
- Added validation in one base model:
  - Luhn algorithm
  - card expiration date
  - during adding (including exceptions)

## L04, L05

- Added 
  - Entity Framework (EF) Core
  - PostgreSQL DB
  - Repository pattern for interaction with DB
- Removed
  - In-memory data