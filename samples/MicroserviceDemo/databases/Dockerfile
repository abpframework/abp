FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
RUN apk add --no-cache bash
WORKDIR /src
COPY . .

WORKDIR "/src/samples/MicroserviceDemo/microservices/ProductService.Host"
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release

WORKDIR "/src/samples/MicroserviceDemo/applications/AuthServer.Host"
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release

FROM build AS final
WORKDIR /src
COPY --from=build /src/samples/MicroserviceDemo/microservices/ProductService.Host ./ProductService.Host
COPY --from=build /src/samples/MicroserviceDemo/applications/AuthServer.Host ./AuthServer.Host
COPY --from=build /src/samples/MicroserviceDemo/databases/entrypoint.sh .
RUN /bin/bash -c "sed -i $'s/\r$//' entrypoint.sh"
RUN chmod +x ./entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]
