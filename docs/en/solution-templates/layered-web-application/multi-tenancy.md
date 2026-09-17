```json
//[doc-seo]
{
    "Description": "Explore multi-tenancy in the ABP Framework's layered solution, enabling isolated data and configurations for multiple tenants."
}
```

# Layered Solution: Multi-Tenancy

```json
//[doc-nav]
{
  "Previous": {
    "Name": "Distribution Locking",
    "Path": "solution-templates/layered-web-application/distributed-locking"
  },
  "Next": {
    "Name": "BLOB storing",
    "Path": "solution-templates/layered-web-application/blob-storing"
  }
}
```

> Some of the features mentioned in this document may not be available in the free version. We're using the **\*** symbol to indicate that a feature is available in the **[Team](https://abp.io/pricing)** and **[Higher](https://abp.io/pricing)** licenses.

Multi-tenancy is a software architecture where a single instance(codebase) of software runs on a server and serves multiple tenants. Tenants are isolated from each other and can have their own data, configurations, and users. This document explains how the multi-tenancy mechanism works in the layered solution template. You can learn more about multi-tenancy in the [Multi-Tenancy](../../framework/architecture/multi-tenancy/index.md), [Tenant Management](../../modules/tenant-management.md) and [SaaS **\***](../../modules/saas.md) documents.

## Multi-Tenancy in Layered Solutions

The layered solution templates use the *Multi-Tenancy* architecture only if you *Enable Multi-Tenancy **\**** option while creating the solution.

![saas-module-selection](images/saas-module-selection.png)

You can use different databases for each tenant or a shared database for some tenants. In the *SaaS **\*** module, you can specify the database connection strings in the [Connection Strings Management Modal](../../modules/saas.md#connection-string). All cached data is isolated by tenant. Each event, background job, and other data is stored with the tenant id.

You can use the `ICurrentTenant` service to get the current tenant information in your application.

```csharp
public class MyService : ITransientDependency
{
    private readonly ICurrentTenant _currentTenant;

    public MyService(ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public void MyMethod()
    {
        var tenantId = _currentTenant.Id;
        var tenantName = _currentTenant.Name;
    }
}
```

Additionally, you can use the [DataFilter](../../framework/infrastructure/data-filtering.md#idatafilter-service-enabledisable-data-filters) system to disable the tenant filter and list all data in the same database.

```csharp
public class MyBookService : ITransientDependency
{
    private readonly IDataFilter<IMultiTenant> _multiTenantFilter;
    private readonly IRepository<Book, Guid> _bookRepository;

    public MyBookService(
        IDataFilter<IMultiTenant> multiTenantFilter,
        IRepository<Book, Guid> bookRepository)
    {
        _multiTenantFilter = multiTenantFilter;
        _bookRepository = bookRepository;
    }

    public async Task<List<Book>> GetAllBooksIncludingDeletedAsync()
    {
        //Temporary disable the IMultiTenant filter
        using (_multiTenantFilter.Disable())
        {
            return await _bookRepository.GetListAsync();
        }
    }
}
```