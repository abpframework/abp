FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
COPY bin/Release/publish .
ENTRYPOINT ["dotnet", "Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo.dll"]
