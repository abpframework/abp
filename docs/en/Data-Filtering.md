# Data Filtering

[Volo.Abp.Data](https://www.nuget.org/packages/Volo.Abp.Data) package defines services to automatically filter data on querying from a database.

## Pre-Defined Filters

ABP defines some filters out of the box.

### ISoftDelete

Used to mark an entity as deleted instead of actually deleting it. Implement the `ISoftDelete` interface to make your entity "soft delete".

Example:

````csharp
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Acme.BookStore
{
    public class Book : AggregateRoot<Guid>, ISoftDelete
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; } //Defined by ISoftDelete
    }
}
````

`ISoftDelete` defines the `IsDeleted` property. When you delete a book using [repositories](Repositories.md), ABP automatically sets `IsDeleted` to true and protects it from actual deletion (you can also manually set the `IsDeleted` property to true if you need). In addition, it **automatically filters deleted entities** when you query the database.

> `ISoftDelete` filter is enabled by default and you can not get deleted entities from database unless you explicitly disable it. See the `IDataFilter` service below.

### IMultiTenant

[Multi-tenancy](Multi-Tenancy.md) is an efficient way of creating SaaS applications. Once you create a multi-tenant application, you typically want to isolate data between tenants. Implement `IMultiTenant` interface to make your entity "multi-tenant aware".

Example:

````csharp
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Acme.BookStore
{
    public class Book : AggregateRoot<Guid>, ISoftDelete, IMultiTenant
    {
        public string Name { get; set; }

        public bool IsDeleted { get; set; } //Defined by ISoftDelete

        public Guid? TenantId { get; set; } //Defined by IMultiTenant
    }
}
````

`IMultiTenant` interface defines the `TenantId` property which is then used to automatically filter the entities for the current tenant. See the [Multi-tenancy](Multi-Tenancy.md) document for more.

## IDataFilter Service: Enable/Disable Data Filters

You can control the filters using `IDataFilter` service.

Example:

````csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore
{
    public class MyBookService : ITransientDependency
    {
        private readonly IDataFilter _dataFilter;
        private readonly IRepository<Book, Guid> _bookRepository;

        public MyBookService(
            IDataFilter dataFilter,
            IRepository<Book, Guid> bookRepository)
        {
            _dataFilter = dataFilter;
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> GetAllBooksIncludingDeletedAsync()
        {
            //Temporary disable the ISoftDelete filter
            using (_dataFilter.Disable<ISoftDelete>())
            {
                return await _bookRepository.GetListAsync();
            }
        }
    }
}
````

* [Inject](Dependency-Injection.md) the `IDataFilter` service to your class.
* Use the `Disable` method within a `using` statement to create a code block where the `ISoftDelete` filter is disabled inside it (Always use it inside a `using` block to guarantee that the filter is reset to its previous state).

`IDataFilter.Enable` method can be used to enable a filter. `Enable` and `Disable` methods can be used in a nested way to define inner scopes.

## AbpDataFilterOptions

`AbpDataFilterOptions` can be used to [set options](Options.md) for the data filter system.

The example code below disables the `ISoftDelete` filter by default which will cause to include deleted entities when you query the database unless you explicitly enable the filter:

````csharp
Configure<AbpDataFilterOptions>(options =>
{
    options.DefaultStates[typeof(ISoftDelete)] = new DataFilterState(isEnabled: false);
});
````

> Carefully change defaults for global filters, especially if you are using a pre-built module which might be developed assuming the soft delete filter is turned on by default. But you can do it for your own defined filters safely.