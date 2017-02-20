$buildFolder = (Get-Item -Path ".\" -Verbose).FullName
$outputFolder = Join-Path $buildFolder "outputs"

$slnFolder = Join-Path $buildFolder "..\"
$abpDeskFolder = Join-Path $slnFolder "src/AbpDesk"
$abpDeskWebFolder = Join-Path $abpDeskFolder "AbpDesk.Web.Mvc"

Set-Location $slnFolder
dotnet restore

Set-Location $abpDeskWebFolder
dotnet publish --output (Join-Path $outputFolder "AbpDesk/Web")

Set-Location $buildFolder
