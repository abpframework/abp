# How to Dynamically Set the Connection String in EF Core

In modern web applications, there are scenarios where you need to determine which database to connect to at runtime rather than at compile time. This could be for multi-tenant applications, environment-specific configurations, or modular architectures where different parts of your application connect to different databases.

In this article, I'll walk you through creating a practical solution for dynamic connection string resolution by building a real ASP.NET Core application. We'll start with a standard template and gradually implement our own `IConnectionStringResolver` pattern.

> **Note**: The code examples are simplified for demonstration purposes. Production applications require additional error handling, logging, and caching.

## The Scenario: Building a Multi-Tenant Web Application

Let's imagine we're building a SaaS application where different tenants can have their own databases. Some tenants share a common database, while premium tenants get their own dedicated database for better performance and data isolation.

Our requirements:
- Default behavior: Use the standard connection string
- Multi-tenant support: Route tenants to their specific databases
- Fallback mechanism: If a tenant-specific database isn't available, use the default
- Simple tenant identification: Use query parameters for this example

> **Note**: We're building this from scratch to understand the concepts, but ABP Framework already handles all of this automatically - and does it much better! It supports separate databases for each tenant, different connection strings for different modules, and automatic fallback when connections aren't found. See how comprehensive ABP's approach is in the [Connection Strings documentation](https://abp.io/docs/latest/framework/fundamentals/connection-strings).

### Step 1: Creating the Project

First, create a new ASP.NET Core Web App with Razor Pages and Individual Authentication:

```bash
dotnet new webapp --auth Individual -n DynamicConnectionDemo
cd DynamicConnectionDemo
```

When you open the project, you'll see the default connection in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=app.db;Cache=Shared"
  }
}
```

And in `Program.cs`, you'll find the standard Entity Framework configuration:

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??  
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");  

builder.Services.AddDbContext<ApplicationDbContext>(options =>  
    options.UseSqlite(connectionString));  

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)  
    .AddEntityFrameworkStores<ApplicationDbContext>();
```

### Step 2: Testing the Base Application

Run the application and test the registration/login functionality to ensure everything works:

```bash
dotnet run
```

Navigate to the registration page, create an account, and verify that the basic authentication flow works correctly.

## Building Our Connection String Resolver

Now let's implement our dynamic connection string resolver. We'll start by defining the interface and implementation.

### Step 3: Creating the Interface

Create a new interface `IConnectionStringResolver` in `Data` folder:

```csharp
public interface IConnectionStringResolver
{
    string Resolve(string connectionName = null);
}
```

### Step 4: Implementing the Resolver

Create the `ConnectionStringResolver` class in `Data` folder:

```csharp
public class ConnectionStringResolver : IConnectionStringResolver
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ConnectionStringResolver(
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string Resolve(string connectionName = null)
    {
        // Add caching logic here if needed
        return GetConnectionString(connectionName);
    }

    private string GetConnectionString(string connectionName)
    {
        // Try to get given named connection string
        if (!string.IsNullOrEmpty(connectionName))
        {
            var connectionString = _configuration.GetConnectionString(connectionName);
            if (!string.IsNullOrEmpty(connectionString))
            {
                return connectionString;
            }
        }

        // Try to get tenant-specific connection string (for multi-tenant apps)
        var tenantId = GetCurrentTenantIdOrNull();
        if (!string.IsNullOrEmpty(tenantId))
        {
            var tenantConnectionString = _configuration.GetConnectionString($"Tenant_{tenantId}");
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                return tenantConnectionString;
            }
        }

        // Fallback to default connection string
        return _configuration.GetConnectionString("DefaultConnection");
    }

    private string? GetCurrentTenantIdOrNull()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
        {
            return null;
        }

        // Adds support for subdomain-based, route-based, or header-based tenant identification
        
        // Example: Query string-based tenant identification
        if (context.Request.Query.ContainsKey("tenant"))
        {
            return context.Request.Query["tenant"].ToString();
        }

        return null;
    }
}
```

### Step 5: Registering the Service

Add the service registration to your `Program.cs`:

```csharp
builder.Services.AddScoped<IConnectionStringResolver, ConnectionStringResolver>();
```

### Step 6: Updating the DbContext Configuration

Now we need to modify our `Program.cs` to use the resolver instead of the static connection string.

Replace this code:

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??  
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");  
builder.Services.AddDbContext<ApplicationDbContext>(options =>  
    options.UseSqlite(connectionString));
