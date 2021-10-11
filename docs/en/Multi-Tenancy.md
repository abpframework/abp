# Multi-Tenancy

Multi-Tenancy is a widely used architecture to create **SaaS applications** where the hardware and software **resources are shared by the customers** (tenants). ABP Framework provides all the base functionalities to create **multi tenant applications**. 

Wikipedia [defines](https://en.wikipedia.org/wiki/Multitenancy) the multi-tenancy as like that:

> Software **Multi-tenancy** refers to a software **architecture** in which a **single instance** of software runs on a server and serves **multiple tenants**. A tenant is a group of users who share a common access with specific privileges to the software instance. With a multitenant architecture, a software application is designed to provide every tenant a **dedicated share of the instance including its data**, configuration, user management, tenant individual functionality and non-functional properties. Multi-tenancy contrasts with multi-instance architectures, where separate software instances operate on behalf of different tenants.

## Terminology: Host vs Tenant

There are two main side of a typical SaaS / Multi-tenant application:

* A **Tenant** is a customer of the SaaS application that pays money to use the service.
* **Host** is the company that owns the SaaS application and manages the system.

The Host and the Tenant terms will be used for that purpose in the rest of the document.

## Configuration

### AbpMultiTenancyOptions: Enable/Disable Multi-Tenancy

`AbpMultiTenancyOptions` is the main options class to **enable/disable the multi-tenancy** for your application.

**Example: Enable multi-tenancy**

```csharp
Configure<AbpMultiTenancyOptions>(options =>
{
    options.IsEnabled = true;
});
```

> Multi-Tenancy is disabled in the ABP Framework by default. However, it is **enabled by default** when you create a new solution using the [startup template](Startup-Templates/Application.md). `MultiTenancyConsts` class in the solution has a constant to control it in a single place.

### Database Architecture

ABP Framework supports all the following approaches to store the tenant data in the database;

* **Single Database**: All tenants are stored in a single database.
* **Database per Tenant**: Every tenant has a separate, dedicated database to store the data related to that tenant.
* **Hybrid**: Some tenants share a single databases while some tenants may have their own databases.

[Tenant management module](Modules/Tenant-Management.md) (which comes pre-installed with the startup projects) allows you to set a connection string for any tenant (as optional), so you can achieve any of the approaches.

## Usage

Multi-tenancy system is designed to **work seamlessly** and make your application code **multi-tenancy unaware** as much as possible.

### IMultiTenant

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

When you implement this interface, ABP Framework **automatically** [filters](Data-Filtering.md) entities for the current tenant when you query from database. So, you don't need to manually add `TenantId` condition while performing queries. A tenant can not access to data of another tenant by default.

#### Why the TenantId Property is Nullable?

`IMultiTenant.TenantId` is **nullable**. When it is null that means the entity is owned by the **Host** side and not owned by a tenant. It is useful when you create a functionality in your system that is both used by the tenant and the host sides.

For example, `IdentityUser` is an entity defined by the [Identity Module](Modules/Identity.md). The host and all the tenants have their own users. So, for the host side, users will have a `null` `TenantId` while tenant users will have their related `TenantId`.

> **Tip**: If your entity is tenant-specific and has no meaning in the host side, you can force to not set `null` for the `TenantId` in the constructor of your entity.

#### When to set the TenantId?

ABP Framework doesn't set the `TenantId` for you (because of the cross tenant operations, ABP can not know the proper `TenantId` in some cases). So, you need to set it yourself **when you create a new multi-tenant entity**.

##### Best Practice

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

### ICurrentTenant

`ICurrentTenant` is the main service to interact with the multi-tenancy infrastructure.

`ApplicationService`, `DomainService`, `AbpController` and some other base classes already has pre-injected `CurrentTenant` properties. For other type of classes, you can inject the `ICurrentTenant` into your service.

#### Tenant Properties

`ICurrentTenant` defines the following properties;

* `Id` (`Guid`): Id of the current tenant. Can be `null` if the current user is a host user or the tenant could not be determined from the request.
* `Name` (`string`): Name of the current tenant. Can be `null` if the current user is a host user or the tenant could not be determined from the request.
* `IsAvailable` (`bool`): Returns `true` if the `Id` is not `null`.

#### Change the Current Tenant

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
* Use `CurrentTenant.Change(null)` to change scope to the host context.

> Always use the `Change` method with a `using` statement like done in this example.

### Data Filtering: Disable the Multi-Tenancy Filter

As mentioned before, ABP Framework handles data isolation between tenants using the [Data Filtering](Data-Filtering.md) system. In some cases, you may want to disable it and perform a query on all the data, without filtering for the current tenant.

**Example: Get count of products in the database, including all the products of all the tenants.**

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace MultiTenancyDemo.Products
{
    public class ProductManager : DomainService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IDataFilter _dataFilter;

        public ProductManager(
            IRepository<Product, Guid> productRepository,
            IDataFilter dataFilter)
        {
            _productRepository = productRepository;
            _dataFilter = dataFilter;
        }

        public async Task<long> GetProductCountAsync()
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                return await _productRepository.GetCountAsync();
            }
        }
    }
}

````

See the [Data Filtering document](Data-Filtering.md) for more.

> Note that this approach won't work if your tenants have **separate databases** since there is no built-in way to query from multiple database in a single database query. You should handle it yourself if you need it.

## Infrastructure

### Determining the Current Tenant

The first thing for a multi-tenant application is to determine the current tenant on the runtime.

ABP Framework provides an extensible **Tenant Resolving** system for that purpose. Tenant Resolving system then used in the **Multi-Tenancy Middleware** to determine the current tenant for the current HTTP request.

#### Tenant Resolvers

