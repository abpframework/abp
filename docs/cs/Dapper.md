# Dapper integrace

Jelikož myšlenka Dapper je taková, že sql příkaz má přednost, tak hlavně poskytuje metody rozšíření pro `IDbConnection` rozhraní.

Abp nezapouzdřuje přílíš mnoho funkcí pro Dapper. Abp Dapper poskytuje základní třídu `DapperRepository<TDbContext>` založenou na Abp EntityFrameworkCore, který poskytuje vlastnosti `IDbConnection` a `IDbTransaction` vyžadované v Dapper.

Tyto dvě vlastnosti fungují dobře s [jednotkou práce](Unit-Of-Work.md).

## Instalace

Nainstalujte a nakonfigurujte EF Core podle [EF Core integrační dokumentace](Entity-Framework-Core.md).

`Volo.Abp.Dapper` je hlavní NuGet balík pro Dapper integraci. Nainstalujte jej proto do vašeho projektu (pro strukturovanou aplikaci do datové/infrastrukturní vrstvy):

```shell
Install-Package Volo.Abp.Dapper
```

Poté přidejte závislost na `AbpDapperModule` modulu (atribut `DependsOn`) do Vašeho [modulu](Module-Development-Basics.md):

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

## Implementace Dapper repozitáře

Následující kód implementuje repozitář `Person`, který vyžaduje `DbContext` z EF Core (MyAppDbContext). Můžete vložit `PersonDapperRepository` k volání jeho metod.

`DbConnection` a `DbTransaction` jsou ze základní třídy `DapperRepository`.

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
