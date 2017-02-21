# COMMON PATHS

$buildFolder = (Get-Item -Path ".\" -Verbose).FullName
$slnFolder = Join-Path $buildFolder "..\"
$outputFolder = Join-Path $buildFolder "outputs"
$abpDeskFolder = Join-Path $slnFolder "src/AbpDesk"
$abpDeskWebFolder = Join-Path $abpDeskFolder "AbpDesk.Web.Mvc"

# CLEAR

Remove-Item $outputFolder -Force -Recurse
New-Item -Path $outputFolder -ItemType Directory

# BUILD

Set-Location $slnFolder
dotnet restore

# PUBLISH

Set-Location $abpDeskWebFolder
dotnet publish --output (Join-Path $outputFolder "AbpDesk/Web")

# CREATE DOCKER IMAGES

Set-Location (Join-Path $outputFolder "AbpDesk/Web")

docker rmi abpdesk/web -f
docker build -t abpdesk/web .

Copy-Item (Join-Path $slnFolder "/docker/*.*") $outputFolder

# FINALIZE

Set-Location $outputFolder