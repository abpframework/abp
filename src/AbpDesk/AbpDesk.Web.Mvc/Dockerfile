FROM microsoft/aspnetcore:2.0

WORKDIR /app
COPY . .

RUN mkdir /Web_PlugIns
RUN mv /app/PlugIns/*.* /Web_PlugIns

ENTRYPOINT ["dotnet", "AbpDesk.Web.Mvc.dll"]