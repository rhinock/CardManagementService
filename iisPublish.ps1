Import-Module WebAdministration

$services = @(
    "GatewayService",
    "BalancerService",
    "CardDataService",
    "LoggerService",
    "OperationDataService",
    "RightsService"
)

$IISSitesPath = "F:\IISSites"
$BaseUri = "http://localhost"

iisreset -stop

foreach($service in $services)
{
    $servicePath = "$IISSitesPath\$service"
    
    if (Test-Path $servicePath)
    {
        Remove-Item $servicePath -Recurse -Force
    }

    dotnet publish $service --no-restore -c Release -o $servicePath
}

iisreset -start

Start-Website -Name "Default Web Site"

foreach($service in $services)
{
    Invoke-WebRequest -Verbose -Uri "$BaseUri/$service" 
}