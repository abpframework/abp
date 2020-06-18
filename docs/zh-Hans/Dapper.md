# Dapper 集成

Dapper 是轻量化的数据库提供者,使用Dapper主要的好处是编写T-SQL查询,它为 `IDbConnection` 接口提供了一些扩展方法.

ABP没有为Dapper封装许多功能, ABP Dapper库在ABP EntityFrameworkCore的基础上提供了 `DapperRepository<TDbContext>` 基类,在其中提供了Dapper需要的`IDbConnection`和`IDbTransaction`属性,这两个属性可以和[工作单元](Unit-Of-Work.md)很好的配合.

## 安装

请先根据[EF Core的集成文档](Entity-Framework-Core.md)安装并配置好EF Core.

`Volo.Abp.Dapper`是Dapper集成的主要[nuget包](https://www.nuget.org/packages/Volo.Abp.Dapper).

将其安装到你的项目中(在分层应用程序中适用于 数据访问/基础设施层):

```shell
Install-Package Volo.Abp.Dapper
```

然后添加 `AbpDapperModule` 模块依赖项(`DependsOn` Attribute) 到 [module](Module-Development-Basics.cn.md)(项目中的Mudole类):

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

## 实现Dapper仓储

下面的代码实现了`Person`仓储, 它需要EF Core的`DbContext`(MyAppDbContext). 你可以注入`PersonDapperRepository`来调用它的方法.

`DbConnection`和`DbTransaction`来自于`DapperRepository`基类.

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