```

With this simpler version:

```csharp
builder.Services.AddDbContext<ApplicationDbContext>();
```

### Step 7: Modifying ApplicationDbContext

Update your `ApplicationDbContext` in `Data` folder to use the resolver:

```csharp
public class ApplicationDbContext : IdentityDbContext
{
    private readonly IConnectionStringResolver _connectionStringResolver;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConnectionStringResolver connectionStringResolver)
        : base(options)
    {
        _connectionStringResolver = connectionStringResolver;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _connectionStringResolver.Resolve();
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}
```

### Step 8: Testing the Implementation

Let's add a simple way to see our resolver in action. Update your `Pages/Index.cshtml`:

```html
@page
@model IndexModel
@inject IConnectionStringResolver ConnectionStringResolver

@{
    ViewData["Title"] = "Home page";
}

<div class="text-center">
    <h1 class="display-4">Welcome</h1>
    <p>Connection String: @ConnectionStringResolver.Resolve()</p>
</div>
```

### Step 9: Adding Multi-Tenant Configuration

To test the multi-tenant functionality, add some tenant-specific connection strings to your `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "DataSource=app.db;Cache=Shared",
    "Tenant_acme": "DataSource=acme.db;Cache=Shared",
    "Tenant_globex": "DataSource=globex.db;Cache=Shared"
  }
}
```

### Step 10: Testing Multi-Tenant Functionality

Now run your application and test the multi-tenant functionality:

1. **Default behavior**: Visit `https://localhost:5001/` - you should see the default connection string
2. **Tenant-specific**: Visit `https://localhost:5001/?tenant=acme` - you should see the ACME tenant's connection string
3. **Another tenant**: Visit `https://localhost:5001/?tenant=globex` - you should see the Globex tenant's connection string
4. **Non-existent tenant**: Visit `https://localhost:5001/?tenant=unknown` - you should see the default connection string (fallback behavior)

> **Note**: The port number may be different on your system. Check the console output when you run `dotnet run` to see the actual URL.

> **Important**: This demo shows that our connection string resolver is working correctly, but it only displays which connection string would be used. In a real application, thanks to our `ApplicationDbContext` modifications, the actual database operations would use the resolved connection string automatically. I kept this demo simple for clarity, but if you create actual tenant databases and test with real data operations, you'll see it works as expected.

## Understanding the Implementation

Let's break down what we've accomplished:

Our resolver follows this priority order:
1. **Named Connection**: If a specific connection name is provided, use that
2. **Tenant-Specific**: Check for tenant-specific connection strings  
3. **Default Fallback**: Use the default connection string

> **Production Note**: This example uses query string parameters for simplicity. In production, you might use subdomains (`acme.myapp.com`), custom headers (`X-Tenant-ID`), route parameters, or JWT claims for tenant identification. Also consider adding caching, proper error handling and so on.

## How ABP Framework Handles This

The approach we've implemented above is very similar to how ABP Framework handles dynamic connection strings. ABP provides a built-in `IConnectionStringResolver` that works almost identically to our custom implementation, but with additional enterprise features:

### ABP's IConnectionStringResolver

ABP Framework includes a sophisticated connection string resolver that:

- Automatically handles multi-tenancy scenarios
- Supports module-specific connection strings out of the box
- Integrates seamlessly with ABP's configuration system
- Provides advanced caching and performance optimizations

```csharp
// In ABP applications, you can simply inject IConnectionStringResolver
public class ProductService : ITransientDependency
{
    private readonly IConnectionStringResolver _connectionStringResolver;

    public ProductService(IConnectionStringResolver connectionStringResolver)
    {
        _connectionStringResolver = connectionStringResolver;
    }

    public async Task<string> GetConnectionStringAsync()
    {
        // ABP automatically handles tenant context, module resolution, and fallbacks
        return await _connectionStringResolver.ResolveAsync("ProductModule");
    }
}
```

ABP's version provides automatic tenant detection, module integration, and enterprise features out of the box.

## Conclusion

The `IConnectionStringResolver` pattern provides a clean way to handle dynamic connection strings in ASP.NET applications. By centralizing connection string logic, you can easily support multi-tenant scenarios, environment-specific configurations, and modular architectures.

This pattern is particularly valuable for applications that need to scale and adapt to different deployment scenarios. Whether you implement your own resolver or use ABP Framework's built-in solution, this approach will make your application more flexible.

## Further Reading

* [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
* [Multi-tenant Applications with EF Core](https://docs.microsoft.com/en-us/ef/core/miscellaneous/multitenancy)
* [Connection Strings and Configuration in .NET](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/connection-strings-and-configuration-files)
* [ABP Framework Multi-Tenancy](https://abp.io/docs/latest/framework/architecture/multi-tenancy)
