# Unifying DbContexts for EF Core / Removing the EF Core Migrations Project

This article shows how to remove the `EntityFrameworkCore.DbMigrations` project from your solution to have a single `DbContext` for your database mappings and code first migrations.

## Motivation

If you create a new solution with **Entity Framework Core** as the database provider, you see two projects related to EF Core:

![default-solution](default-solution.png)

* `EntityFrameworkCore` project contains the actual `DbContext` of your application. It includes all the database mappings and your repository implementations.
* `EntityFrameworkCore.DbMigrations` project, on the other hand, contains another `DbContext` class that is only used to create and apply the database migrations. It contains the database mappings for all the modules you are using, so have a single, unified database schema.

There were two main reasons we'd created such a separation;

1. Your actual `DbContext` remains simple and focused. It only contains your own entities and doesn't contain anything related to the modules that are used by the application.
2. You can create your own classes that map to the tables of depending modules. For example, the `AppUser` entity (that is included in the downloaded solution) is mapped to `AbpUsers` table in the database, which is actually mapped to the `IdentityUser` entity of the [Identity Module](https://docs.abp.io/en/abp/latest/Modules/Identity). That means they share the same database table. `AppUser` includes less properties compared to `IdentityServer`. You only add the properties you need, not more. This also allows you to add new standard (type-safe) properties to the `AppUser` for your custom requirements as long as you carefully manage the database mappings.

We've [documented the structure](https://docs.abp.io/en/abp/latest/Entity-Framework-Core-Migrations) in details. However, it has always been a problem for the developers since that structure makes your database mappings complicated when you re-use tables of the depended modules. Many developers are misunderstanding or making mistakes while mapping such classes, especially when they try to use these entities in relations to other entities.

**So, [we've decided](https://github.com/abpframework/abp/issues/8776) to cancel that separation and remove the `EntityFrameworkCore.DbMigrations` project in the version 4.4. New startup solutions will come with a single `EntityFrameworkCore` project and single `DbContext` class.**

If you want to make it in your solution with today, follow the steps in this article.

> There is one **drawback** with the new design (everything in software development is a trade-off). We need to remove the `AppUser` entity, because EF Core can't map two classes to single table without an inheritance relation. I will cover this later in this article and provide suggestions to deal with it.

> If you are using **ABP Commercial**, ABP Suite code generation won't work correctly with the new design. In this case, we suggest to wait for the next version.

## All the Changes in one PR!

I've created a new solution with v4.3, then made all the changes in a pull request, so you can see all the changes line by line.  While this article will cover all, you may want to check the changes done in this PR **(TODO: LINK)** if you have problems with the implementation.

## The Steps

Our goal to enable database migrations in the `EntityFrameworkCore` project, remove the `EntityFrameworkCore.DbMigrations` project and revisit the code depending on that package.

### 1) Add Microsoft.EntityFrameworkCore.Tools package to the EntityFrameworkCore project

Add the following code into the `EntityFrameworkCore.csproj` file:

````xml
<ItemGroup>
  <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="5.0.*">
    <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
    <PrivateAssets>compile; contentFiles; build; buildMultitargeting; buildTransitive; analyzers; native</PrivateAssets>
  </PackageReference>
</ItemGroup>
````

### 2) Create design time DbContext factory

Create a class implementing `IDesignTimeDbContextFactory<T>` inside the `EntityFrameworkCore` project:

````csharp
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UnifiedContextsDemo.EntityFrameworkCore
{
    public class UnifiedContextsDemoDbContextFactory : IDesignTimeDbContextFactory<UnifiedContextsDemoDbContext>
    {
        public UnifiedContextsDemoDbContext CreateDbContext(string[] args)
        {
            UnifiedContextsDemoEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<UnifiedContextsDemoDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new UnifiedContextsDemoDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../UnifiedContextsDemo.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
````

I basically copied from the `EntityFrameworkCore.DbMigrations` project, renamed and uses the actual `DbContext` of the application.

### 3) Create DB schema migrator

Copy `EntityFrameworkCore...DbSchemaMigrator` (`...` standard for your project name) class to the `EntityFrameworkCore` project and change the code in the `MigrateAsync` method to use the actual `DbContext` of the application. In my case, the final class is shown below:

````csharp
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UnifiedContextsDemo.Data;
using Volo.Abp.DependencyInjection;

namespace UnifiedContextsDemo.EntityFrameworkCore
{
    public class EntityFrameworkCoreUnifiedContextsDemoDbSchemaMigrator
        : IUnifiedContextsDemoDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreUnifiedContextsDemoDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the UnifiedContextsDemoMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<UnifiedContextsDemoDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}
````

### 4) Move module configurations

The migrations `DbContext` typically contains code lines like `builder.ConfigureXXX()` for each module you are using. We can move these lines to our actual `DbContext` in the `EntityFrameworkCore` project. Also, remove the database mappings for the `AppUser` (we will remove this entity). Optionally, you may move the database mappings code for your own entities from `...DbContextModelCreatingExtensions` class in the `OnModelCreating` method of the actual `DbContext`, and remove the static extension class.

For the example solution, the final `DbContext` class is shown below:

````csharp
using Microsoft.EntityFrameworkCore;
using UnifiedContextsDemo.Users;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace UnifiedContextsDemo.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class UnifiedContextsDemoDbContext
        : AbpDbContext<UnifiedContextsDemoDbContext>
    {
        public DbSet<AppUser> Users { get; set; }

        /* Add DbSet properties for your Aggregate Roots / Entities here.
         * Also map them inside UnifiedContextsDemoDbContextModelCreatingExtensions.ConfigureUnifiedContextsDemo
         */

        public UnifiedContextsDemoDbContext(
            DbContextOptions<UnifiedContextsDemoDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePermissionManagement();
            builder.ConfigureSettingManagement();
            builder.ConfigureBackgroundJobs();
            builder.ConfigureAuditLogging();
            builder.ConfigureIdentity();
            builder.ConfigureIdentityServer();
            builder.ConfigureFeatureManagement();
            builder.ConfigureTenantManagement();

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(UnifiedContextsDemoConsts.DbTablePrefix + "YourEntities", UnifiedContextsDemoConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}
````

### 5) Remove `EntityFrameworkCore.DbMigrations` project from the solution

Remove the `EntityFrameworkCore.DbMigrations` project from the solution and replace references given to that project by the `EntityFrameworkCore` project reference.

Also, change usages of `...EntityFrameworkCoreDbMigrationsModule` to `...EntityFrameworkCoreModule` (`...` stands for your project name).

In this example, I had to change references and usages in the `DbMigrator`, `Web` and `EntityFrameworkCore.Tests` projects.

### 6) Remove AppUser Entity

We need to remove the `AppUser` entity, because EF Core can't map two classes to single table without an inheritance relation. So, remove this class and all the usages. You can replace the usages with `IdentityUser` if you need to query users in your application code.

TODO