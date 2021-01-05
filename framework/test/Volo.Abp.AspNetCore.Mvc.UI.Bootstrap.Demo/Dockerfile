FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
COPY bin/Release/publish .
ENTRYPOINT ["dotnet", "Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.dll"]
