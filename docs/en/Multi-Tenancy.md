# Multi-Tenancy

Multi-Tenancy is a widely used architecture to create **SaaS applications** where the hardware and software **resources are shared by the customers** (tenants). ABP Framework provides all the base functionalities to create **multi tenant applications**. 

Wikipedia [defines](https://en.wikipedia.org/wiki/Multitenancy) the multi-tenancy as like that:

> Software **Multi-tenancy** refers to a software **architecture** in which a **single instance** of software runs on a server and serves **multiple tenants**. A tenant is a group of users who share a common access with specific privileges to the software instance. With a multitenant architecture, a software application is designed to provide every tenant a **dedicated share of the instance including its data**, configuration, user management, tenant individual functionality and non-functional properties. Multi-tenancy contrasts with multi-instance architectures, where separate software instances operate on behalf of different tenants.

## Terminology: Host vs Tenant

There are two main side of a typical SaaS / Multi-tenant application:

* A **Tenant** is a customer of the SaaS application that pays money to use the service.
* **Host** is the company that owns the SaaS application and manages the system.

We will use the Host and Tenant terms for that purpose in the rest of the document.

## IMultiTenant

You should implement the `IMultiTenant` interface for your [entities](Entities.md) to make them **multi-tenancy ready**. 

**Example: A multi-tenant *Product* entity**

````csharp
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace MultiTenancyDemo.Products
{
    public class Product : AggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; } //Defined by the IMultiTenant interface

        public string Name { get; set; }

        public float Price { get; set; }
    }
}
````

* `IMultiTenant` interface just defines a `TenantId` property.

When you implement this interface, ABP Framework **automatically** [filters](Data-Filtering.md) entities for the current tenant when you query from database. So, you don't need to manually add `TenantId` condition while performing queries; a tenant can not access to data of another tenant.

### Why the TenantId Property is Nullable?

`IMultiTenant.TenantId` is **nullable**. When it is null that means the entity is owned by the **Host** side and not owned by a tenant. It is useful when you create a functionality in your system that is both used by the tenant and the host sides.

For example, `IdentityUser` is an entity defined by the [Identity Module](Modules/Identity.md). The host and all the tenants have their own users. So, for the host side, users will have a `null` `TenantId` while tenant users will have their related `TenantId`.

> **Tip**: If your entity is tenant-specific and has no meaning in the host side, you can force to not set `null` for the `TenantId` in the constructor of your entity.

### When to set the TenantId?

ABP Framework doesn't set the `TenantId` for you (because of the cross tenant operations, ABP can not know the proper `TenantId` in some cases). So, you need to set it yourself **when you create a new multi-tenant entity**.

#### Best Practice

We suggest to set the `TenantId` in the constructor and never allow to change it again. So, the `Product` class can be re-written as below:

````csharp
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace MultiTenancyDemo.Products
{
    public class Product : AggregateRoot<Guid>, IMultiTenant
    {
        //Private setter prevents changing it later
        public Guid? TenantId { get; private set; }

        public string Name { get; set; }

        public float Price { get; set; }

        protected Product()
        {
            //This parameterless constructor is needed for ORMs
        }
        
        public Product(string name, float price, Guid? tenantId)
        {
            Name = name;
            Price = price;
            TenantId = tenantId; //Set in the constructor
        }
    }
}
````

> You can see the [entities document](Entities.md) for a more about entities and aggregate roots.

You typically use the `ICurrentTenant` to set the `TenantId` while creating a new `Product`.

