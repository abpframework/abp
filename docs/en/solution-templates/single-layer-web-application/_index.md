```json
//[doc-seo]
{
    "Description": "Explore the Single Layer Application Solution Template to learn how to create a straightforward .NET solution structure with ease."
}
```

# Single Layer Application Solution Template

This template provides a simple solution structure with a single project. This document explains that solution structure in details.

## Getting Started

* Follow the [Getting Started guide](../../get-started/single-layer-web-application.md) to create a new solution using this startup solution template.
* Follow the [TODO application tutorial](../../tutorials/todo/single-layer/index.md) to learn how to create a simple application with this startup solution template.

## The Solution Structure

If you created your solution with the default options, you will have a .NET solution as shown below:

![](../../images/bookstore-single-layer-solution-structure.png)

In the next sections, we will explain the structure based on this example. Your startup solution can be slightly different based on your preferences.

### Folder Structure

Since this template provides a single-project solution, we've separated concerns into folders instead of projects. You can see the pre-defined folders as shown below:

![](../../images/single-layer-folder-structure.png)

* Define your database mappings (for [EF Core](../../framework/data/entity-framework-core) or [MongoDB](../../framework/data/mongodb) and [repositories](../../framework/architecture/domain-driven-design/repositories.md) in the `Data` folder.
* Define your [entities](../../framework/architecture/domain-driven-design/entities.md) in the `Entities` folder.
* Define your UI localization keys/values in the `Localization` folder.
* Define your UI menu items in the `Menus` folder.
* Define your [object-to-object mapping](../../framework/infrastructure/object-to-object-mapping.md) classes in the `ObjectMapping` folder.
* Define your UI pages (Razor Pages) in the `Pages` folder (create `Controllers` and `Views` folder yourself if you prefer the MVC pattern).
* Define your [application services](../../framework/architecture/domain-driven-design/application-services.md) in the `Services` folder. 

### How to Run?

Before running the application, you need to create the database and seed the initial data. To do that, you can run the following command in the directory of your project (in the same folder of the `.csproj` file):

```bash
dotnet run --migrate-database
```

This command will create the database and seed the initial data for you. Then you can run the application with any IDE that supports .NET or by running the `dotnet run` command in the directory of your project. The default username is `admin` and the password is `1q2w3E*`.

> While creating a database & applying migrations seem only necessary for relational databases, you should run this command even if you choose a NoSQL database provider (like MongoDB). In that case, it still seeds the initial data which is necessary for the application.

### The Angular UI 

If you choose `Angular` as the UI framework, the solution will be separated into two folders:

* An `angular` folder that contains the Angular UI application, the client-side code.
* An `aspnet-core` folder that contains the ASP.NET Core solution (a single project), the server-side code.

The server-side is similar to the solution described in the *Solution Structure* section above. This project serves the API, so the Angular application can consume it.

The client-side application consumes the HTTP APIs as mentioned. You can see the folder structure of the Angular project shown below:

![](../../images/single-layer-angular-folder-structure.png)

  ## See Also

* [Video tutorial](https://abp.io/video-courses/essentials/app-template)