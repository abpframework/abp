# Contribution Guide for the Angular UI

## Pre-requirements

- Dotnet core SDK https://dotnet.microsoft.com/en-us/download
- Nodejs LTS https://nodejs.org/en/
- Docker https://docs.docker.com/engine/install
- Angular CLI. https://angular.io/guide/what-is-angular#angular-cli
- Abp CLI https://docs.abp.io/en/abp/latest/cli
- A code editor

Note: This article prepare Windows OS. You may change the path type of your OS.

Examples:

* Windows: `templates\app\aspnet-core\src\MyCompanyName.MyProjectName.DbMigrator\appsettings.json`
* Unix: `templates/app/aspnet-core/src/MyCompanyName.MyProjectName.DbMigrator/appsettings.json`

## Sample docker commands

You need to install SQL Server and Redis. You can install these programs without docker, but my example uses docker containers. Your computer should have Docker Engine. Then open the terminal and execute the commands one by one.
For the SQL Server

```cmd
docker run -v sqlvolume:/var/opt/mssql -e 'ACCEPT_EULA=Y' -e "SA_PASSWORD=yourpassword" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU3-ubuntu-18.04
```

For the Redis

```cmd
docker run -p 6379:6379 -d redis
```

Then we are ready to download and execute the code.

## Folder Structure

The app has a backend written in .net core (c#) and an angular app. It would help if you ran both of them.

## Running Backend App

The path of the Backend app is “templates\app\aspnet-core.” If you want to work with dockerized SQL Server, you should change connection strings for running with docker. The path of the connection string is
`templates\app\aspnet-core\src\MyCompanyName.MyProjectName.DbMigrator\appsettings.json`.

Before running the backend, you should run the Db migrator project. The DbMigrator created initial tables and values. The path of DbMigrator is `templates\app\aspnet-core\src\MyCompanyName.MyProjectName.DbMigrator`. Open a terminal in the path and execute the command `dotnet run` in terminal

One last step before the running the backend is installing client-side libraries. Go to `templates\app\aspnet-core`. Open a terminal in the path and execute the command `abp install-libs` in terminal

Next step you should go to path of backend host project. The path is `templates\app\aspnet-core\src\MyCompanyName.MyProjectName.HttpApi.HostWithIds`. Open a terminal in the path and execute the command `dotnet run` in terminal

Your backend should be running successfully

## Running Frontend App

There is a demo app. The path of the demo app is `npm\ng-packs\apps\dev-app`. The demo app is connected to the packages with local references. Open the terminal in `npm\ng-packs\apps\dev-app` and execute `yarn` or `npm i` in terminal. After the package installed run `npm start` or `yarn start`.

The repo uses Nx and packages connected with `local references`. The packages path is `npm\ng-packs\packages`
