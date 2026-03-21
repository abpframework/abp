---
name: abp-module
description: ABP reusable Module solution template - EF Core + MongoDB dual support, virtual methods for extensibility, DbTablePrefix, module options pattern, entity extension, separate connection string. Use when building or reviewing reusable ABP modules that will be distributed or consumed by other solutions.
---

# ABP Module Solution Template

> **Docs**: https://abp.io/docs/latest/solution-templates/application-module

This template is for developing reusable ABP modules. Key requirement: **extensibility** - consumers must be able to override and customize module behavior.

## Solution Structure

```
MyModule/
├── src/
│   ├── MyModule.Domain.Shared/      # Constants, enums, localization
│   ├── MyModule.Domain/             # Entities, repository interfaces, domain services
│   ├── MyModule.Application.Contracts/ # DTOs, service interfaces
│   ├── MyModule.Application/        # Service implementations
│   ├── MyModule.EntityFrameworkCore/ # EF Core implementation
│   ├── MyModule.MongoDB/            # MongoDB implementation
│   ├── MyModule.HttpApi/            # REST controllers
│   ├── MyModule.HttpApi.Client/     # Client proxies
│   ├── MyModule.Web/                # MVC/Razor Pages UI
│   └── MyModule.Blazor/             # Blazor UI
├── test/
│   └── MyModule.Tests/
└── host/
    └── MyModule.HttpApi.Host/       # Test host application
```

## Database Independence

Support both EF Core and MongoDB:

### Repository Interface (Domain)
```csharp
public interface IBookRepository : IRepository<Book, Guid>
{
    Task<Book> FindByNameAsync(string name);
    Task<List<Book>> GetListByAuthorAsync(Guid authorId);
}
```

### EF Core Implementation
```csharp
public class BookRepository : EfCoreRepository<MyModuleDbContext, Book, Guid>, IBookRepository
{
    public async Task<Book> FindByNameAsync(string name)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(b => b.Name == name);
    }
}
```

### MongoDB Implementation
```csharp
public class BookRepository : MongoDbRepository<MyModuleMongoDbContext, Book, Guid>, IBookRepository
{
    public async Task<Book> FindByNameAsync(string name)
    {
        var queryable = await GetQueryableAsync();
        return await queryable.FirstOrDefaultAsync(b => b.Name == name);
    }
}
```

## Table/Collection Prefix

Allow customization to avoid naming conflicts:

```csharp
// Domain.Shared
public static class MyModuleDbProperties
{
    public static string DbTablePrefix { get; set; } = "MyModule";
    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "MyModule";
}
```

Usage:
```csharp
builder.Entity<Book>(b =>
{
    b.ToTable(MyModuleDbProperties.DbTablePrefix + "Books", MyModuleDbProperties.DbSchema);
});
```

## Module Options

Provide configuration options:

```csharp
// Domain
public class MyModuleOptions
{
    public bool EnableFeatureX { get; set; } = true;
    public int MaxItemCount { get; set; } = 100;
}
```

Usage in module:
```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<MyModuleOptions>(options =>
    {
        options.EnableFeatureX = true;
    });
}
```

Usage in service:
```csharp
public class MyService : ITransientDependency
{
    private readonly MyModuleOptions _options;

    public MyService(IOptions<MyModuleOptions> options)
    {
        _options = options.Value;
    }
}
```

## Extensibility Points

### Virtual Methods (Critical for Modules!)
When developing a reusable module, **all public and protected methods must be virtual** to allow consumers to override behavior:

```csharp
public class BookAppService : ApplicationService, IBookAppService
{
    // ✅ Public methods MUST be virtual
    public virtual async Task<BookDto> CreateAsync(CreateBookDto input)
    {
        var book = await CreateBookEntityAsync(input);
        await _bookRepository.InsertAsync(book);
        return _bookMapper.MapToDto(book);
    }

    // ✅ Use protected virtual for helper methods (not private)
    protected virtual Task<Book> CreateBookEntityAsync(CreateBookDto input)
    {
        return Task.FromResult(new Book(
            GuidGenerator.Create(),
            input.Name,
            input.Price
        ));
    }

    // ❌ WRONG for modules - private methods cannot be overridden
    // private Book CreateBook(CreateBookDto input) { ... }
}
```

This allows module consumers to:
- Override specific methods without copying entire class
- Extend functionality while preserving base behavior
- Customize module behavior for their needs

### Entity Extension
Support object extension system:
```csharp
public class MyModuleModuleExtensionConfigurator
{
    public static void Configure()
    {
        OneTimeRunner.Run(() =>
        {
            ObjectExtensionManager.Instance.Modules()
                .ConfigureMyModule(module =>
                {
                    module.ConfigureBook(book =>
                    {
                        book.AddOrUpdateProperty<string>("CustomProperty");
                    });
                });
        });
    }
}
```

## Localization

```csharp
// Domain.Shared
[LocalizationResourceName("MyModule")]
public class MyModuleResource
{
}

// Module configuration
Configure<AbpLocalizationOptions>(options =>
{
    options.Resources
        .Add<MyModuleResource>("en")
        .AddVirtualJson("/Localization/MyModule");
});
```

## Permission Definition

```csharp
public class MyModulePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(
            MyModulePermissions.GroupName,
            L("Permission:MyModule"));

        myGroup.AddPermission(
            MyModulePermissions.Books.Default,
            L("Permission:Books"));
    }
}
```

## Best Practices

1. **Virtual methods** - All public/protected methods must be `virtual` for extensibility
2. **Protected virtual helpers** - Use `protected virtual` instead of `private` for helper methods
3. **Database agnostic** - Support both EF Core and MongoDB
4. **Configurable** - Use options pattern for customization
5. **Localizable** - Use localization for all user-facing text
6. **Table prefix** - Allow customization to avoid conflicts
7. **Separate connection string** - Support dedicated database
8. **No dependencies on host** - Module should be self-contained
9. **Test with host app** - Include a host application for testing
