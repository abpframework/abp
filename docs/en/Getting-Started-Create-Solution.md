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

## Create a New Project

Use the `new` command of the ABP CLI to create a new project:

````shell
abp new Acme.BookStore{{if UI == "NG"}} -u angular{{else if UI == "Blazor"}} -u blazor{{end}}{{if DB == "Mongo"}} -d mongodb{{end}}{{if Tiered == "Yes"}}{{if UI == "MVC"}} --tiered{{else}} --separate-identity-server{{end}}{{end}}
````

*You can use different level of namespaces; e.g. BookStore, Acme.BookStore or Acme.Retail.BookStore.* 

{{ if Tiered == "Yes" }}

{{ if UI == "MVC" }}

* `--tiered` argument is used to create N-tiered solution where authentication server, UI and API layers are physically separated.

{{ else }}

* `--separate-identity-server` argument is used to separate the identity server application from the API host application. If not specified, you will have a single endpoint on the server.

{{ end }}

{{ end }}

> [ABP CLI document](./CLI.md) covers all of the available commands and options.

> Alternatively, you can **create and download** projects from [ABP Framework website](https://abp.io/get-started) by easily selecting the all the options from the page.

### The Solution Structure

The solution has a layered structure (based on the [Domain Driven Design](Domain-Driven-Design.md)) and contains unit & integration test projects. See the [application template document](Startup-Templates/Application.md) to understand the solution structure in details. 

{{ if DB == "Mongo" }}

#### MongoDB Transactions

The [startup template](Startup-templates/Index.md) **disables** transactions in the `.MongoDB` project by default. If your MongoDB server supports transactions, you can enable the it in the *YourProjectMongoDbModule* class's `ConfigureServices` method:

  ```csharp
Configure<AbpUnitOfWorkDefaultOptions>(options =>
{
    options.TransactionBehavior = UnitOfWorkTransactionBehavior.Auto;
});
  ```

> Or you can delete that code since `Auto` is already the default behavior.

{{ end }}

## Next Step

* [Running the solution](Getting-Started-Running-Solution.md)