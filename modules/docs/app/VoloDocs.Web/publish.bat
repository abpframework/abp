@echo off

dotnet clean
dotnet restore
dotnet build

DEL /F/Q/S "C:\Publishes\VoloDocs" > NUL 
RMDIR /Q/S "C:\Publishes\VoloDocs"

dotnet publish -c Release -o "C:\Publishes\VoloDocs\Web"

cd..\VoloDocs.Migrator && dotnet publish -c Release -o "C:\Publishes\VoloDocs\Migrator"