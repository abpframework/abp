@echo off

dotnet clean
dotnet restore
dotnet build

DEL /F/Q/S "C:\Publishes\VoloDocs" > NUL && RMDIR /Q/S "C:\Publishes\VoloDocs"

dotnet publish -c Release -r linux-x64  -o "C:\Publishes\VoloDocs\linux-x64\VoloDocs.Web" --self-contained false

cd..\VoloDocs.Migrator 

dotnet publish -c Release -r linux-x64 -o "C:\Publishes\VoloDocs\linux-x64\Migrator" --self-contained false
