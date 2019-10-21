## Getting Started With the Angular Application Template

This tutorial explain how to create a new Angular application using the startup template, configure and run it.

### Creating a New Project

This tutorial uses **ABP CLI** to create a new project. See the [Get Started](https://abp.io/get-started) page for other options.

Install the ABP CLI using a command line window, if you've not installed before:

````bash
dotnet tool install -g Volo.Abp.Cli
````

Use `abp new` command in an empty folder to create your project:

````bash
abp new Acme.BookStore -u angular
````

> You can use different level of namespaces; e.g. BookStore, Acme.BookStore or Acme.Retail.BookStore.

`-u angular` option specifies the UI framework to be Angular. Default database provider is EF Core. See the [CLI documentation](CLI.md) for all available options.

#### Pre Requirements

The created solution requires;

* [Visual Studio 2017 (v15.9.0+)](https://visualstudio.microsoft.com/tr/downloads/)
* [.NET Core 2.2+](https://www.microsoft.com/net/download/dotnet-core/)
* [Node v10.16+](https://nodejs.org)
* [Yarn v1.17+](https://yarnpkg.com/)

### The Solution Structure

Open the solution in **Visual Studio**:

![bookstore-visual-studio-solution](images/bookstore-visual-studio-solution-for-spa.png)

The solution has a layered structure (based on [Domain Driven Design](Domain-Driven-Design.md)) and contains unit & integration test projects properly configured to work with **EF Core** & **SQLite in-memory** database.

> See the [Application Template Document](Startup-Templates/Application.md) to understand the solution structure in details.

### Database Connection String

Check the **connection string** in the `appsettings.json` file under the `.HttpApi.Host` project:

````json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=BookStore;Trusted_Connection=True"
  }
}
````

The solution is configured to use **Entity Framework Core** with **MS SQL Server**. EF Core supports [various](https://docs.microsoft.com/en-us/ef/core/providers/) database providers, so you can use another DBMS if you want. Change the connection string if you need.

### Create Database & Apply Database Migrations

You have two options to create the database.

#### Using the DbMigrator Application

The solution contains a console application (named `Acme.BookStore.DbMigrator` in this sample) that can create database, apply migrations and seed initial data. It is useful on development as well as on production environment.

> `.DbMigrator` project has its own `appsettings.json`. So, if you have changed the connection string above, you should also change this one.

Right click to the `.DbMigrator` project and select **Set as StartUp Project**:

![set-as-startup-project](images/set-as-startup-project.png)

Hit F5 (or Ctrl+F5) to run the application. It will have an output like shown below:

![set-as-startup-project](images/db-migrator-app.png)

#### Using EF Core Update-Database Command

Ef Core has `Update-Database` command which creates database if necessary and applies pending migrations. Right click to the `.HttpApi.Host` project and select **Set as StartUp Project**:

![set-as-startup-project](images/set-as-startup-project.png)

Open the **Package Manager Console**, select `.EntityFrameworkCore.DbMigrations` project as the **Default Project** and run the `Update-Database` command:

![pcm-update-database](images/pcm-update-database-v2.png)

This will create a new database based on the configured connection string.

> Using the `.DbMigrator` tool is the suggested way, because it also seeds the initial data to be able to properly run the web application.

### Running the Application

#### Run the API Host (Server Side)

Ensure that the `.HttpApi.Host` project is the startup project and run the application which will open a Swagger UI:

![bookstore-homepage](images/bookstore-swagger-ui-host.png)

You can see the application APIs and test them here. Get [more info](https://swagger.io/tools/swagger-ui/) about the Swagger UI.

##### Authorization for the Swagger UI

Most of the application APIs require authentication & authorization. If you want to test authorized APIs, manually go to the `/Account/Login` page, enter `admin` as the username and `1q2w3E*` as the password to login to the application. Then you will be able to execute authorized APIs too.

#### Run the Angular Application (Client Side)

Go to the `angular` folder, open a command line terminal, type the `yarn` command (we suggest to the [yarn](https://yarnpkg.com) package manager while npm install will also work in most cases):

````bash
yarn
````

Once all node modules are loaded, execute `yarn start` or `npm start` command:

````bash
yarn start
````

Open your favorite browser and go to `localhost:4200` URL. Initial username is `admin` and password is `1q2w3E*`.

The startup template includes the **identity management** and **tenant management** modules. Once you login, the Administration menu will be available where you can manage **tenants**, **roles**, **users** and their **permissions**.

> We recommend [Visual Studio Code](https://code.visualstudio.com/) as the editor for the Angular project, but you are free to use your favorite editor.

### What's Next?

* [Application development tutorial](Tutorials/Angular/Part-I.md)
