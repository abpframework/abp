# Implementing Multiple Global Query Filters with Entity Framework Core

Global query filters are one of Entity Framework Core's most powerful features for automatically filtering data based on certain conditions. They allow you to define filter criteria at the entity level that are automatically applied to all LINQ queries, making it impossible for developers to accidentally forget to include important filtering logic. In this article, we'll explore how to implement multiple global query filters in ABP Framework, covering built-in filters, custom filters, and performance optimization techniques.

By the end of this guide, you'll understand how ABP Framework's data filtering system works, how to create custom global query filters for your specific business requirements, how to combine multiple filters effectively, and how to optimize filter performance using user-defined functions.

## Understanding Global Query Filters in EF Core

Global query filters were introduced in EF Core 2.0 and allow you to automatically append LINQ predicates to queries generated for an entity type. This is particularly useful for scenarios like multi-tenancy, soft delete, data isolation, and row-level security.

In traditional applications, developers must remember to add filter conditions manually to every query:

```csharp
// Manual filtering - error-prone and tedious
var activeBooks = await _bookRepository
    .GetListAsync(b => b.IsDeleted == false && b.TenantId == currentTenantId);
```

With global query filters, this logic is applied automatically:

```csharp
// Filter is applied automatically - no manual filtering needed
var activeBooks = await _bookRepository.GetListAsync();
```

ABP Framework provides a sophisticated data filtering system built on top of EF Core's global query filters, with built-in support for soft delete, multi-tenancy, and the ability to easily create custom filters.

### Important: Plain EF Core vs ABP Composition

In plain EF Core, calling `HasQueryFilter` multiple times for the same entity does **not** create multiple active filters. The last call replaces the previous one (unless you use newer named-filter APIs in recent EF Core versions).

ABP provides `HasAbpQueryFilter` to compose query filters safely. This method combines your custom filter with ABP's built-in filters (such as `ISoftDelete` and `IMultiTenant`) and with other `HasAbpQueryFilter` calls.

## ABP Framework's Data Filtering System

ABP's data filtering system is defined in the `Volo.Abp.Data` namespace and provides a consistent way to manage filters across your application. The core interface is `IDataFilter<TFilter>`, which allows you to enable or disable filters programmatically.

### Built-in Filters

ABP Framework comes with several built-in filters:

1. **ISoftDelete**: Automatically filters out soft-deleted entities
2. **IMultiTenant**: Automatically filters entities by current tenant (for SaaS applications)
3. **IIsActive**: Filters entities based on active status

Let's look at how these are implemented in the ABP framework:

The `ISoftDelete` interface is straightforward:

```csharp
namespace Volo.Abp;

public interface ISoftDelete
{
    bool IsDeleted { get; }
}
```

Any entity implementing this interface will automatically have deleted records filtered out of queries.

### Enabling and Disabling Filters

ABP provides the `IDataFilter<TFilter>` service to control filter behavior at runtime:

```csharp
public class BookAppService : ApplicationService
{
    private readonly IDataFilter<ISoftDelete> _softDeleteFilter;
    private readonly IRepository<Book, Guid> _bookRepository;

    public BookAppService(
        IDataFilter<ISoftDelete> softDeleteFilter,
        IRepository<Book, Guid> bookRepository)
    {
        _softDeleteFilter = softDeleteFilter;
        _bookRepository = bookRepository;
    }

    public async Task<List<Book>> GetAllBooksIncludingDeletedAsync()
    {
        // Temporarily disable the soft delete filter
        using (_softDeleteFilter.Disable())
        {
            return await _bookRepository.GetListAsync();
        }
    }

    public async Task<List<Book>> GetActiveBooksAsync()
    {
        // Filter is enabled by default - soft-deleted items are excluded
        return await _bookRepository.GetListAsync();
    }
}
```

You can also check if a filter is enabled and enable/disable it programmatically:

```csharp
public async Task ProcessBooksAsync()
{
    // Check if filter is enabled
    if (_softDeleteFilter.IsEnabled)
    {
        // Enable or disable explicitly
        _softDeleteFilter.Enable();
        // or
        _softDeleteFilter.Disable();
    }
}
```

## Creating Custom Global Query Filters

Now let's create custom global query filters for a real-world scenario. Imagine we have a library management system where we need to filter books based on:

1. **Publication Status**: Only show published books in public areas
2. **User's Department**: Users can only see books from their department
3. **Approval Status**: Only show approved content

### Step 1: Define Filter Interfaces

First, create the filter interfaces. You can define them in the same file as your entity or in separate files:

