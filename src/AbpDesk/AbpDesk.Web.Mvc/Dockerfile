FROM microsoft/aspnetcore

WORKDIR /app
COPY . .

RUN mkdir /Web_PlugIns
RUN mv /app/PlugIns/*.* /Web_PlugIns

ENTRYPOINT ["dotnet", "AbpDesk.Web.Mvc.dll"]