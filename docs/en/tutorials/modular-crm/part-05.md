# Building the Ordering Module

````json
//[doc-nav]
{
  "Previous": {
    "Name": "Creating the Initial Ordering Module",
    "Path": "tutorials/modular-crm/part-04"
  },
  "Next": {
    "Name": "Integrating Modules: Implementing Integration Services",
    "Path": "tutorials/modular-crm/part-06"
  }
}
````

In the previous part, we created a custom Ordering module and installed it into the main application. However, the Ordering module has no functionality now. In this part, we will create an `Order` entity and add functionality to create and list the orders.

## Creating an `Order` Entity

Open the `ModularCrm.Ordering` .NET solution in your IDE.

> Tip: You can open the folder of a module's .NET solution by right-clicking the related module in ABP Studio and selecting the *Open with* -> *Explorer* command.

The following figure shows the `ModularCrm.Ordering` module in the *Solution Explorer* panel of Visual Studio:

![visual-studio-ordering-module-initial](images/visual-studio-ordering-module-initial.png)

### Adding `Volo.Abp.Ddd.Domain` Package Reference

As you see in the preceding figure, the solution structure is very minimal. It also has a minimal ABP dependency. ABP Framework has [hundreds of NuGet packages](https://abp.io/packages), but the `ModularCrm.Ordering` project only has the [`Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared`](https://abp.io/package-detail/Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared) package reference:

![visual-studio-ordering-ui-package-dependency](images/visual-studio-ordering-ui-package-dependency.png)

We will create an [entity class](../../framework/architecture/domain-driven-design/entities.md). ABP defines base entity classes and other related infrastructure in the [`Volo.Abp.Ddd.Domain`](https://abp.io/package-detail/Volo.Abp.Ddd.Domain) package. So, we need to add a reference to that NuGet package.

You can add that package and manually arrange the [module](../../framework/architecture/modularity/basics.md) class dependency. However, we will use ABP Studio as a more practical option.

Return to ABP Studio, right-click the `ModularCrm.Ordering` package in the *Solution Explorer* and select the *Add Package Reference* command:

![abp-studio-add-package-reference-2](images/abp-studio-add-package-reference-2.png)

That command opens a dialog to add a new package reference:

![abp-studio-add-nuget-package-reference](images/abp-studio-add-nuget-package-reference.png)

Select the *NuGet* tab, type `Volo.Abp.Ddd.Domain` as the *Package name* and write the version of the package you want to install. Please be sure that you are installing the same version as the other ABP packages you are already using. ABP Studio will provide an easier way to select the package and its version in future ABP versions.

Click the *OK* button. Now you can check the *Packages* under the `ModularCrm.Ordering` module *Dependencies* to see the `Volo.Abp.Ddd.Domain` package is installed:

![abp-studio-added-ddd-domain-package](images/abp-studio-added-ddd-domain-package.png)

### Adding an `Order` Class

Now, you can return your IDE and add an `Order` class to the `ModularCrm.Ordering` project (open an `Entities` folder and place the `Order.cs` into that folder):

````csharp
using System;
using ModularCrm.Ordering.Contracts.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace ModularCrm.Ordering.Entities
{
    public class Order : CreationAuditedAggregateRoot<Guid>
    {
        public Guid ProductId { get; set; }
        public string CustomerName { get; set; }
        public OrderState State { get; set; }
    }
}
````

We allow users to place only a single product within an order. The `Order` entity would be much more complex in a real-world application. However, the complexity of the `Order` entity doesn't affect modularity, so we keep it simple to focus on modularity in this tutorial. We are inheriting from the [`CreationAuditedAggregateRoot` class](../../framework/architecture/domain-driven-design/entities.md) since I want to know when an order has been created and who has created it.

### Adding an `OrderState` Enumeration

We used an `OrderState` enumeration that has not yet been defined. Open an `Enums` folder in the `ModularCrm.Ordering.Contracts` project and create an `OrderState.cs` file inside it:

````csharp
namespace ModularCrm.Ordering.Contracts.Enums;

public enum OrderState : byte
{
    Placed = 0,
    Delivered = 1,
    Canceled = 2
}
````

The final structure of the Ordering module should be similar to the following figure in your IDE:

![visual-studio-order-entity](images/visual-studio-order-entity.png)

## Configuring the Database Mapping

The `Order` entity has been created. Now, we need to configure the database mapping for that entity. We will first install the Entity Framework Core package, define the database table mapping, create a database migration and update the database.

### Installing the Entity Framework Core Package

> In this section, we will install the [`Volo.Abp.EntityFrameworkCore`](https://abp.io/package-detail/Volo.Abp.EntityFrameworkCore) package to the Ordering module. That package is DBMS-independent and leaves the DBMS selection to the final application. If you want, you can install a DBMS-specific package instead. For example, you can install the [`Volo.Abp.EntityFrameworkCore.SqlServer`](https://abp.io/package-detail/Volo.Abp.EntityFrameworkCore.SqlServer) package if you are using SQL Server and want to make SQL Server specific configuration for your module's database.
> You can search for other packages on the [abp.io/packages](https://abp.io/packages) page.

Stop the web application if it is still running. Return to ABP Studio, right-click the `ModularCrm.Ordering` package on the *Solution Explorer* panel and select the *Add Package Reference* command:

![abp-studio-add-package-reference-3](images/abp-studio-add-package-reference-3.png)

Select the *NuGet* tab, type `Volo.Abp.EntityFrameworkCore` as the *Package name* and specify a *Version* that is compatible with the ABP version used by your solution:

![abp-studio-add-package-reference-dialog-2](images/abp-studio-add-package-reference-dialog-2.png)

Once you click the *OK* button, the NuGet package reference is added.

### Defining the Database Mappings

Entity Framework Core requires defining a `DbContext` class as the main object for the database mapping. We want to use the main application's `DbContext` object. That way, we can control the database migrations at a single point, ensure database transactions on multi-module operations, and establish relations between database tables of different modules. However, the Ordering module can not use the main application's `DbContext` object because it doesn't depend on the main application, and we don't want to establish such a dependency.

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

We can inject and use the `IOrderingDbContext` in the Ordering module. However, we will not usually directly use that interface. Instead, we will use ABP's [repositories](../../framework/architecture/domain-driven-design/repositories.md), which internally uses that interface.

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

                //Always call this method to set base entity properties
                b.ConfigureByConvention();

                //Properties of the entity
                b.Property(q => q.CustomerName).IsRequired().HasMaxLength(120);
            });
        }
    }
}
````

