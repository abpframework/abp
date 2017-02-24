# COMMON PATHS

$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$slnFolder = Join-Path $buildFolder "../"
$outputFolder = Join-Path $buildFolder "outputs"
$abpDeskFolder = Join-Path $slnFolder "src/AbpDesk"
$abpDeskWebFolder = Join-Path $abpDeskFolder "AbpDesk.Web.Mvc"

## CLEAR ######################################################################

Remove-Item $outputFolder -Force -Recurse
New-Item -Path $outputFolder -ItemType Directory

## RESTORE NUGET PACKAGES #####################################################

Set-Location $slnFolder
dotnet restore

## PUBLISH ASPDESK WEB ########################################################

Set-Location $abpDeskWebFolder
dotnet publish --output (Join-Path $outputFolder "AbpDesk/Web")

New-Item -Path (Join-Path $outputFolder "AbpDesk/Web/PlugIns") -ItemType Directory
Copy-Item (Join-Path $abpDeskFolder "Web_PlugIns/*") (Join-Path $outputFolder "AbpDesk/Web/PlugIns/")

## PUBLISH IDENTITY HTTP API HOST #############################################

Set-Location (Join-Path $slnFolder "src/Volo.Abp.Identity.HttpApi.Host")
dotnet publish --output (Join-Path $outputFolder "AbpIdentity/HttpApiHost")

## CREATE DOCKER IMAGES #######################################################

Set-Location (Join-Path $outputFolder "AbpDesk/Web")

docker rmi abpdesk/web -f
docker build -t abpdesk/web .

Set-Location (Join-Path $outputFolder "AbpIdentity/HttpApiHost")

docker rmi abpidentity/httpapihost -f
docker build -t abpidentity/httpapihost .

## DOCKER COMPOSE FILES #######################################################

Copy-Item (Join-Path $slnFolder "docker/*.*") $outputFolder

## FINALIZE ###################################################################

Set-Location $outputFolder