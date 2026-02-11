# Repository Pattern in the ASP.NET Core

If you’ve built a .NET app with a database, you’ve likely used Entity Framework, Dapper, or ADO.NET. They’re useful tools; still, when they live inside your business logic or controllers, the code can become harder to keep tidy and to test.

That’s where the **Repository Pattern** comes in.

At its core, the Repository Pattern acts as a **middle layer between your domain and data access logic**. It abstracts the way you store and retrieve data, giving your application a clean separation of concerns:

* **Separation of Concerns:** Business logic doesn’t depend on the database.
* **Easier Testing:** You can replace the repository with a fake or mock during unit tests.
* **Flexibility:** You can switch data sources (e.g., from SQL to MongoDB) without touching business logic.

Let’s see how this works with a simple example.

## A Simple Example with Product Repository

Imagine we’re building a small e-commerce app. We’ll start by defining a repository interface for managing products.

You can find the complete sample code in this GitHub repository:

https://github.com/m-aliozkaya/RepositoryPattern

### Domain model and context

We start with a single entity and a matching `DbContext`.

`Product.cs`

```csharp
using System.ComponentModel.DataAnnotations;

namespace RepositoryPattern.Web.Models;

public class Product
{
    public int Id { get; set; }

    [Required, StringLength(64)]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [StringLength(256)]
    public string? Description { get; set; }

    public int Stock { get; set; }
}
```

`"AppDbContext.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Web.Models;

namespace RepositoryPattern.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
}
```

### Generic repository contract and base class

All entities share the same CRUD needs, so we define a generic interface and an EF Core implementation.

`Repositories/IRepository.cs`

```csharp
using System.Linq.Expressions;

namespace RepositoryPattern.Web.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
```

`Repositories/EfRepository.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Web.Data;

namespace RepositoryPattern.Web.Repositories;

public class EfRepository<TEntity>(AppDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    protected readonly AppDbContext Context = context;

    public virtual async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await Context.Set<TEntity>().FindAsync([id], cancellationToken);

    public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Context.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);

    public virtual async Task<List<TEntity>> GetListAsync(
        System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await Context.Set<TEntity>()
            .AsNoTracking()
            .Where(predicate)
            .ToListAsync(cancellationToken);

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Context.Set<TEntity>().Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            return;
        }

        Context.Set<TEntity>().Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }
}
```

Reads use `AsNoTracking()` to avoid tracking overhead, while write methods call `SaveChangesAsync` to keep the sample straightforward.

### Product-specific repository

Products need one extra query: list the items that are almost out of stock. We extend the generic repository with a dedicated interface and implementation.

`Repositories/IProductRepository.cs`

```csharp
using RepositoryPattern.Web.Models;

namespace RepositoryPattern.Web.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetLowStockProductsAsync(int threshold, CancellationToken cancellationToken = default);
}
```

`Repositories/ProductRepository.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using RepositoryPattern.Web.Data;
using RepositoryPattern.Web.Models;

namespace RepositoryPattern.Web.Repositories;

public class ProductRepository(AppDbContext context) : EfRepository<Product>(context), IProductRepository
{
    public Task<List<Product>> GetLowStockProductsAsync(int threshold, CancellationToken cancellationToken = default) =>
        Context.Products
            .AsNoTracking()
            .Where(product => product.Stock <= threshold)
            .OrderBy(product => product.Stock)
            .ToListAsync(cancellationToken);
}
```

### 🧩 A Note on Unit of Work

The Repository Pattern is often used together with the **Unit of Work** pattern to manage transactions efficiently.

> 💡 *If you want to dive deeper into the Unit of Work pattern, check out our separate blog post dedicated to that topic. https://abp.io/community/articles/lv4v2tyf

### Service layer and controller

Controllers depend on a service, and the service depends on the repository. That keeps HTTP logic and data logic separate.

`Services/ProductService.cs`

```csharp
using RepositoryPattern.Web.Models;
using RepositoryPattern.Web.Repositories;

namespace RepositoryPattern.Web.Services;

public class ProductService(IProductRepository productRepository)
{
    private readonly IProductRepository _productRepository = productRepository;

    public Task<List<Product>> GetProductsAsync(CancellationToken cancellationToken = default) =>
        _productRepository.GetAllAsync(cancellationToken);

    public Task<List<Product>> GetLowStockAsync(int threshold, CancellationToken cancellationToken = default) =>
        _productRepository.GetLowStockProductsAsync(threshold, cancellationToken);

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        _productRepository.GetByIdAsync(id, cancellationToken);

    public Task CreateAsync(Product product, CancellationToken cancellationToken = default) =>
        _productRepository.AddAsync(product, cancellationToken);

    public Task UpdateAsync(Product product, CancellationToken cancellationToken = default) =>
        _productRepository.UpdateAsync(product, cancellationToken);

    public Task DeleteAsync(int id, CancellationToken cancellationToken = default) =>
        _productRepository.DeleteAsync(id, cancellationToken);
}
```

`Controllers/ProductsController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;
using RepositoryPattern.Web.Models;
using RepositoryPattern.Web.Services;

namespace RepositoryPattern.Web.Controllers;

public class ProductsController(ProductService productService) : Controller
{
    private readonly ProductService _productService = productService;

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        const int lowStockThreshold = 5;
        var products = await _productService.GetProductsAsync(cancellationToken);
        var lowStock = await _productService.GetLowStockAsync(lowStockThreshold, cancellationToken);

        return View(new ProductListViewModel(products, lowStock, lowStockThreshold));
    }

    // remaining CRUD actions call through ProductService in the same way
}
```

The controller never reaches for `AppDbContext`. Every operation travels through the service, which keeps tests simple and makes future refactors easier.

### Dependency registration and seeding

The last step is wiring everything up in `Program.cs`.

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProductsDb"));
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();
```

The sample also seeds three products so the list page shows data on first run.

Run the site with:

```powershell
dotnet run --project RepositoryPattern.Web
```

## How ABP approaches the same idea

ABP includes generic repositories by default (`IRepository<TEntity, TKey>`), so you often skip writing the implementation layer shown above. You inject the interface into an application service, call methods like `InsertAsync` or `CountAsync`, and ABP’s Unit of Work handles the transaction. When you need custom queries, you can still derive from `EfCoreRepository<TEntity, TKey>` and add them.

For more details, check out the official ABP documentation on repositories: https://abp.io/docs/latest/framework/architecture/domain-driven-design/repositories

### Closing note

This setup keeps data access tidy without being heavy. Start with the generic repository, add small extensions per entity, pass everything through services, and register the dependencies once. Whether you hand-code it or let ABP supply the repository, the structure stays the same and your controllers remain clean.