**Example: Creating a new product in a [Domain Service](Domain-Services.md)** 

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace MultiTenancyDemo.Products
{
    public class ProductManager : DomainService
    {
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductManager(IRepository<Product, Guid> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> CreateAsync(string name, float price)
        {
            var product = new Product(name, price, CurrentTenant.Id);
            return await _productRepository.InsertAsync(product);
        }
    }
}
````

* `DomainService` base class (and some common base classes in the ABP Framework) provides the `CurrentTenant`, so you directly use it. Otherwise, you need to [inject](Dependency-Injection.md) the `ICurrentTenant` service.

## ICurrentTenant

`ICurrentTenant` is the main service to interact with the multi-tenancy infrastructure. `ApplicationService`, `DomainService`, `AbpController` and some other base classes already has pre-injected `CurrentTenant` properties. For other type of classes, you can inject the `ICurrentTenant` into your service.

### Tenant Properties

`ICurrentTenant` defines the following properties;

* `Id` (`Guid`): Id of the current tenant. Can be `null` if the current user is a host user or the tenant could not be determined from the request.
* `Name` (`string`): Name of the current tenant. Can be `null` if the current user is a host user or the tenant could not be determined from the request.
* `IsAvailable` (`bool`): Returns `true` if the `Id` is not `null`.

### Change the Current Tenant

ABP Framework automatically filters the resources (database, cache...) based on the `ICurrentTenant.Id`. However, in some cases you may want to perform an operation on behalf of a specific tenant, generally when you are in the host context.

`ICurrentTenant.Change` method changes the current tenant for a limited scope, so you can safely perform operations for the tenant.

**Example: Get product count of a specific tenant**

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace MultiTenancyDemo.Products
{
    public class ProductManager : DomainService
    {
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductManager(IRepository<Product, Guid> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<long> GetProductCountAsync(Guid? tenantId)
        {
            using (CurrentTenant.Change(tenantId))
            {
                return await _productRepository.GetCountAsync();
            }
        }
    }
}
````

* `Change` method can be used in a **nested way**. It restores the `CurrentTenant.Id` to the previous value after the `using` statement.
* When you use `CurrentTenant.Id` inside the `Change` scope, you get the `tenantId` provided to the `Change` method. So, the repository also get this `tenantId` and can filter the database query accordingly.

> Always use the `Change` method with a `using` statement like done in this example.

## Determining Current Tenant

The first thing for a multi-tenant application is to determine the current tenant on the runtime. ABP Framework provides an extensible *Tenant Resolving* system for that purpose.

### Default Tenant Resolvers

The following resolvers are provided by default;

* `CurrentUserTenantResolveContributor`: Gets the tenant id from claims of the current user, if the current user has logged in. **This should always be the first contributor for the security**.
* `QueryStringTenantResolveContributor`: Tries to find current tenant id from query string parameters. The parameter name is `__tenant` by default.
* `FormTenantResolveContributor`：Tries to find current tenant id from form parameters. The parameter name is `__tenant` by default.
* `RouteTenantResolveContributor`: Tries to find current tenant id from route (URL path). The variable name is `__tenant` by default. If you defined a route with this variable, then it can determine the current tenant from the route.
* `HeaderTenantResolveContributor`: Tries to find current tenant id from HTTP headers. The header name is `__tenant` by default.
* `CookieTenantResolveContributor`: Tries to find current tenant id from cookie values. The cookie name is `__tenant` by default.

#### Problems with the NGINX

You may have problems with the `__tenant` in the HTTP Headers if you're using the [nginx](https://www.nginx.com/) as the reverse proxy server. Because it doesn't allow to use underscore and some other special characters in the HTTP headers and you may need to manually configure it. See the following documents please: 
http://nginx.org/en/docs/http/ngx_http_core_module.html#ignore_invalid_headers
http://nginx.org/en/docs/http/ngx_http_core_module.html#underscores_in_headers

#### AbpAspNetCoreMultiTenancyOptions

`__tenant` parameter name can be changed using `AbpAspNetCoreMultiTenancyOptions`.

**Example:**

````csharp
services.Configure<AbpAspNetCoreMultiTenancyOptions>(options =>
{
    options.TenantKey = "MyTenantKey";
});
````

> However, we don't suggest to change this value since some clients may assume the the `__tenant` as the parameter name and they might need to manually configure then.

### Domain/Subdomain Tenant Resolver

In a real application, most of times you will want to determine current tenant either by subdomain (like mytenant1.mydomain.com) or by the whole domain (like mytenant.com). If so, you can configure the `AbpTenantResolveOptions` to add the domain tenant resolver.

**Example: Add a subdomain resolver**

````csharp
Configure<AbpTenantResolveOptions>(options =>
{
    options.AddDomainTenantResolver("{0}.mydomain.com");
});
````

* `{0}` is the placeholder to determine current tenant's unique name.
* Add this code to the `ConfigureServices` method of your [module](Module-Development-Basics.md).
* This should be done in the *Web/API Layer* since the URL is a web related stuff.

### Custom Tenant Resolvers

You can add implement your custom tenant resolver and configure the `AbpTenantResolveOptions` in your module's `ConfigureServices` method as like below:

````csharp
Configure<AbpTenantResolveOptions>(options =>
{
    options.TenantResolvers.Add(new MyCustomTenantResolveContributor());
});
````

`MyCustomTenantResolveContributor` must inherit from the `TenantResolveContributorBase` (or implement the `ITenantResolveContributor`) as shown below:

````csharp
using Volo.Abp.MultiTenancy;

namespace MultiTenancyDemo.Web
{
    public class MyCustomTenantResolveContributor : TenantResolveContributorBase
    {
        public override string Name => "Custom";

        public override void Resolve(ITenantResolveContext context)
        {
            //TODO...
        }
    }
}
````

* A tenant resolver should set `context.TenantIdOrName` if it can determine it. If not, just leave it as is to allow the next resolver to determine it.
* `context.ServiceProvider` can be used if you need to additional services to resolve from the [dependency injection](Dependency-Injection.md) system.

## Avdanced Topics

### Tenant Store

Volo.Abp.MultiTenancy package defines **ITenantStore** to abstract data source from the framework. You can implement ITenantStore to work with any data source (like a relational database) that stores information of your tenants.

...

##### Configuration Data Store

There is a built in (and default) tenant store, named ConfigurationTenantStore, that can be used to store tenants using standard [configuration system](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/) (with [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration) package). Thus, you can define tenants as hard coded or get from your appsettings.json file.

###### Example: Define tenants as hard-coded

````C#
using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDefaultTenantStoreOptions>(options =>
            {
                options.Tenants = new[]
                {
                    new TenantConfiguration(
                        Guid.Parse("446a5211-3d72-4339-9adc-845151f8ada0"), //Id
                        "tenant1" //Name
                    ),
                    new TenantConfiguration(
                        Guid.Parse("25388015-ef1c-4355-9c18-f6b6ddbaf89d"), //Id
                        "tenant2" //Name
                    )
                    {
                        //tenant2 has a seperated database
                        ConnectionStrings =
                        {
                            {ConnectionStrings.DefaultConnectionStringName, "..."}
                        }
                    }
                };
            });
        }
    }
}
````

