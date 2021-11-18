
- [Use multiple environments in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-5.0)
```PowerShell
$Env:ASPNETCORE_ENVIRONMENT = "Staging"
dotnet run --no-launch-profile

$Env:ASPNETCORE_ENVIRONMENT = ""
dotnet run --no-launch-profile

$Env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet run --no-launch-profile
```