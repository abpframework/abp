## Multi-Tenancy

ABP Multi-tenancy module provides base functionality to create multi tenant applications. 

Wikipedia [defines](https://en.wikipedia.org/wiki/Multitenancy) multi-tenancy as like that:

> Software **Multi-tenancy** refers to a software **architecture** in which a **single instance** of a software runs on a server and serves **multiple tenants**. A tenant is a group of users who share a common access with specific privileges to the software instance. With a multitenant architecture, a software application is designed to provide every tenant a **dedicated share of the instance including its data**, configuration, user management, tenant individual functionality and non-functional properties. Multi-tenancy contrasts with multi-instance architectures, where separate software instances operate on behalf of different tenants.

### Volo.Abp.MultiTenancy Package

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

#### Define Entities

You can implement **IMultiTenant** interface for your entities to make them multi-tenancy ready. Example:

````C#
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    public class Product : AggregateRoot, IMultiTenant
    {
        public Guid? TenantId { get; set; } //IMultiTenant defines TenantId property

        public string Name { get; set; }

        public float Price { get; set; }
    }
}
````

IMultiTenant requires to define a **TenantId** property in the implementing entity (See [entity documentation](Entities.md) for more about entities).

#### Obtain Current Tenant's Id

Your code may require to get current tenant's id (regardless of how it's retrieved actually). You can [inject](Dependency-Injection.md) and use **ICurrentTenant** interface for such cases. Example:

````C#
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    public class MyService : ITransientDependency
    {
        private readonly ICurrentTenant _currentTenant;

        public MyService(ICurrentTenant currentTenant)
        {
            _currentTenant = currentTenant;
        }

        public void DoIt()
        {
            var tenantId = _currentTenant.Id;
            //use tenantId in your code...
        }
    }
}
````

#### Change Current Tenant

TODO: ...

#### Determining Current Tenant

The first thing for a multi-tenant application is to determine the current tenant on the runtime. Volo.Abp.MultiTenancy package only provides abstractions (named as tenant resolver) for determining the current tenant, however it does not have any implementation out of the box.

**Volo.Abp.AspNetCore.MultiTenancy** package has implementation to determine the current tenant from current web request (from subdomain, header, cookie, route... etc.). See Volo.Abp.AspNetCore.MultiTenancy Package section later in this document.

##### Custom Tenant Resolvers

You can add your custom tenant resolver to **TenantResolveOptions** in your module's ConfigureServices method as like below:

````C#
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
            Configure<TenantResolveOptions>(options =>
            {
                options.TenantResolvers.Add(new MyCustomTenantResolveContributor());
            });

            //...
        }
    }
}
````

`MyCustomTenantResolveContributor` must implement **ITenantResolveContributor** as shown below:

````C#
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    public class MyCustomTenantResolveContributor : ITenantResolveContributor
    {
        public void Resolve(ITenantResolveContext context)
        {
            context.TenantIdOrName = ... //find tenant id or tenant name from somewhere...
        }
    }
}
````

A tenant resolver can set **TenantIdOrName** if it can determine it. If not, just leave it as is to allow next resolver to determine it.

#### Tenant Store

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
            Configure<ConfigurationTenantStoreOptions>(options =>
            {
                options.Tenants = new[]
                {
                    new TenantInformation(
                        Guid.Parse("446a5211-3d72-4339-9adc-845151f8ada0"), //Id
                        "tenant1" //Name
                    ),
                    new TenantInformation(
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

            Configure<ConfigurationTenantStoreOptions>(configuration);
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

ITenantStore works with **TenantInformation** class that has several properties for a tenant:

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

Volo.Abp.AspNetCore.MultiTenancy package adds following tenant resolvers to determine current tenant from current web request (ordered by priority). These resolvers are added and work out of the box:

* **QueryStringTenantResolver**: Tries to find current tenant id from query string parameter. Parameter name is "__tenant" by default.
* **RouteTenantResolver**: Tries to find current tenant id from route (URL path). Variable name is "__tenant" by default. So, if you defined a route with this variable, then it can determine the current tenant from the route.
* **HeaderTenantResolver**: Tries to find current tenant id from HTTP header. Header name is "__tenant" by default.
* **CookieTenantResolver**: Tries to find current tenant id from cookie values. Cookie name is "__tenant" by default.

> If you use nginx as a reverse proxy server, please note that if `TenantKey` contains an underscore or other special characters, there may be a problem, please refer to: 
http://nginx.org/en/docs/http/ngx_http_core_module.html#ignore_invalid_headers
http://nginx.org/en/docs/http/ngx_http_core_module.html#underscores_in_headers


"__tenant" parameter name can be changed using AspNetCoreMultiTenancyOptions. Example:

````C#
services.Configure<AspNetCoreMultiTenancyOptions>(options =>
{
    options.TenantKey = "MyTenantKey";
});
````

##### Domain Tenant Resolver

In a real application, most of times you will want to determine current tenant either by subdomain (like mytenant1.mydomain.com) or by the whole domain (like mytenant.com). If so, you can configure TenantResolveOptions to add a domain tenant resolver.

###### Example: Add a subdomain resolver

````C#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAspNetCoreMultiTenancyModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<TenantResolveOptions>(options =>
            {
                //Subdomain format: {0}.mydomain.com (adding as the highest priority resolver)
                options.TenantResolvers.Insert(0, new DomainTenantResolver("{0}.mydomain.com"));
            });

            //...
        }
    }
}
````

{0} is the the placeholder to determine current tenant's unique name.

Instead of ``options.TenantResolvers.Insert(0, new DomainTenantResolver("{0}.mydomain.com"));`` you can use this shortcut:

````C#
options.AddDomainTenantResolver("{0}.mydomain.com");
````

###### Example: Add a domain resolver

````C#
options.AddDomainTenantResolver("{0}.com");
````