###### Example: Define tenants in appsettings.json

First create your configuration from your appsettings.json file as you always do.

````C#
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = BuildConfiguration();

            Configure<AbpDefaultTenantStoreOptions>(configuration);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
````

Then add a "**Tenants**" section to your appsettings.json:

````json
"Tenants": [
    {
      "Id": "446a5211-3d72-4339-9adc-845151f8ada0",
      "Name": "tenant1"
    },
    {
      "Id": "25388015-ef1c-4355-9c18-f6b6ddbaf89d",
      "Name": "tenant2",
      "ConnectionStrings": {
        "Default": "...write tenant2's db connection string here..."
      }
    }
  ]
````

##### Volo.Abp... Package (TODO)

TODO: This package implements ITenantStore using a real database...

#### Tenant Information

ITenantStore works with **TenantConfiguration** class that has several properties for a tenant:

* **Id**: Unique Id of the tenant.
* **Name**: Unique name of the tenant.
* **ConnectionStrings**: If this tenant has dedicated database(s) to store it's data, then connection strings can provide database connection strings (it may have a default connection string and connection strings per modules - TODO: Add link to Abp.Data package document).

A multi-tenant application may require additional tenant properties, but these are the minimal requirements for the framework to work with multiple tenants.

#### Change Tenant By Code

TODO...

### Volo.Abp.AspNetCore.MultiTenancy Package

Volo.Abp.AspNetCore.MultiTenancy package integrate multi-tenancy to ASP.NET Core applications. To install it to your project, run the following command on PMC:

````
Install-Package Volo.Abp.AspNetCore.MultiTenancy
````

Then you can add **AbpAspNetCoreMultiTenancyModule** dependency to your module:

````C#
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAspNetCoreMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
````

#### Multi-Tenancy Middleware

Volo.Abp.AspNetCore.MultiTenancy package includes the multi-tenancy middleware...

````C#
app.UseMultiTenancy();
````

TODO:...

#### Determining Current Tenant From Web Request

### Related Packages

TODO

Volo.Abp.MultiTenancy package defines fundamental interfaces to make your code "multi-tenancy ready". So, install it to your project using the package manager console (PMC):

````
Install-Package Volo.Abp.MultiTenancy
````

> This package is already installed by default with the startup template. So, most of the time, you don't need to install it manually.

Then you can add **AbpMultiTenancyModule** dependency to your module:

````C#
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
````

> With the "Multi-tenancy ready" concept, we intent to develop our code to be compatible with multi-tenancy approach. Then it can be used in a multi-tenant application or not, depending on the requirements of the final application.

## See Also

* [Features](Features.md)