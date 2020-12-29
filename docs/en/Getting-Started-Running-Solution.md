# Getting Started

````json
//[doc-params]
{
    "UI": ["MVC", "Blazor", "NG"],
    "DB": ["EF", "Mongo"],
    "Tiered": ["Yes", "No"]
}
````

> This document assumes that you prefer to use **{{ UI_Value }}** as the UI framework and **{{ DB_Value }}** as the database provider. For other options, please change the preference on top of this document.

## Create the Database

### Connection String

Check the **connection string** in the `appsettings.json` file under the {{if Tiered == "Yes"}}`.IdentityServer` and `.HttpApi.Host` projects{{else}}{{if UI=="MVC"}}`.Web` project{{else}}`.HttpApi.Host` project{{end}}{{end}}

{{ if DB == "EF" }}

````json
"ConnectionStrings": {
  "Default": "Server=localhost;Database=BookStore;Trusted_Connection=True"
}
````

The solution is configured to use **Entity Framework Core** with **MS SQL Server** by default. EF Core supports [various](https://docs.microsoft.com/en-us/ef/core/providers/) database providers, so you can use any supported DBMS. See [the Entity Framework integration document](Entity-Framework-Core.md) to learn how to [switch to another DBMS](Entity-Framework-Core-Other-DBMS.md).

### Apply the Migrations

The solution uses the [Entity Framework Core Code First Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli). So, you need to apply migrations to create the database. There are two ways of applying the database migrations.

#### Apply Migrations Using the DbMigrator

The solution comes with a `.DbMigrator` console application which applies migrations and also **seeds the initial data**. It is useful on **development** as well as on **production** environment.

> `.DbMigrator` project has its own `appsettings.json`. So, if you have changed the connection string above, you should also change this one. 

Right click to the `.DbMigrator` project and select **Set as StartUp Project**

![set-as-startup-project](images/set-as-startup-project.png)

 Hit F5 (or Ctrl+F5) to run the application. It will have an output like shown below:

 ![db-migrator-output](images/db-migrator-output.png)

> Initial [seed data](Data-Seeding.md) creates the `admin` user in the database (with the password is `1q2w3E*`) which is then used to login to the application. So, you need to use `.DbMigrator` at least once for a new database.

#### Using EF Core Update-Database Command

Ef Core has `Update-Database` command which creates database if necessary and applies pending migrations.

{{ if UI == "MVC" }}

Right click to the {{if Tiered == "Yes"}}`.IdentityServer`{{else}}`.Web`{{end}} project and select **Set as StartUp project**: 

{{ else if UI != "MVC" }}

Right click to the `.HttpApi.Host` project and select **Set as StartUp Project**: 

{{ end }}

![set-as-startup-project](images/set-as-startup-project.png)

Open the **Package Manager Console**, select `.EntityFrameworkCore.DbMigrations` project as the **Default Project** and run the `Update-Database` command:

![package-manager-console-update-database](images/package-manager-console-update-database.png)

This will create a new database based on the configured connection string.

> **Using the `.DbMigrator` tool is the suggested way**, because it also seeds the initial data to be able to properly run the web application.
>
> If you just use the `Update-Database` command, you will have an empty database, so you can not login to the application since there is no initial admin user in the database. You can use the `Update-Database` command in development time when you don't need to seed the database. However, using the `.DbMigrator` application is easier and you can always use it to migrate the schema and seed the database.

{{ else if DB == "Mongo" }}

````json
"ConnectionStrings": {
  "Default": "mongodb://localhost:27017/BookStore"
}
````

The solution is configured to use **MongoDB** in your local computer, so you need to have a MongoDB server instance up and running or change the connection string to another MongoDB server.

### Seed Initial Data

The solution comes with a `.DbMigrator` console application which **seeds the initial data**. It is useful on **development** as well as on **production** environment.

> `.DbMigrator` project has its own `appsettings.json`. So, if you have changed the connection string above, you should also change this one. 

Right click to the `.DbMigrator` project and select **Set as StartUp Project**

![set-as-startup-project](images/set-as-startup-project.png)

 Hit F5 (or Ctrl+F5) to run the application. It will have an output like shown below:

 ![db-migrator-output](images/db-migrator-output.png)

> Initial [seed data](Data-Seeding.md) creates the `admin` user in the database (with the password is `1q2w3E*`) which is then used to login to the application. So, you need to use `.DbMigrator` at least once for a new database.

{{ end }}

## Run the Application

{{ if UI == "MVC" }}

{{ if Tiered == "Yes" }}

> Tiered solutions use **Redis** as the distributed cache. Ensure that it is installed and running in your local computer. If you are using a remote Redis Server, set the configuration in the `appsettings.json` files of the projects below.

1. Ensure that the `.IdentityServer` project is the startup project. Run this application that will open a **login** page in your browser.

> Use Ctrl+F5 in Visual Studio (instead of F5) to run the application without debugging. If you don't have a debug purpose, this will be faster.

You can login, but you cannot enter to the main application here. This is **just the authentication server**.

2. Ensure that the `.HttpApi.Host` project is the startup project and run the application which will open a **Swagger UI** in your browser.

![swagger-ui](images/swagger-ui.png)

This is the HTTP API that is used by the web application.

3. Lastly, ensure that the `.Web` project is the startup project and run the application which will open a **welcome** page in your browser

![mvc-tiered-app-home](images/bookstore-home.png)

Click to the **login** button which will redirect you to the *authentication server* to login to the application:

![bookstore-login](images/bookstore-login.png)

{{ else # Tiered != "Yes" }}

Ensure that the `.Web` project is the startup project. Run the application which will open the **login** page in your browser:

> Use Ctrl+F5 in Visual Studio (instead of F5) to run the application without debugging. If you don't have a debug purpose, this will be faster.

![bookstore-login](images/bookstore-login.png)

{{ end # Tiered }}

{{ else # UI != "MVC" }}

### Running the HTTP API Host (Server Side)

{{ if Tiered == "Yes" }}

> Tiered solutions use Redis as the distributed cache. Ensure that it is installed and running in your local computer. If you are using a remote Redis Server, set the configuration in the `appsettings.json` files of the projects below.

Ensure that the `.IdentityServer` project is the startup project. Run the application which will open a **login** page in your browser.

> Use Ctrl+F5 in Visual Studio (instead of F5) to run the application without debugging. If you don't have a debug purpose, this will be faster.

You can login, but you cannot enter to the main application here. This is just the authentication server.

Ensure that the `.HttpApi.Host` project is the startup project and run the application which will open a Swagger UI:

{{ else # Tiered == "No" }}

Ensure that the `.HttpApi.Host` project is the startup project and run the application which will open a Swagger UI:

> Use Ctrl+F5 in Visual Studio (instead of F5) to run the application without debugging. If you don't have a debug purpose, this will be faster.

{{ end # Tiered }}

![swagger-ui](images/swagger-ui.png)

You can see the application APIs and test them here. Get [more info](https://swagger.io/tools/swagger-ui/) about the Swagger UI.

{{ end # UI }}

{{ if UI == "Blazor" }}

### Running the Blazor Application (Client Side)

Ensure that the `.Blazor` project is the startup project and run the application.

> Use Ctrl+F5 in Visual Studio (instead of F5) to run the application without debugging. If you don't have a debug purpose, this will be faster.

Once the application starts, click to the **Login** link on to header, which redirects you to the authentication server to enter a username and password:

![bookstore-login](images/bookstore-login.png)

{{ else if UI == "NG" }}

### Running the Angular Application (Client Side)

Go to the `angular` folder, open a command line terminal, type the `yarn` command (we suggest to the [yarn](https://yarnpkg.com/) package manager while `npm install` will also work)

```bash
yarn
```

Once all node modules are loaded, execute `yarn start` (or `npm start`) command:

```bash
yarn start
```

It may take a longer time for the first build. Once it finishes, it opens the Angular UI in your default browser with the [localhost:4200](http://localhost:4200/) address.

![bookstore-login](images/bookstore-login.png)

{{ end }}

Enter **admin** as the username and **1q2w3E*** as the password to login to the application. The application is up and running. You can start developing your application based on this startup template.

## Mobile Development

If you want to include a [React Native](https://reactnative.dev/) project in your solution, add `-m react-native` (or `--mobile react-native`) argument to project creation command. This is a basic React Native startup template to develop mobile applications integrated to your ABP based backends.

See the [Getting Started with the React Native](Getting-Started-React-Native.md) document to learn how to configure and run the React Native application.

## See Also

* [Web Application Development Tutorial](Tutorials/Part-1.md)
* [Application Startup Template](Startup-Templates/Application.md)