#### Configuring the Main Application

Open the main application's solution in your IDE, find the `ModularCrmDbContext` class under the `ModularCrm.EntityFrameworkCore` project and follow the 3 steps below:

**(1)** Add the following attribute on top of the `ModularCrmDbContext` class:

````csharp
[ReplaceDbContext(typeof(IOrderingDbContext))]
````

The `ReplaceDbContext` attribute allows the use of the `ModularCrmDbContext` class in the services in the Ordering module.

**(2)** Implement the `IOrderingDbContext` by the `ModularCrmDbContext` class:

````csharp
public class ModularCrmDbContext :
    AbpDbContext<ModularCrmDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext,
    IProductsDbContext,
    IOrderingDbContext //NEW: IMPLEMENT THE INTERFACE
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; } //NEW: ADD DBSET PROPERTY
	...
}
````

**(3)** Finally, call the `ConfigureOrdering()` extension method inside the `OnModelCreating` method after other `Configure...` module calls:

````csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    ...
    builder.ConfigureOrdering(); //NEW: CALL THE EXTENSION METHOD
}
````

In this way, the Ordering module can use' ModularCrmDbContext' over the `IProductsDbContext` interface. This part is only needed once for a module. Next time, you can add a new database migration, as explained in the next section.

#### Add a Database Migration

Now, we can add a new database migration. You can use Entity Framework Core's `Add-Migration` (or `dotnet ef migrations add`) terminal command, but in this tutorial, we will use ABP Studio's shortcut UI.

