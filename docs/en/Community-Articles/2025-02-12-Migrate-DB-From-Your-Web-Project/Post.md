In some scenarios, you might prefer to **update your ABP project's database by simply visiting a URL** rather than running a console application (such as a typical `DbMigrator` project). This post demonstrates how to do that in an ABP application. In my sample application I used the single-layer project but you can benefit the same approach for other templates. My sample project name was created with this name: **Acme.BookStore**.

------

## Step 1: Remove the Existing Localization Middleware

Open your web project's module class file—this is typically in `BookStoreModule.cs` if you’re using the no-layer template. 
Or if your project is multi-layered, it's `BookStoreWebModule.cs`. Find the below line and **remove** it:

```csharp
app.UseAbpRequestLocalization();
```

The reason is that the localization middleware depends on the database, which does not yet exist! 
We'll add this middleware conditionally in the next step.

------

## Step 2: Conditionally Use the Localization Middleware

We only want `AbpRequestLocalizationMiddleware` for routes **other than** our new migration endpoint (`/api/migrate`). To achieve this, we use [Middleware Branching](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/). Our new controller will work on `/api/migrate` endpoint, so let's add a condition for this specific URL.

```csharp
app.UseWhen(httpContext =>
    !httpContext.Request.Path.StartsWithSegments("/api/migrate", StringComparison.OrdinalIgnoreCase),
    appBuilder =>
    {
        appBuilder.UseAbpRequestLocalization();
    }
);
```

This ensures all requests except `/api/migrate` will still use `AbpRequestLocalization` middleware.

------

## Step 3: Create the Migration Controller

Create a new file named `MigrationController.cs` under your `Controllers` folder or wherever you store controllers. 
Then copy-paste the following class:

```csharp
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Saas.Tenants;

namespace Acme.BookStore.Web.Controllers //TODO: you may need to fix this namespace!
{
    [Route("api")]
    [IgnoreAntiforgeryToken]
    public class MigrationController : AbpController
    {
        private const string DefaultAdminEmail = "admin@admin.com";
        private const string DefaultAdminPassword = "My-Admin-Password";

        private readonly IDataSeeder _dataSeeder;
        private readonly ITenantRepository _tenantRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IEnumerable<DbSchemaMigrator> _dbSchemaMigrators;

        public MigrationController(
            IDataSeeder dataSeeder,
            ITenantRepository tenantRepository,
            ICurrentTenant currentTenant,
            IEnumerable<DbSchemaMigrator> dbSchemaMigrators)
        {
            _dataSeeder = dataSeeder;
            _tenantRepository = tenantRepository;
            _currentTenant = currentTenant;
            _dbSchemaMigrators = dbSchemaMigrators;
        }

        [HttpGet]
        [Route("migrate")]
        public async Task<IActionResult> MigrateAsync(CancellationToken cancellationToken)
        {
            try
            {
                Logger.LogInformation("Started database migrations...");

                await MigrateDatabaseSchemaAsync();
                await SeedDataAsync();

                Logger.LogInformation("Successfully completed host database migrations. Started migrating tenant databases...");

                if (_currentTenant.IsAvailable) // or check MultiTenancyConsts.IsEnabled
                {
                    await MigrateTenantDatabases(cancellationToken);
                }

                Logger.LogInformation("Successfully completed all database migrations.");
                return Ok("Migration and seed completed successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred during migration: {ex.Message}");
            }
        }

        private async Task MigrateTenantDatabases(CancellationToken cancellationToken)
        {
            var tenants = await _tenantRepository.GetListAsync(includeDetails: true, cancellationToken: cancellationToken);

            var migratedDatabaseSchemas = new HashSet<string>();
            foreach (var tenant in tenants)
            {
                using (_currentTenant.Change(tenant.Id))
                {
                    if (tenant.ConnectionStrings.Any())
                    {
                        var tenantConnectionStrings = tenant.ConnectionStrings
                            .Select(x => x.Value)
                            .ToList();

                        if (!migratedDatabaseSchemas.IsSupersetOf(tenantConnectionStrings))
                        {
                            await MigrateDatabaseSchemaAsync(tenant);
                            migratedDatabaseSchemas.AddIfNotContains(tenantConnectionStrings);
                        }
                    }

                    await SeedDataAsync(tenant);
                }

                Logger.LogInformation($"Successfully completed {tenant.Name} tenant database migrations.");
            }
        }

        private async Task MigrateDatabaseSchemaAsync(Tenant tenant = null)
        {
            Logger.LogInformation($"Migrating schema for {(tenant == null ? "host" : tenant.Name + " tenant")} database...");
            foreach (var migrator in _dbSchemaMigrators)
            {
                await migrator.MigrateAsync();
            }
        }

        private async Task SeedDataAsync(Tenant tenant = null)
        {
            Logger.LogInformation($"Executing {(tenant == null ? "host" : tenant.Name + " tenant")} database seed...");

            await _dataSeeder.SeedAsync(
                new DataSeedContext(tenant?.Id)
                    .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName, DefaultAdminEmail)
                    .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName, DefaultAdminPassword)
            );
        }
    }
}
```



------



## Step 4: Navigate to the Migration URL

Build and run your web application. Then visit:

```
https://your-website.com/api/migrate
```

If your database did not exist, it will be created, and the default data will be seeded. If you enabled multi-tenancy, the tenant databases will also be migrated in this solution.

------



**That’s all there is to it!** By adding a simple controller and selectively applying the localization middleware, you can migrate your ABP app’s database with a single URL.



Happy coding 😊