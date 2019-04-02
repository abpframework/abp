FROM microsoft/dotnet:2.2-sdk-alpine AS build
RUN apk add --no-cache bash
COPY . .

WORKDIR /templates/service/host/IdentityServerHost
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release

WORKDIR /templates/service/host/MyCompanyName.MyProjectName.Host
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release

FROM build AS final
WORKDIR /src
COPY --from=build /templates/service/host/IdentityServerHost ./IdentityServerHost
COPY --from=build /templates/service/host/MyCompanyName.MyProjectName.Host ./MyCompanyName.MyProjectName.Host
COPY --from=build /templates/service/database/entrypoint.sh .
RUN /bin/bash -c "sed -i $'s/\r$//' entrypoint.sh"
RUN chmod +x ./entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]