Ensure that the solution has built. You can right-click the `ModularCrm` (under the `main` folder) on ABP Studio *Solution Runner* and select the *Dotnet CLI* -> *Graph Build* command.

Right-click the `ModularCrm.EntityFrameworkCore` package and select the *EF Core CLI* -> *Add Migration* command:

![abp-studio-add-entity-framework-core-migration](images/abp-studio-add-entity-framework-core-migration.png)

The *Add Migration* command opens a new dialog to get a migration name:

![abp-studio-entity-framework-core-add-migration-order](images/abp-studio-entity-framework-core-add-migration-order.png)

Once you click the *OK* button, a new database migration class is added to the `Migrations` folder of the `ModularCrm.EntityFrameworkCore` project:

![visual-studio-new-migration-class-2](images/visual-studio-new-migration-class-2.png)

Now, you can return to ABP Studio, right-click the `ModularCrm.EntityFrameworkCore` project and select the *EF Core CLI* -> *Update Database* command:

![abp-studio-entity-framework-core-update-database](images/abp-studio-entity-framework-core-update-database.png)

After the operation completes, you can check your database to see the new `Orders` table has been created:

![sql-server-products-database-table](images/sql-server-orders-database-table.png)

## Creating the User Interface

Since this is a non-layered module, we can use entities and repositories directly on the user interface. If you think that is not a good practice, then use the layered module template as we've already done for the *Products* module. But for the Ordering module, we will keep it very simple for this tutorial to show it is also possible.

### Creating a `_ViewImports.cshtml` File

Open the `ModularCrm.Ordering` .NET solution in your favorite IDE, locate the `ModularCrm.Ordering` project and create a `Pages` folder under that project. Then add a `_ViewImports.cshtml` file under that `Pages` folder with the following content:

````csharp
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, Volo.Abp.AspNetCore.Mvc.UI
@addTagHelper *, Volo.Abp.AspNetCore.Mvc.UI.Bootstrap
@addTagHelper *, Volo.Abp.AspNetCore.Mvc.UI.Bundling
````

That file imports some tag helpers from ASP.NET Core and ABP. The `Pages` folder should be like the following figure:

![visual-studio-pages-folder](images/visual-studio-pages-folder.png)

### Creating the Orders Page

Create an `Orders` folder under the `Pages` folder and add an `Index.cshtml` Razor Page inside that new folder. Then replace the `Index.cshtml.cs` content with the following code block:

