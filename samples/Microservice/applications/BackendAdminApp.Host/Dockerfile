FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/runtime:2.2 AS build
WORKDIR /src
COPY . .
WORKDIR "/src/samples/Microservice/applications/BackendAdminApp.Host"
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release -o /app

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BackendAdminApp.Host.dll"]
