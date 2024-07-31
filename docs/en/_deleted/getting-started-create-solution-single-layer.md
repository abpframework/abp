# Getting Started

````json
//[doc-params]
{
    "UI": ["MVC", "Blazor", "BlazorServer", "NG"],
    "DB": ["EF", "Mongo"]
}
````

````json
//[doc-nav]
{
  "Next": {
    "Name": "Running the solution",
    "Path": "Getting-Started-Running-Solution-Single-Layer"
  },
  "Previous": {
    "Name": "Setup Your Development Environment",
    "Path": "Getting-Started-Setup-Environment-Single-Layer"
  }
}
````

> This document assumes that you prefer to use **{{ UI_Value }}** as the UI framework and **{{ DB_Value }}** as the database provider. For other options, please change the preference on top of this document.

## Create a New Project

We will use the ABP CLI to create a new ABP project.

> You can also use the ABP CLI Command Generator on the [ABP website](https://abp.io/get-started) by easily selecting all options from the page.

Use the `new` command of the ABP CLI to create a new project:

````shell
abp new Acme.BookStore -t app-nolayers{{if UI == "NG"}} -u angular{{else if UI == "Blazor"}} -u blazor{{else if UI == "BlazorServer"}} -u blazor-server{{end}}{{if DB == "Mongo"}} -d mongodb{{end}}
````

*You can use different level of namespaces; e.g. BookStore, Acme.BookStore or Acme.Retail.BookStore.* 

> [ABP CLI document](../cli/index.md) covers all of the available commands and options.

## The Solution Structure

The solution structure is based on the [Single-Layer Startup Template](../solution-templates/single-layer-web-application) where everything is in one project instead of the [Domain Driven Design](../framework/architecture/domain-driven-design). You can check its [documentation](../solution-templates/single-layer-web-application) for more details.

{{ if DB == "Mongo" }}

## MongoDB Transactions

The [startup template](../solution-templates) **disables** transactions in the `.MongoDB` project by default. If your MongoDB server supports transactions, you can enable it in the *YourProjectModule* class's `ConfigureMongoDB` method:

  ```csharp
Configure<AbpUnitOfWorkDefaultOptions>(options =>
{
	options.TransactionBehavior = UnitOfWorkTransactionBehavior.Enabled; //or UnitOfWorkTransactionBehavior.Auto
});
  ```

> Or you can delete that code since `Auto` is already the default behavior.

{{ end }}