````csharp
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModularCrm.Ordering.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ModularCrm.Ordering.Pages.Orders
{
    public class IndexModel : PageModel
    {
        public List<Order> Orders { get; set; }

        private readonly IRepository<Order, Guid> _orderRepository;

        public IndexModel(IRepository<Order, Guid> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task OnGetAsync()
        {
            Orders = await _orderRepository.GetListAsync();
        }
    }
}
````

Here, we are injecting a repository to query `Order` entities from the database to show on the page. Open the `Index.cshtml` file and replace the content with the following code block:

````html
@page
@model ModularCrm.Ordering.Pages.Orders.IndexModel

<h1>Orders</h1>

<abp-card>
    <abp-card-body>
        <abp-list-group>
            @foreach (var order in Model.Orders)
            {
                <abp-list-group-item>
                    <strong>Customer:</strong> @order.CustomerName <br />
                    <strong>Product:</strong> @order.ProductId <br />
                    <strong>State:</strong> @order.State
                </abp-list-group-item>
            }
        </abp-list-group>
    </abp-card-body>
</abp-card>
````

This page shows a list of orders on the UI. We haven't created a UI to create new orders, and we will not do it to keep this tutorial simple. If you want to learn how to create advanced UIs with ABP, please follow the [Book Store tutorial](../book-store/index.md).

### Creating Some Sample Data

You can open the database and manually create a few order records to show on the UI:

![sql-server-orders-table-content](images/sql-server-orders-table-content.png)

You can get `ProductId` values from the `Products` table and [generate](https://www.guidgenerator.com/) some random GUIDs for other GUID fields.

### Building the Application

Now, we will run the application to see the result. Please stop the application if it is already running. Then open the *Solution Runner* panel, right-click the `ModularCrm.Web` application, and select the *Build* -> *Graph Build* command:

![abp-studio-solution-runner-graph-build](images/abp-studio-solution-runner-graph-build.png)

We've performed a graph build since we've made a change on a module, and more than building the main application is needed. *Graph Build* command also builds the depended modules if necessary. Alternatively, you could build the Ordering module first (on ABP Studio or your IDE), then right-click the `ModularCrm.Web` application and select the *Run* -> *Build & Start*. This approach can be faster if you have too many modules and you make a change in one of the modules.

### Running the Application

Run the main application on ABP Studio, manually type `/Orders` to the end of your application's URL to open the *Orders* page:

![abp-studio-solution-runner-orders-page](images/abp-studio-solution-runner-orders-page.png)

Great! We can see the list of orders. However, there are two problems:

1. The Order page has no menu item on the main menu. This is because we haven't configured the [navigation menu system](../../framework/ui/mvc-razor-pages/navigation-menu.md) yet.
2. We see Product's GUID ID instead of its name. This is because the Ordering module has no integration with the Products module and doesn't have access to Product module's database to perform a JOIN query.

We will solve the second problem in the [next part](part-06.md), but we can easily add a menu item for the Orders page now.

### Adding a Menu Item

ABP provides a modular navigation [menu system](../../framework/ui/mvc-razor-pages/navigation-menu.md) where each module can contribute to the main menu dynamically.

Open the `ModularCrm.Ordering` .NET solution in your IDE and add the following `OrderingMenuContributor` class into the `ModularCrm.Ordering` project:

````csharp
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace ModularCrm.Ordering
{
    public class OrderingMenuContributor : IMenuContributor
    {
        public Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                context.Menu.AddItem(
                    new ApplicationMenuItem(
                        "ModularCrm.Orders.Index", // Unique menu id
                        "Orders", // Menu display text
                        "~/Orders", // URL
                        "fa-solid fa-basket-shopping" // Icon CSS class
                    )
                );
            }

            return Task.CompletedTask;
        }
    }
}
````

`OrderingMenuContributor` implements the `IMenuContributor` interface, which forces us to implement the `ConfigureMenuAsync` method. In that method, we can manipulate the menu items (add new menu items, remove existing menu items or change the properties of existing menu items). The `ConfigureMenuAsync` method is executed whenever the menu is rendered on the UI, so you can dynamically decide how to manipulate the menu items.

After creating such a class, we should configure the `AbpNavigationOptions` to add that contributor. Open the `OrderingWebModule` class in the `ModularCrm.Ordering` project and add the following configuration code into the `ConfigureServices` method (if there is no `ConfigureServices` method, first create it as shown below):

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<AbpNavigationOptions>(options =>
    {
        options.MenuContributors.Add(new OrderingMenuContributor());
    });
}
````

That's all. You can stop the main application (if it is already working), make a graph build on the main application, run it again on ABP Studio's *Solution Runner* panel and *Browse* it to see the result:

![abp-studio-browser-orders-menu-item](images/abp-studio-browser-orders-menu-item.png)

The *Orders* menu item is added under the *Products* menu item.

> You can check the [menu documentation](../../framework/ui/mvc-razor-pages/navigation-menu.md) to learn more about manipulating menu items.

## Summary

In this part of the *Modular CRM* tutorial, we've built the functionality inside the Ordering module we created in the [previous part](part-04.md). Since we've created the Ordering module from scratch (with the *Empty Module* template), we had to implement many aspects manually, add ABP packages, create some configuration classes, etc. It is good to do all these manually for one time to learn the things, but it is better to use the other module templates (that pre-configure the fundamentals for us) for a more comfortable development experience.

In the next part, we will work on establishing communication between the Orders module and the Products module.
