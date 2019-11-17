# Dapper Integration

Because Dapper's idea is that the sql statement takes precedence, and mainly provides some extension methods for the `IDbConnection` interface.

Abp does not encapsulate too many functions for Dapper. Abp Dapper provides a `DapperRepository<TDbContext>` base class based on Abp EntityFrameworkCore, which provides the `IDbConnection` and `IDbTransaction` properties required by Dapper.

These two properties can work well with [Unit-Of-Work](Unit-Of-Work.md).

## Installation

Please install and configure EF Core according to [EF Core's integrated documentation](Entity-Framework-Core.md).

`Volo.Abp.Dapper` is the main nuget package for the Dapper integration. Install it to your project (for a layered application, to your data/infrastructure layer):

```shell
Install-Package Volo.Abp.Dapper
```

Then add `AbpDapperModule` module dependency (`DependsOn` attribute) to your [module](Module-Development-Basics.md):

````C#
using Volo.Abp.Dapper;
using Volo.Abp.Modularity;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpDapperModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
````

## Implement Dapper Repository

The following code implements the `Person` repository, which requires EF Core's `DbContext` (MyAppDbContext). You can inject `PersonDapperRepository` to call its methods.

`DbConnection` and `DbTransaction` are from the `DapperRepository` base class.

```C#
public class PersonDapperRepository : DapperRepository<MyAppDbContext>, ITransientDependency
{
    public PersonDapperRepository(IDbContextProvider<MyAppDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public virtual async Task<List<string>> GetAllPersonNames()
    {
        return (await DbConnection.QueryAsync<string>("select Name from People", transaction: DbTransaction))
            .ToList();
    }

    public virtual async Task<int> UpdatePersonNames(string name)
    {
        return await DbConnection.ExecuteAsync("update People set Name = @NewName", new { NewName = name },
            DbTransaction);
    }
}
```