```csharp
// Can be placed in the same file as Book entity or in separate files
namespace Library;

public interface IPublishable
{
    bool IsPublished { get; }
    DateTime PublishDate { get; set; }
}

public interface IDepartmentRestricted
{
    Guid DepartmentId { get; }
}

public interface IApproveable
{
    bool IsApproved { get; }
}

public interface IPublishedFilter
{
}

public interface IApprovedFilter
{
}
```

`IPublishable` / `IApproveable` are implemented by entities and define entity properties.
`IPublishedFilter` / `IApprovedFilter` are filter-state interfaces used with `IDataFilter` so you can enable/disable those filters at runtime.

### Step 2: Add Filter Expressions to DbContext

Now let's add the filter expressions to your existing DbContext. First, here's how to use `HasAbpQueryFilter` to create **always-on** filters (they cannot be toggled at runtime):

```csharp
// MyProjectDbContext.cs
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Authorization;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Library;

public class LibraryDbContext : AbpDbContext<LibraryDbContext>
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Author> Authors { get; set; }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Book>(b =>
        {
            b.ToTable("Books");
            b.ConfigureByConvention();

            // HasAbpQueryFilter creates ALWAYS-ACTIVE filters
            // These cannot be toggled at runtime via IDataFilter
            b.HasAbpQueryFilter(book =>
                book.IsPublished &&
                book.PublishDate <= DateTime.UtcNow);

            b.HasAbpQueryFilter(book => book.IsApproved);
        });

        builder.Entity<Department>(b =>
        {
            b.ToTable("Departments");
            b.ConfigureByConvention();
        });
    }
}
```

> **Note:** Using `HasAbpQueryFilter` alone creates filters that are always active and cannot be toggled at runtime. This approach is simpler but less flexible. For toggleable filters, see Step 3 below.

### Step 3: Make Filters Toggleable (Optional)

If you need filters that can be enabled/disabled at runtime via `IDataFilter<T>`, override `ShouldFilterEntity` and `CreateFilterExpression` instead of (or in addition to) `HasAbpQueryFilter`:

```csharp
// MyProjectDbContext.cs
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore;

namespace Library;

public class LibraryDbContext : AbpDbContext<LibraryDbContext>
{
    protected bool IsPublishedFilterEnabled => DataFilter?.IsEnabled<IPublishedFilter>() ?? false;
    protected bool IsApprovedFilterEnabled => DataFilter?.IsEnabled<IApprovedFilter>() ?? false;

    protected override bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
    {
        if (typeof(IPublishable).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        if (typeof(IApproveable).IsAssignableFrom(typeof(TEntity)))
        {
            return true;
        }

        return base.ShouldFilterEntity<TEntity>(entityType);
    }

    protected override Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>(
        ModelBuilder modelBuilder,
        EntityTypeBuilder<TEntity> entityTypeBuilder)
        where TEntity : class
    {
        var expression = base.CreateFilterExpression<TEntity>(modelBuilder, entityTypeBuilder);

        if (typeof(IPublishable).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>> publishFilter = e =>
                !IsPublishedFilterEnabled ||
                (
                    EF.Property<bool>(e, nameof(IPublishable.IsPublished)) &&
                    EF.Property<DateTime>(e, nameof(IPublishable.PublishDate)) <= DateTime.UtcNow
                );

            expression = expression == null
                ? publishFilter
                : QueryFilterExpressionHelper.CombineExpressions(expression, publishFilter);
        }

        if (typeof(IApproveable).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>> approvalFilter = e =>
                !IsApprovedFilterEnabled || EF.Property<bool>(e, nameof(IApproveable.IsApproved));

            expression = expression == null
                ? approvalFilter
                : QueryFilterExpressionHelper.CombineExpressions(expression, approvalFilter);
        }

        return expression;
    }
}
```

This mapping step is what connects `IDataFilter<IPublishedFilter>` and `IDataFilter<IApprovedFilter>` to entity-level predicates. Without this step, `HasAbpQueryFilter` expressions remain always active.

> **Important:** Note that we use `DateTime` (not `DateTime?`) in the filter expression to match the entity property type. Adjust accordingly if your entity uses nullable `DateTime?`.

### Step 4: Disable Custom Filters with IDataFilter

Once custom filters are mapped to the ABP data-filter pipeline, you can disable them just like built-in filters:

