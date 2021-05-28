# Tenant Management Module

[Multi-Tenancy](../Multi-Tenancy.md) is one of the core features of ABP Framework. It provides the fundamental infrastructure to build your own SaaS (Software-as-a-Service) solution. ABP's multi-tenancy system abstracts where your tenants are stored, by providing the `ITenantStore` interface. All you need to do is to implement that interface.

**The Tenant Management module is an implementation of the the `ITenantStore` interface. It stores tenants in a database. It also provides UI to manage your tenants and their [features](../Features.md).**

> Please **refer to the [Multi-Tenancy](../Multi-Tenancy.md) documentation** to understand the multi-tenancy system of the ABP Framework. This document focuses on the Tenant Management module.

### About the ABP Commercial SaaS Module

The [SaaS Module](https://commercial.abp.io/modules/Volo.Saas) is an alternative implementation of this module with more features and possibilities. It is distributed as a part of the [ABP Commercial](https://commercial.abp.io/) subscription.

## How to Install

This module comes as pre-installed (as [NuGet/NPM packages](NuGet/NPM packages)) when you [create a new solution](https://abp.io/get-started) with the ABP Framework. You can continue to use it as package and get updates easily, or you can include its source code into your solution (see `get-source` [CLI](../CLI.md) command) to develop your custom module.

### The Source Code

The source code of this module can be accessed [here](https://github.com/abpframework/abp/tree/dev/modules/tenant-management). The source code is licensed with [MIT](https://choosealicense.com/licenses/mit/), so you can freely use and customize it.

## User Interface

This module adds "*Administration -> Tenant Management -> Tenants*" menu item to the main menu of the application, which opens the page shown below:

![module-tenant-management-page](../images/module-tenant-management-page.png)

In this page, you see the all the tenants. You can create a new tenant as shown below:

![module-tenant-management-new-tenant](../images/module-tenant-management-new-tenant.png)

In this modal;

* **Name**: The unique name of the tenant. If you use subdomains for your tenants (like https://some-tenant.your-domain.com), this will be the subdomain name.
* **Admin Email Address**: Email address of the admin user for this tenant.
* **Admin Password**: The password of the admin user for this tenant.

When you click to *Actions* button near to a tenant, you will see the actions you can take:

![module-tenant-management-actions](../images/module-tenant-management-actions.png)

### Managing the Tenant Features

The Features action opens a modal to enable/disable/set [features](../Features.md) for the related tenant. Here, an example modal:

![features-modal](../images/features-modal.png)

### Managing the Host Features

*Manage Host features* button is used  to set features for the host side, if you use the features of your application also in the host side.

## Internals

This section can be used as a reference if you want to [customize](../Customizing-Application-Modules-Guide.md) this module without changing [its source code](https://github.com/abpframework/abp/tree/dev/modules/tenant-management).

### Domain Layer

#### Aggregates

* `Tenant`

#### Repositories

* `ITenantRepository`

#### Domain Services

* `TenantManager`

### Application Layer

#### Application Services

* `TenantAppService`

#### Permissions

- `AbpTenantManagement.Tenants`: Tenant management.
- `AbpTenantManagement.Tenants.Create`: Creating a new tenant.
- `AbpTenantManagement.Tenants.Update`: Editing an existing tenant.
- `AbpTenantManagement.Tenants.Delete`: Deleting an existing tenant.
- `AbpTenantManagement.Tenants.ManageFeatures`: Manage features of the tenants.

### Entity Framework Core Integration

* `TenantManagementDbContext` (implements `ITenantManagementDbContext`)

**Database Tables:**

* `AbpTenants`
* `AbpTenantConnectionStrings`

### MongoDB Integration

* `TenantManagementMongoDbContext` (implements `ITenantManagementMongoDbContext`)

**Database Collections:**

* `AbpTenants` (also includes the connection string)

## Notices

ABP Framework allows to use *database per tenant* approach that allows a tenant can have a dedicated database. This module has the fundamental infrastructure to make that implementation possible (see its source code), however it doesn't implement the application layer and UI functionalities to provide it as an out of the box implementation. You can implement these features yourself, or consider to use the [ABP Commercial Saas Module](https://docs.abp.io/en/commercial/latest/modules/saas) that fully implements it and provides much more business features.

## See Also

* [Multi-Tenancy](../Multi-Tenancy.md)
* [ABP Commercial SaaS Module](https://docs.abp.io/en/commercial/latest/modules/saas)