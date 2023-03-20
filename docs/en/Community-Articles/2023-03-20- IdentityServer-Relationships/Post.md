## Introduction
In this article, I will talk about the relationships of IdentityServer in every web application you create using ABP framework. When you read this article, you will learn how to extend the user entity in applications you develop using the ABP framework with a primitive type, extending the user by associating the user with another entity (User-many-to-one-X).


## Creating the Solution 
>For the source code of the application: https://github.com/onurpicakci/Abp-Identity-Relationship

In this article we will use EF Core as the database and MVC as the user interface. But Angular will also work on Blazor Server and Blazor WebAssembly. Abp Framework offers starter templates to get started faster. We can create a new starter template using the Abp CLI:

```
abp new IdentityRelationship
```

After the download is complete, you can run the `IdentityRelationship.DbMigrator` project to create database migrations. You can then run the `IdentityRelationship.Web` project to see our application running.

> Default admin username is **admin** and password is **1q2w3E\***

![solution-image](images/solution-image.png)

## Module Entity Extensions

Module entity extension system is a high level extension system that allows you to define new properties for existing entities of the depended modules. It automatically adds properties to the entity, database, HTTP API and the user interface in a single point.

### Extending the User Entity With a Primitive Type

Open the `IdentityRelationshipModuleExtensionConfigurator` class inside the `Domain.Shared` project of your solution and change the `ConfigureExtraPropertiesmethod` as shown below to add a `IdentificationNumber property` to the `IdentityUser` entity of the [Identity Module](https://docs.abp.io/en/abp/latest/Modules/Identity).

```csharp
public static void ConfigureExtraProperties()
{
    OneTimeRunner.Run(() =>
    {
        ObjectExtensionManager.Instance.Modules()
            .ConfigureIdentity(identity =>
            {
                identity.ConfigureUser(user =>
                {
                    user.AddOrUpdateProperty<string>( //property type: string
                        "IdentificationNumber", //property name
                        property =>
                        {
                            //validation rules
                            property.Attributes.Add(new RequiredAttribute());
                            property.Attributes.Add(
                                new StringLengthAttribute(64) {
                                    MinimumLength = 4
                                }
                            );

                            //...other configurations for this property
                        }
                    );
                });
            });
    });
}
```
> This method is called inside the IdentityRelationshipDomainSharedModule at the beginning of the application. OneTimeRunner is a utility class that guarantees to execute this code only one time per application, since multiple calls are unnecessary.

If you want to localize, open the `IdentityRelationship.Domain.Shared` project and create a new localization in your `/Localization/IdentityRelationship/en.json` file.

```json
 "IdentificationNumber": "Identification Number"
 ```
Once you define a property, it appears in the create and update forms of the related entity:

 ![identification-number](images/identification-number.png)

 New properties also appear in the data table of the related page:

 ![users-page](images/users-page.png)


## Navigation Properties / Foreign Keys

It is supported to add an extension property to an entity that is Id of another entity (foreign key).

### Example: Let's associate a department in the database with a user

First create a `Departments` folder in the `IdentityRelationship.Domain` project and add the `Department` class inside:
```csharp
using System;
using Volo.Abp.Domain.Entities;

namespace IdentityRelationship.Departments;

public class Department : AggregateRoot<Guid>
{
    public string Name { get; set; }
}
```

EF Core requires that you relate the entities with your `DbContext`. The easiest way to do so is adding a DbSet property to the `IdentityRelationshipDbContext` class in the `IdentityRelationship.EntityFrameworkCore` project, as shown below:

```csharp
    public DbSet<Department> Departments { get; set; }
```

Then in the `IdentityRelationship.EntityFrameworkCore` project of your solution, open the `/EntityFrameworkCore/IdentityRelationshipEfCoreEntityExtensionMappings` class and update your code:

```csharp
public static class IdentityRelationshipEfCoreEntityExtensionMappings
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        IdentityRelationshipGlobalFeatureConfigurator.Configure();
        IdentityRelationshipModuleExtensionConfigurator.Configure();

        OneTimeRunner.Run(() =>
        {
            ObjectExtensionManager.Instance
                .MapEfCoreProperty<IdentityUser, Guid>(
                    "DepartmentId",
                    (entityBuilder, propertyBuilder) => { propertyBuilder.HasMaxLength(128); }
                );
                
        });
    }
}
```

This class can be used to map these extra properties to table fields in the database. Please read [this](https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities?_ga=2.21022651.140118448.1679289046-1173891759.1672473062) document to improve your understanding of what we are doing.

We need to create a new migration to see the changes in the database. Open your `EntityFrameworkCore` project in the terminal (this depends on the IDE you are using).

![terminal-image](images/open-terminal.png)

```bash
dotnet ef migrations add Create_Department_Entity
```

Finally, run the `IdentityRelationship.DbMigrator` project to update the database.

When you look at your database, you can see that `Department` table has been added and `DepartmentId` has been added to your `AbpUsers` table.

![database-tables](images/database-tables.png)

![users-table](images/users-table.png)

Again open the `IdentityRelationshipModuleExtensionConfigurator` class in the Domain.Shared project and add the following code:

```csharp

user.AddOrUpdateProperty<Guid>(
    "DepartmentId",
    property =>
    {
        property.UI.Lookup.Url = "/api/app/departments";
        property.UI.Lookup.DisplayPropertyName = "name";
    }
);
```

`UI.Lookup.Url` option takes a URL to get list of departments to select on edit/create forms. This endpoint can be a typical controller, an auto API controller or any type of endpoint that returns a proper JSON response.

Create a `Departments` folder in the `IdentityRelationship.Application.Contracts` project of your solution and add the `DepartmentDto` class in it

```csharp
using System;
namespace IdentityRelationship.Departments;

public class DepartmentDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}
```
Now let's create an `IDepartmentAppService`interface in the `Departments` folder

```csharp
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IdentityRelationship.Departments;

public interface IDepartmentAppService : IApplicationService
{
    public  Task<ListResultDto<DepartmentDto>> GetAsync();
}
```

Time to implement the `IDepartmentAppService` interface. Create a `Departments` folder in your `IdentityRelationship.Application` project and add the `DepartmentAppService` class inside. 

```csharp
using System;
using System.Threading.Tasks;
using IdentityRelationship.Departments;
using Volo.Abp.Application.Dtos;

namespace IdentityRelationship.Department;

public class DepartmentAppService : IdentityRelationshipAppService, IDepartmentAppService
{
    public async Task<ListResultDto<DepartmentDto>> GetAsync()
    {
        return  new ListResultDto<DepartmentDto>(
            new[]
            {
                new DepartmentDto
                {
                    Id = Guid.Parse("6267f0df-870f-4173-be44-d74b4b56d2bd"),
                    Name = "Human Resources"
                },
                new DepartmentDto
                {
                    Id = Guid.Parse("21c7b61f-330c-489e-8b8c-80e0a78a5cc5"),
                    Name = "Production"
                }
            }
        );
    }
}
```
Run your `IdentityRelationship.Web` project and add a department to one of your users.

![user-department-image](images/user-department.png)

And shows the department name on the data table:

![users-page-department-image](images/users-page-department.png)

## Conclusion
In this article I talked about IdentityUser's relationships. Thank you for reading the article, I hope it was useful. See you soon!

## Preferences
- https://docs.abp.io/en/abp/latest/Module-Entity-Extensions
- https://learn.microsoft.com/en-us/ef/core/modeling/relationships








