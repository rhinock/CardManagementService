# Card Management Service

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

$Env:ASPNETCORE_ENVIRONMENT = ""
dotnet run --no-launch-profile
```