```csharp
public class BookAppService : ApplicationService
{
    private readonly IRepository<Book, Guid> _bookRepository;
    private readonly IDataFilter<IPublishedFilter> _publishedFilter;
    private readonly IDataFilter<IApprovedFilter> _approvedFilter;

    public BookAppService(
        IRepository<Book, Guid> bookRepository,
        IDataFilter<IPublishedFilter> publishedFilter,
        IDataFilter<IApprovedFilter> approvedFilter)
    {
        _bookRepository = bookRepository;
        _publishedFilter = publishedFilter;
        _approvedFilter = approvedFilter;
    }

    public async Task<List<Book>> GetIncludingUnpublishedAndUnapprovedAsync()
    {
        using (_publishedFilter.Disable())
        using (_approvedFilter.Disable())
        {
            return await _bookRepository.GetListAsync();
        }
    }
}
```

## Advanced: Multiple Filters with User-Defined Functions

Starting from ABP v8.3, you can use user-defined function (UDF) mapping for better performance. This approach generates more efficient SQL and allows EF Core to create better execution plans.

### Step 1: Enable UDF Mapping

First, configure your module to use UDF mapping:

```csharp
// MyProjectModule.cs
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.GlobalFilters;
using Microsoft.Extensions.DependencyInjection;

namespace Library;

[DependsOn(
    typeof(AbpEntityFrameworkCoreModule),
    typeof(AbpDddDomainModule)
)]
public class LibraryModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpEfCoreGlobalFilterOptions>(options =>
        {
            options.UseDbFunction = true; // Enable UDF mapping
        });
    }
}
```

### Step 2: Define DbFunctions

Create static methods that EF Core will map to database functions:

```csharp
// LibraryDbFunctions.cs
using Microsoft.EntityFrameworkCore;

namespace Library;

public static class LibraryDbFunctions
{
    public static bool IsPublishedFilter(bool isPublished, DateTime? publishDate)
    {
        return isPublished && (publishDate == null || publishDate <= DateTime.UtcNow);
    }

    public static bool IsApprovedFilter(bool isApproved)
    {
        return isApproved;
    }

    public static bool DepartmentFilter(Guid entityDepartmentId, Guid userDepartmentId)
    {
        return entityDepartmentId == userDepartmentId;
    }
}
```

### Step 4: Apply UDF Filters

Update your DbContext to use the UDF-based filters:

```csharp
// MyProjectDbContext.cs
protected override void OnModelCreating(ModelBuilder builder)
{
    base.OnModelCreating(builder);

    // Map CLR methods to SQL scalar functions.
    // Create matching SQL functions in a migration.
    var isPublishedMethod = typeof(LibraryDbFunctions).GetMethod(
        nameof(LibraryDbFunctions.IsPublishedFilter),
        new[] { typeof(bool), typeof(DateTime?) })!;
    builder.HasDbFunction(isPublishedMethod);

    var isApprovedMethod = typeof(LibraryDbFunctions).GetMethod(
        nameof(LibraryDbFunctions.IsApprovedFilter),
        new[] { typeof(bool) })!;
    builder.HasDbFunction(isApprovedMethod);

    builder.Entity<Book>(b =>
    {
        b.ToTable("Books");
        b.ConfigureByConvention();

        // ABP way: define separate filters. HasAbpQueryFilter composes them.
        b.HasAbpQueryFilter(book =>
            LibraryDbFunctions.IsPublishedFilter(book.IsPublished, book.PublishDate));

        b.HasAbpQueryFilter(book =>
            LibraryDbFunctions.IsApprovedFilter(book.IsApproved));
    });
}
```

This approach generates cleaner SQL and improves query performance, especially in complex scenarios with multiple filters.

## Working with Complex Filter Combinations

When combining multiple filters, it's important to understand how they interact. Let's explore some common scenarios.

### Combining Tenant and Department Filters

In a multi-tenant application, you might need to combine tenant isolation with department-level access control:

```csharp
public class BookAppService : ApplicationService
{
    private readonly IRepository<Book, Guid> _bookRepository;
    private readonly IDataFilter<IMultiTenant> _tenantFilter;
    private readonly ICurrentUser _currentUser;

    public BookAppService(
        IRepository<Book, Guid> bookRepository,
        IDataFilter<IMultiTenant> tenantFilter,
        ICurrentUser currentUser)
    {
        _bookRepository = bookRepository;
        _tenantFilter = tenantFilter;
        _currentUser = currentUser;
    }

    public async Task<List<BookDto>> GetMyDepartmentBooksAsync()
    {
        var currentUser = _currentUser;
        var userDepartmentId = GetUserDepartmentId(currentUser);

        // Get all books without department filter, then filter in memory
        // (for scenarios where you need custom filter logic)
        using (_tenantFilter.Disable()) // Optional: disable tenant filter if needed
        {
            var allBooks = await _bookRepository.GetListAsync();
            
            // Apply department filter in memory (custom logic)
            var departmentBooks = allBooks
                .Where(b => b.DepartmentId == userDepartmentId)
                .ToList();

            return ObjectMapper.Map<List<Book>, List<BookDto>>(departmentBooks);
        }
    }

    private Guid GetUserDepartmentId(ICurrentUser currentUser)
    {
        // Get user's department from claims or database
        var departmentClaim = currentUser.FindClaim("DepartmentId");
        return Guid.Parse(departmentClaim.Value);
    }
}
```

