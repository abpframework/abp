FROM mcr.microsoft.com/dotnet/sdk:10.0
COPY bin/Release/net10.0/publish/ app/
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "Bench.Web.dll"]