# Web Application Development Tutorial - Part 6: Authors
````json
//[doc-params]
{
    "UI": ["MVC","NG"],
    "DB": ["EF","Mongo"]
}
````
{{
if UI == "MVC"
  UI_Text="mvc"
else if UI == "NG"
  UI_Text="angular"
else
  UI_Text="?"
end
if DB == "EF"
  DB_Text="Entity Framework Core"
else if DB == "Mongo"
  DB_Text="MongoDB"
else
  DB_Text="?"
end
}}

## About This Tutorial

In this tutorial series, you will build an ABP based web application named `Acme.BookStore`. This application is used to manage a list of books and their authors. It is developed using the following technologies:

* **{{DB_Text}}** as the ORM provider. 
* **{{UI_Value}}** as the UI Framework.

This tutorial is organized as the following parts;

- [Part 1: Creating the server side](Part-1.md)
- [Part 2: The book list page](Part-2.md)
- [Part 3: Creating, updating and deleting books](Part-3.md)
- [Part 4: Integration tests](Part-4.md)
- [Part 5: Authorization](Part-5.md)
- **Part 6: The author entity (this part)**

### Download the Source Code

This tutorials has multiple versions based on your **UI** and **Database** preferences. We've prepared two combinations of the source code to be downloaded:

* [MVC (Razor Pages) UI with EF Core](https://github.com/abpframework/abp-samples/tree/master/BookStore-Mvc-EfCore)
* [Angular UI with MongoDB](https://github.com/abpframework/abp-samples/tree/master/BookStore-Angular-MongoDb)

## Introduction

In the previous parts, we've used the ABP infrastructure to easily build some services;

* Used the [CrudAppService](../Application-Services.md) base class instead of manually developing an application service for standard create, read, update and delete operations.
* Used [generic repositories](../Repositories.md) to completely automate the database layer.
* Used [conventional API controllers](../API/Auto-API-Controllers.md) instead of manually writing API controllers.

For the "Authors" part, we will do most of the things manually to show how you can do it in case of need.

## The Author Entity

Create an `Authors` folder (namespace) in the `Acme.BookStore.Domain` project and add an `Author` class inside it:

````csharp
using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Authors
{
    public class Author : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; internal set; }

        public DateTime BirthDate { get; set; }

        public string ShortBio { get; set; }
    }
}
````

* Inherited from `FullAuditedAggregateRoot<Guid>` which makes the entity [soft delete](../Data-Filtering.md) (that means when you delete it, it is not deleted in the database, but just marked as deleted) with all the [auditing](../Entities.md) properties.
* `internal set` for the `Name` property restricts to set this property from out of the domain layer. Because we want to change it in a controlled way in the domain layer to prevent to create two authors with the same name, to just demonstrate a simple business rule.