### Filter Priority and Override

Sometimes you need to override filters in specific scenarios. ABP provides a flexible way to handle this:

```csharp
public async Task<Book> GetBookForEditingAsync(Guid id)
{
    // Disable soft delete filter to get deleted records for restoration
    using (DataFilter.Disable<ISoftDelete>())
    {
        return await _bookRepository.GetAsync(id);
    }
}

public async Task<Book> GetBookIncludingUnpublishedAsync(Guid id)
{
    // Use GetQueryableAsync to customize the query
    var query = await _bookRepository.GetQueryableAsync();
    
    // Manually apply or bypass filters
    var book = await query
        .FirstOrDefaultAsync(b => b.Id == id);

    return book;
}
```

## Best Practices for Multiple Global Query Filters

When implementing multiple global query filters, consider these best practices:

### 1. Keep Filters Simple

Complex filter expressions can significantly impact query performance. Keep each condition focused on a single concern. In ABP, you can define them separately with `HasAbpQueryFilter`, which composes with ABP's built-in filters:

```csharp
// Good (ABP): separate, focused filters composed by HasAbpQueryFilter
b.HasAbpQueryFilter(b => b.IsPublished);
b.HasAbpQueryFilter(b => b.IsApproved);
b.HasAbpQueryFilter(b => b.DepartmentId == userDeptId);

// Avoid: calling HasQueryFilter multiple times for the same entity
// in plain EF Core (the last call replaces the previous one)
b.HasQueryFilter(b => b.IsPublished);
b.HasQueryFilter(b => b.IsApproved);
```

### 2. Use Indexing

Ensure your database has appropriate indexes for filtered columns:

```csharp
builder.Entity<Book>(b =>
{
    b.HasIndex(b => b.IsPublished);
    b.HasIndex(b => b.IsApproved);
    b.HasIndex(b => b.DepartmentId);
    b.HasIndex(b => new { b.IsPublished, b.PublishDate });
});
```

### 3. Consider Performance Impact

Use UDF mapping for better performance with complex filters. Profile your queries and analyze execution plans.

### 4. Document Filter Behavior

Clearly document which filters are applied to each entity to help developers understand the behavior:

```csharp
/// <summary>
/// Book entity with the following global query filters:
/// - ISoftDelete: Automatically excludes soft-deleted books
/// - IMultiTenant: Automatically filters by current tenant
/// - IPublishable: Excludes unpublished books (based on IsPublished and PublishDate)
/// - IApproveable: Excludes unapproved books (based on IsApproved)
/// </summary>
/// <remarks>
/// Filter interfaces (IPublishable, IApproveable, IPublishedFilter, IApprovedFilter)
/// are defined in Step 1: Define Filter Interfaces
/// </remarks>
public class Book : AuditedAggregateRoot<Guid>, ISoftDelete, IMultiTenant, IPublishable, IApproveable
{
    public string Name { get; set; }

    public BookType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }

    public bool IsPublished { get; set; }

    public bool IsApproved { get; set; }

    public Guid? TenantId { get; set; }

    public bool IsDeleted { get; set; }

    public Guid DepartmentId { get; set; }
}
```

## Testing Global Query Filters

Testing with global query filters can be challenging. Here's how to do it effectively:

### Unit Testing Filters

```csharp
[Fact]
public void Book_QueryFilter_Should_Filter_Unpublished()
{
    var options = new DbContextOptionsBuilder<BookStoreDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDb")
        .Options;

    using (var context = new BookStoreDbContext(options))
    {
        context.Books.Add(new Book { Name = "Published Book", IsPublished = true });
        context.Books.Add(new Book { Name = "Unpublished Book", IsPublished = false });
        context.SaveChanges();
    }

    using (var context = new BookStoreDbContext(options))
    {
        // Query with filter enabled (default)
        var publishedBooks = context.Books.ToList();
        Assert.Single(publishedBooks);
        Assert.Equal("Published Book", publishedBooks[0].Name);
    }
}
```

### Integration Testing with Filter Control

