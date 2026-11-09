# Measurement settings shared by both projects. Dot-source it: . ./scripts/env.ps1
# Projects stay as generated; everything that differs from the template is set here.
$env:ASPNETCORE_ENVIRONMENT = 'Production'
$env:DOTNET_ENVIRONMENT = 'Production'
$env:ConnectionStrings__Default = 'Host=localhost;Port=5433;Database=Bench;User ID=bench;Password=bench;'
# Information level writes several lines per request; under load the logger becomes the bottleneck.
$env:Serilog__MinimumLevel__Default = 'Warning'
$env:Serilog__MinimumLevel__Override__Microsoft = 'Warning'
