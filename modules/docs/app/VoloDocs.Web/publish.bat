@echo off

dotnet clean
dotnet restore
dotnet build

DEL /F/Q/S "C:\Publishes\VoloDocs" > NUL && RMDIR /Q/S "C:\Publishes\VoloDocs"

dotnet publish -c Release -r win-x64   --self-contained true -o "C:\Publishes\VoloDocs\win-x64\Web"
dotnet publish -c Release -r win-x86   --self-contained true -o "C:\Publishes\VoloDocs\win-x86\Web"
dotnet publish -c Release -r osx-x64   --self-contained true -o "C:\Publishes\VoloDocs\osx-x64\Web"
dotnet publish -c Release -r linux-x64 --self-contained true -o "C:\Publishes\VoloDocs\linux-x64\Web"

cd..\VoloDocs.Migrator 

dotnet publish -c Release -r win-x64   --self-contained true -o "C:\Publishes\VoloDocs\win-x64\Migrator"
dotnet publish -c Release -r win-x86   --self-contained true -o "C:\Publishes\VoloDocs\win-x86\Migrator"
dotnet publish -c Release -r osx-x64   --self-contained true -o "C:\Publishes\VoloDocs\osx-x64\Migrator"
dotnet publish -c Release -r linux-x64 --self-contained true -o "C:\Publishes\VoloDocs\linux-x64\Migrator"