```csharp
[Fact]
public async Task Should_Get_Deleted_Book_When_Filter_Disabled()
{
    var dataFilter = GetRequiredService<IDataFilter>();

    // Arrange
    var book = await _bookRepository.InsertAsync(
        new Book { Name = "Test Book" },
        autoSave: true
    );

    await _bookRepository.DeleteAsync(book);

    // Act - with filter disabled
    using (dataFilter.Disable<ISoftDelete>())
    {
        var deletedBook = await _bookRepository
            .FirstOrDefaultAsync(b => b.Id == book.Id);

        deletedBook.ShouldNotBeNull();
        deletedBook.IsDeleted.ShouldBeTrue();
    }
}
```

### Testing Custom Global Query Filters

Here's a complete example of testing custom toggleable filters:

```csharp
[Fact]
public async Task Should_Filter_Unpublished_Books_By_Default()
{
    // Default: filters are enabled
    var result = await WithUnitOfWorkAsync(async () =>
    {
        var bookRepository = GetRequiredService<IRepository<Book, Guid>>();
        return await bookRepository.GetListAsync();
    });

    // Only published and approved books should be returned
    result.All(b => b.IsPublished).ShouldBeTrue();
    result.All(b => b.IsApproved).ShouldBeTrue();
}

[Fact]
public async Task Should_Return_All_Books_When_Filter_Disabled()
{
    var result = await WithUnitOfWorkAsync(async () =>
    {
        // Disable the published filter to see unpublished books
        using (_publishedFilter.Disable())
        {
            var bookRepository = GetRequiredService<IRepository<Book, Guid>>();
            return await bookRepository.GetListAsync();
        }
    });

    // Should include unpublished books
    result.Any(b => b.Name == "Unpublished Book").ShouldBeTrue();
}

[Fact]
public async Task Should_Combine_Filters_Correctly()
{
    // Test combining multiple filter disables
    using (_publishedFilter.Disable())
    using (_approvedFilter.Disable())
    {
        var bookRepository = GetRequiredService<IRepository<Book, Guid>>();
        var allBooks = await bookRepository.GetListAsync();
        
        // All books should be visible
        allBooks.Count.ShouldBe(5);
    }
}
```

> **Tip:** When using ABP's test base, inject `IDataFilter<IPublishedFilter>` and `IDataFilter<IApprovedFilter>` to control filters in your tests.

## Key Takeaways

✅ **Global query filters automatically apply filter criteria to all queries**, reducing developer error and ensuring consistent data filtering across your application.

✅ **ABP Framework provides a sophisticated data filtering system** with built-in support for soft delete (`ISoftDelete`) and multi-tenancy (`IMultiTenant`), plus the ability to create custom filters.

✅ **Use `IDataFilter<TFilter>` to control filters at runtime**, enabling or disabling filters as needed for specific operations.

✅ **To make custom filters toggleable, override `ShouldFilterEntity` and `CreateFilterExpression`** in your DbContext. Using only `HasAbpQueryFilter` creates filters that are always active.

✅ **Combine multiple filters carefully** and consider performance implications, especially with complex filter expressions.

✅ **Leverage user-defined function (UDF) mapping** for better SQL generation and query performance, available since ABP v8.3.

✅ **Always test filter behavior** to ensure filters work as expected in different scenarios, including edge cases.

## Conclusion

Global query filters are essential for building secure, well-isolated applications. ABP Framework's data filtering system provides a robust foundation that builds on EF Core's capabilities while adding convenient features like runtime filter control and UDF mapping optimization.

By implementing multiple global query filters strategically, you can ensure data isolation, simplify your query logic, and reduce the risk of accidentally exposing unauthorized data. Remember to keep filters simple, add appropriate database indexes, and test thoroughly to maintain optimal performance.

Start implementing global query filters in your ABP applications today to leverage automatic data filtering across all your repositories and queries.

### See Also

- [ABP Data Filtering Documentation](https://abp.io/docs/latest/framework/fundamentals/data-filtering)
- [EF Core Global Query Filters](https://learn.microsoft.com/en-us/ef/core/querying/filters)
- [ABP Multi-Tenancy Documentation](https://abp.io/docs/latest/framework/fundamentals/multi-tenancy)
- [Using User-defined function mapping for global filters](https://abp.io/docs/latest/framework/infrastructure/data-filtering#using-user-defined-function-mapping-for-global-filters)

---

## References

- [ABP Framework Documentation](https://docs.abp.io)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [EF Core Global Query Filters](https://learn.microsoft.com/en-us/ef/core/querying/filters)
- [User-defined Function Mapping](https://learn.microsoft.com/en-us/ef/core/querying/user-defined-function-mapping)
