# Building the Ordering Module

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Creating the Initial Ordering Module",
    "Path": "tutorials/modular-crm/part-04"
  }
}
````

In the previous part, we've created a custom Ordering module and installed it into the main application. However, the Ordering module has no functionality now. In this part, we will create an `Order` entity and add functionality to create and list the orders.

## Creating an `Order` Entity

Open the `ModularCrm.Ordering` .NET solution in your IDE.

> Tip: You can open the folder of a module's .NET solution by right-clicking the related module in ABP Studio, selecting the *Open with* -> *Explorer* command.

The following figure shows the `ModularCrm.Ordering` module in the *Solution Explorer* panel of Visual Studio:

![visual-studio-ordering-module-initial](images/visual-studio-ordering-module-initial.png)

### Adding `Volo.Abp.Ddd.Domain` Package Reference

As you see in the preceding figure, the solution structure is very minimal. It also has a minimal ABP dependency. ABP Framework has [hundreds of NuGet packages](https://abp.io/packages), but the `ModularCrm.Ordering` project only has the [`Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared`](https://abp.io/package-detail/Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared) package reference:

![visual-studio-ordering-ui-package-dependency](images/visual-studio-ordering-ui-package-dependency.png)

We will create an [entity class](../../framework/architecture/domain-driven-design/entities.md) and ABP defines base entity classes and other related infrastructure in the [`Volo.Abp.Ddd.Domain`](https://abp.io/package-detail/Volo.Abp.Ddd.Domain) package. So, we need to add a reference to that NuGet package.

You can add that package and arrange the [module](../../framework/architecture/modularity/basics.md) class dependency manually. However, here we will use ABP Studio as a more practical way.

Return to ABP Studio, right-click the `ModularCrm.Ordering` package in the *Solution Explorer* and select the *Add Package Reference* command:

![abp-studio-add-package-reference-2](images/abp-studio-add-package-reference-2.png)

That command opens a dialog to add a new package reference:

![abp-studio-add-nuget-package-reference](images/abp-studio-add-nuget-package-reference.png)

Select the *NuGet* tab, type `Volo.Abp.Ddd.Domain` as the *Package name* and write the version of the package you want to install. Please be sure that you are installing exactly the same version with the other ABP packages you are already using. In future versions, ABP Studio will provide an easier way to select the package and its version.

Click the *OK* button and then you can check the *Packages* under the `ModularCrm.Ordering` module *Dependencies* to see the `Volo.Abp.Ddd.Domain` package is installed:

![abp-studio-added-ddd-domain-package](images/abp-studio-added-ddd-domain-package.png)

### Adding an `Order` Class

Now, you can return your IDE and add an `Order` class to the `ModularCrm.Ordering` project (open an `Entities` folder and place the `Order.cs` into that folder):

````csharp
using System;
using Volo.Abp.Domain.Entities;
using ModularCrm.Ordering.Contracts.Enums;

namespace ModularCrm.Ordering.Entities
{
    public class Order : AggregateRoot<Guid>
    {
        public Guid ProductId { get; set; }
        public string CustomerName { get; set; }
        public OrderState State { get; set; }
    }
}
````

We are allowing to place only a single product within an order. In a real-world application, the `Order` entity would be much more complex. However, the complexity of the `Order` entity doesn't effect modularity, so we keep it simple to focus on modularity in this tutorial.

### Adding an `OrderState` Enumeration

We used an `OrderState` enumeration that is not defined yet. Open an `Enums` folder in the `ModularCrm.Ordering.Contracts` project and create an `OrderState.cs` file inside it:

````csharp
namespace ModularCrm.Ordering.Contracts.Enums;

public enum OrderState : byte
{
    Placed = 0,
    Delivered = 1,
    Canceled = 2
}
````

Final structure of the Ordering module should be similar to the following figure in your IDE:

![visual-studio-order-entity](images/visual-studio-order-entity.png)

## Configuring the Database Mapping

The `Order` entity has been created. Now, we need to configure the database mapping for that entity. We will first install the Entity Framework Core package, define the database table mapping, create a database migration and update the database.

### Installing the Entity Framework Core Package

> In this section, we will install the [`Volo.Abp.EntityFrameworkCore`](https://abp.io/package-detail/Volo.Abp.EntityFrameworkCore) package to the Ordering module. That package is DBMS-independent and leaves the DBMS selection to the final application. If you want, you can install DBMS-specific package instead. For example, you can install the [`Volo.Abp.EntityFrameworkCore.SqlServer`](https://abp.io/package-detail/Volo.Abp.EntityFrameworkCore.SqlServer) package if you are using SQL server and want to make SQL Server specific configuration for your module's database.
> You can search for other packages on the [abp.io/packages](https://abp.io/packages) page.

Stop the web application if it is still running. Return to ABP Studio, right-click the `ModularCrm.Ordering` package on the *Solution Explorer* panel and select the *Add Package Reference* command:

![abp-studio-add-package-reference-3](images/abp-studio-add-package-reference-3.png)

Select the *NuGet* tab, type `Volo.Abp.EntityFrameworkCore` as the *Package name* and specify a *Version* that is compatible with the ABP version used by your solution:

![abp-studio-add-package-reference-dialog-2](images/abp-studio-add-package-reference-dialog-2.png)

Once you click the *OK* button, the NuGet package reference is added.

### Defining the Database Mappings

Entity Framework Core requires to define a `DbContext` class as the main object for the database mapping. We want to use the main application's `DbContext` object. In that way, we can control the database migrations in a single point, ensure database transactions on multi-module operations and establish relations between database tables of different modules. However, the Ordering module can not use the main application's `DbContext` object, because it doesn't depend on the main application and we don't want to establish such a dependency.

As a solution, we will define a `DbContext` interface in the Ordering module which is then implemented by the main module's `DbContext`.

Open your IDE, create a `Data` folder under the `ModularCrm.Ordering` project, and create an `IOrderingDbContext` interface under that folder:

````csharp
using Microsoft.EntityFrameworkCore;
using ModularCrm.Ordering.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace ModularCrm.Ordering.Data
{
    public interface IOrderingDbContext : IEfCoreDbContext
    {
        DbSet<Order> Orders { get; set; }
    }
}
````

Now, we can inject and use the `IOrderingDbContext` in the Ordering module. Actually, we will not directly use that interface most of the time. Instead, we will use ABP's [repositories](../../framework/architecture/domain-driven-design/repositories.md).

It is best to configure the database table mapping for the `Order` entity in the Ordering module. We will create an extension method that will be called by the main application later. Create a class named `OrderingDbContextModelCreatingExtensions` in the same `Data` folder:

````csharp
using Microsoft.EntityFrameworkCore;
using ModularCrm.Ordering.Entities;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ModularCrm.Ordering.Data
{
    public static class OrderingDbContextModelCreatingExtensions
    {
        public static void ConfigureOrdering(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<Order>(b =>
            {
                //Configure table name
                b.ToTable("Orders");

                //Always call this method to setup base entity properties
                b.ConfigureByConvention();

                //Properties of the entity
                b.Property(q => q.CustomerName).IsRequired().HasMaxLength(120);
            });
        }
    }
}
````

#### Configuring the Main Application

Open the 

d

d