##### Default Tenant Resolvers

The following resolvers are provided and configured by default;

* `CurrentUserTenantResolveContributor`: Gets the tenant id from claims of the current user, if the current user has logged in. **This should always be the first contributor for the security**.
* `QueryStringTenantResolveContributor`: Tries to find current tenant id from query string parameters. The parameter name is `__tenant` by default.
* `FormTenantResolveContributor`：Tries to find current tenant id from form parameters. The parameter name is `__tenant` by default.
* `RouteTenantResolveContributor`: Tries to find current tenant id from route (URL path). The variable name is `__tenant` by default. If you defined a route with this variable, then it can determine the current tenant from the route.
* `HeaderTenantResolveContributor`: Tries to find current tenant id from HTTP headers. The header name is `__tenant` by default.
* `CookieTenantResolveContributor`: Tries to find current tenant id from cookie values. The cookie name is `__tenant` by default.

###### Problems with the NGINX

You may have problems with the `__tenant` in the HTTP Headers if you're using the [nginx](https://www.nginx.com/) as the reverse proxy server. Because it doesn't allow to use underscore and some other special characters in the HTTP headers and you may need to manually configure it. See the following documents please: 
http://nginx.org/en/docs/http/ngx_http_core_module.html#ignore_invalid_headers
http://nginx.org/en/docs/http/ngx_http_core_module.html#underscores_in_headers

###### AbpAspNetCoreMultiTenancyOptions

`__tenant` parameter name can be changed using `AbpAspNetCoreMultiTenancyOptions`.

**Example:**

````csharp
services.Configure<AbpAspNetCoreMultiTenancyOptions>(options =>
{
    options.TenantKey = "MyTenantKey";
});
````

If you change the `TenantKey`, make sure to pass it to `CoreModule` in the Angular client as follows:

```js
@NgModule({
  imports: [
    CoreModule.forRoot({
      // ...
      tenantKey: 'MyTenantKey'
    }),
  ],
  // ...
})
export class AppModule {}
```

If you need to access it, you can inject it as follows:

```js
import { Inject } from '@angular/core';
import { TENANT_KEY } from '@abp/ng.core';

class SomeComponent {
    constructor(@Inject(TENANT_KEY) private tenantKey: string) {}
} 
```

> However, we don't suggest to change this value since some clients may assume the the `__tenant` as the parameter name and they might need to manually configure then.

##### Domain/Subdomain Tenant Resolver

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

> There is an [example](https://github.com/abpframework/abp-samples/tree/master/DomainTenantResolver) that uses the subdomain to determining the current tenant. 

##### Custom Tenant Resolvers

You can add implement your custom tenant resolver and configure the `AbpTenantResolveOptions` in your module's `ConfigureServices` method as like below:

````csharp
Configure<AbpTenantResolveOptions>(options =>
{
    options.TenantResolvers.Add(new MyCustomTenantResolveContributor());
});
````

`MyCustomTenantResolveContributor` must inherit from the `TenantResolveContributorBase` (or implement the `ITenantResolveContributor`) as shown below:

````csharp
using System.Threading.Tasks;
using Volo.Abp.MultiTenancy;

namespace MultiTenancyDemo.Web
{
    public class MyCustomTenantResolveContributor : TenantResolveContributorBase
    {
        public override string Name => "Custom";

        public override Task ResolveAsync(ITenantResolveContext context)
        {
            //TODO...
        }
    }
}
````

* A tenant resolver should set `context.TenantIdOrName` if it can determine it. If not, just leave it as is to allow the next resolver to determine it.
* `context.ServiceProvider` can be used if you need to additional services to resolve from the [dependency injection](Dependency-Injection.md) system.

#### Multi-Tenancy Middleware

Multi-Tenancy middleware is an ASP.NET Core request pipeline [middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware) that determines the current tenant from the HTTP request and sets the `ICurrentTenant` properties.

Multi-Tenancy middleware is typically placed just under the [authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication) middleware (`app.UseAuthentication()`):

````csharp
app.UseMultiTenancy();
````

> This middleware is already configured in the startup templates, so you normally don't need to manually add it.

### Tenant Store

`ITenantStore` is used to get the tenant configuration from a data source.

#### Tenant Management Module

The [tenant management module](Modules/Tenant-Management) is **included in the startup templates** and implements the `ITenantStore` interface to get the tenants and their configuration from a database. It also provides the necessary functionality and UI to manage the tenants and their connection strings.

#### Configuration Data Store

**If you don't want to use the tenant management module**, the `DefaultTenantStore` is used as the `ITenantStore` implementation. It gets the tenant configurations from the [configuration system](Configuration.md) (`IConfiguration`). You can either configure the `AbpDefaultTenantStoreOptions` [options](Options.md) or set it in your `appsettings.json` file:

**Example: Define tenants in appsettings.json**

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
        "Default": "...tenant2's db connection string here..."
      }
    }
  ]
````

> It is recommended to **use the Tenant Management module**, which is already pre-configured when you create a new application with the ABP startup templates.

### Other Multi-Tenancy Infrastructure

ABP Framework was designed to respect to the multi-tenancy in every aspect and most of the times everything will work as expected.

BLOB Storing, Caching, Data Filtering, Data Seeding, Authorization and all the other services are designed to properly work in a multi-tenant system.

## The Tenant Management Module

ABP Framework provides all the the infrastructure to create a multi-tenant application, but doesn't make any assumption about how you manage (create, delete...) your tenants.

The [Tenant Management module](Modules/Tenant-Management.md) provides a basic UI to manage your tenants and set their connection strings. It is pre-configured for the [application startup template](Startup-Templates/Application.md).

## See Also

* [Features](Features.md)
