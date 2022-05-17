FROM mcr.microsoft.com/dotnet/aspnet:6.0.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
COPY bin/Release/publish .
ENTRYPOINT ["dotnet", "Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.dll"]
