```json
//[doc-seo]
{
    "Description": "Learn how to set up and contribute to Angular UI in the ABP Framework with essential tools, commands, and guidelines for developers."
}
```

# Contribution Guide for the Angular UI

This guide explains how to set up the ABP Angular UI workspace, run the demo app, and prepare your environment to contribute UI changes. It assumes that you are already familiar with basic Angular and .NET development.

> Before sending a pull request for Angular UI changes, please also read the main [Contribution Guide](index.md).

## Pre-requirements

Make sure you have the following tools installed:

- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- [Node.js LTS](https://nodejs.org/en/) (recommended: use the version supported by the Angular CLI used in this repository)
- [Docker Engine](https://docs.docker.com/engine/install/) (required if you use the sample SQL Server and Redis containers)
- [Angular CLI](https://angular.dev/tools/cli)
- [ABP CLI](https://docs.abp.io/en/abp/latest/cli)
- A code editor (for example, Visual Studio Code or Visual Studio)

> This article uses Windows-style paths in examples. On Unix-like systems, replace backslashes (`\`) with forward slashes (`/`).

Examples:

- Windows: `templates\app\aspnet-core\src\MyCompanyName.MyProjectName.DbMigrator\appsettings.json`
- Unix: `templates/app/aspnet-core/src/MyCompanyName.MyProjectName.DbMigrator/appsettings.json`

## Sample Docker Commands

You need SQL Server and Redis. You can install these programs without Docker, but the examples below use Docker containers. Your computer should have Docker Engine running. Then open a terminal and execute the commands.

### SQL Server

```bash
docker run -v sqlvolume:/var/opt/mssql \
  -e 'ACCEPT_EULA=Y' \
  -e 'SA_PASSWORD=YourStrong!Passw0rd' \
  -p 1433:1433 \
  -d mcr.microsoft.com/mssql/server:2019-CU3-ubuntu-18.04
```

- Replace `YourStrong!Passw0rd` with a strong password that satisfies SQL Server password requirements.
- The `sqlvolume` named volume is used to persist database files.

### Redis

```bash
docker run -p 6379:6379 -d redis:latest
```

After running the commands, you can use `docker ps` to verify that both containers are running.

Once the containers are ready, you can download the ABP source code and run the apps.

## Folder Structure

The sample application has:

- A backend built with ASP.NET Core (C#).
- An Angular workspace managed by Nx.

You will run both the backend and the Angular dev app during development.

## Running the Backend App

The backend root path is `templates\app\aspnet-core`.

### 1. Configure the Connection Strings

If you are using the Dockerized SQL Server, update the connection strings to point to your Docker container. The configuration file is:

- `templates\app\aspnet-core\src\MyCompanyName.MyProjectName.DbMigrator\appsettings.json`

Ensure that the connection string uses the correct server name (`localhost,1433` by default), user (`sa`), and your password.

### 2. Run the DbMigrator

The DbMigrator project creates the initial database schema and seed data.

```bash
cd templates/app/aspnet-core/src/MyCompanyName.MyProjectName.DbMigrator
dotnet run
```

Wait until the migration completes successfully.

### 3. Install Client-side Libraries

Before running the backend host, install the client-side libraries:

```bash
cd templates/app/aspnet-core
abp install-libs
```

This command restores the required client-side libraries for the backend.

### 4. Run the Backend Host

Go to the backend HTTP API host project folder. The exact project name may differ based on your template, but it will be similar to:

- `templates\app\aspnet-core\src\MyCompanyName.MyProjectName.HttpApi.HostWithIds`

Run the host:

```bash
cd templates/app/aspnet-core/src/MyCompanyName.MyProjectName.HttpApi.HostWithIds
dotnet run
```

After it starts, the backend API will be available on a localhost URL defined in the project (for example, `https://localhost:44305`, depending on your template).

## Running the Frontend (Angular Dev App)

The Angular workspace is under `npm\ng-packs`. It is an Nx workspace that contains both the dev app and the Angular UI packages.

- Dev app path: `npm\ng-packs\apps\dev-app`
- Package path: `npm\ng-packs\packages\`

The dev app uses local references to the packages under `packages`, so your library changes will be reflected immediately while the dev server is running.

### 1. Install Dependencies

From the dev app folder:

```bash
cd npm/ng-packs/apps/dev-app
yarn
# or, if you prefer npm:
# npm install
```

Choose one package manager (preferably `yarn` if that is what the repository uses) and stick with it.

### 2. Start the Dev Server

```bash
yarn start
# or:
# npm start
```

This will start the Angular dev server (via Nx) and open the dev app in your browser. Ensure that the backend API is running so the dev app can connect to it.

## Typical Contribution Workflow

1. Start SQL Server and Redis (for example, using the Docker commands above).
2. Run DbMigrator to create and seed the database.
3. Run `abp install-libs` and start the backend HTTP API host.
4. Install dependencies and start the Angular dev app.
5. Make changes in the Angular UI packages under `npm\ng-packs\packages\`.
6. Run any relevant tests for the affected packages (for example, via Nx).
7. Commit your changes and open a pull request on GitHub, referencing the related issue.

## Troubleshooting

- **Backend cannot connect to SQL Server**
  - Check that the SQL Server container is running (`docker ps`).
  - Verify the connection string server/port and `SA_PASSWORD` value.
- **Angular app cannot reach the backend API**
  - Confirm that the backend host is running and listening on the expected URL.
  - Check the API base URL configuration in the dev app’s environment files.
- **Node or package manager version issues**
  - Use an LTS version of Node.js.
  - Consider using a version manager (like `nvm`) to match the version used in the project.

## See Also

- [Contribution Guide](index.md)
- [ABP CLI](https://docs.abp.io/en/abp/latest/cli)
