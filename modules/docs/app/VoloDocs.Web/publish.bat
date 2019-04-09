@echo off
rem dotnet clean
rem  dotnet build
DEL /F/Q/S "C:\Publishes\VoloDocs.Web" > NUL
RMDIR /Q/S "C:\Publishes\VoloDocs.Web"
dotnet publish -c Release -o "C:\Publishes\VoloDocs.Web"