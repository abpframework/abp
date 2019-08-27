## Entity Framework Core PostgreSQL Integration

> See [Entity Framework Core Integration document](../Entity-Framework-Core.md) for the basics of the EF Core integration.

### EntityFrameworkCore Project Update

- In `Acme.BookStore.EntityFrameworkCore` project replace package `Volo.Abp.EntityFrameworkCore.SqlServer` with `Volo.Abp.EntityFrameworkCore.PostgreSql` 
- Update to use PostgreSQL in `BookStoreEntityFrameworkCoreModule`. Example:

````C#
[DependsOn(
        //code omitted for brevity
		
		/* This was updated from AbpEntityFrameworkCoreSqlServerModule */
        typeof(AbpEntityFrameworkCorePostgreSqlModule),
		/* This was updated from AbpEntityFrameworkCoreSqlServerModule */
        
		//code omitted for brevity
        )]
public class Acme.BookStore.EntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BookStoreDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                /* This was updated */
                options.UsePostgreSql();
				/* This was updated */
            });
        }
    }
````

### EntityFrameworkCore.DbMigrations Project Update
- **Do** update to use PostgreSQL in `BookStoreMigrationsDbContextModelSnapshot.cs`
	
import `Npgsql.EntityFrameworkCore.PostgreSQL.Metadata` by adding
	
````C#
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
````	
	
replace all references
````
.HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
````

 to

````C#
.HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
````

### Update Connection String Settings
> Update the PostgreSQL connection string in all `appsettings.json` files

