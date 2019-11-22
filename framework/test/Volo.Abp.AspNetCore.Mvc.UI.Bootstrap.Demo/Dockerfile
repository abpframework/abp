FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80
COPY bin/Release/publish .
ENTRYPOINT ["dotnet", "Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.dll"]
