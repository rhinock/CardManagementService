# Card Management Service

- Create and set up a template project
- Setting up the configuration from file and environment variables
- Middleware for logging requests.
- Middleware with methods for adding and adding cards for user.
- Data is stored in memory.
- Card model:
  - CVC
  - Pan (card number)
  - Expire (expiration date)
  - Name (card name, user-defined)
  - IsDefault (whether the card is default paymend card)
  - UserId

# [Use multiple environments in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-5.0)
```PowerShell
$Env:ASPNETCORE_ENVIRONMENT = "Staging"
dotnet run --no-launch-profile

$Env:ASPNETCORE_ENVIRONMENT = ""
dotnet run --no-launch-profile

$Env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run --no-launch-profile
```