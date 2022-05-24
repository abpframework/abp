# ABP CLI - 新解决方案命令示例

`abp new`命令基于abp模板创建abp解决方案或其他组件. [ABP CLI](CLI.md)有一些参数可以用于创建新的ABP解决方案. 在本文档中, 我们将向你展示一些创建新的解决方案的命令示例. 所有的项目名称都是`Acme.BookStore`. 目前, 唯一可用的移动端项目是`React Native`移动端应用程序. 可用的数据库提供程序有`Entity Framework Core`和`MongoDB`. 所有命令都以`abp new`开头.

## Angular

以下命令用于创建Angular UI项目:

* 在新文件夹中创建项目, **Entity Framework Core**, 非移动端应用程序:  

  ````bash
  abp new Acme.BookStore -u angular --mobile none --database-provider ef -csf
  ````
  
* 在新文件夹中创建项目, **Entity Framework Core**, 默认应用程序模板, **拆分Identity Server**:

  ```bash
  abp new Acme.BookStore -t app -u angular -m none --separate-identity-server --database-provider ef -csf
  ```

* 在新文件夹中创建项目, **Entity Framework Core**, **自定义连接字符串**:

  ```bash
  abp new Acme.BookStore -u angular -csf --connection-string Server=localhost;Database=MyDatabase;Trusted_Connection=True
  ```

* 在`C:\MyProjects\Acme.BookStore`中创建解决方案, **MongoDB**, 默认应用程序模板, 包含移动端项目:

  ```bash
  abp new Acme.BookStore -u angular --database-provider mongodb --output-folder C:\MyProjects\Acme.BookStore
  ```

* 在新文件夹中创建项目, **MongoDB**, 默认应用程序模板, 不创建移动端应用程序, **拆分Identity Server**:

  ```bash
  abp new Acme.BookStore -t app -u angular -m none --separate-identity-server --database-provider mongodb -csf
  ```

## MVC

以下命令用于创建MVC UI项目:

* 在新文件夹中创建项目, **Entity Framework Core**, 不创建移动端应用程序:

  ```bash
  abp new Acme.BookStore -t app -u mvc --mobile none --database-provider ef -csf
  ```

* 在新文件夹中创建项目, **Entity Framework Core**, **分层结构** (*Web和HTTP API层是分开的*), 不创建移动端应用程序:

  ```bash
  abp new Acme.BookStore -u mvc --mobile none --tiered --database-provider ef -csf
  ```

* 在新文件夹中创建项目, **MongoDB**, 不创建移动端应用程序:

  ```bash
  abp new Acme.BookStore -t app -u mvc --mobile none --database-provider mongodb -csf
  ```
  
* 在新文件夹中创建项目, **MongoDB**, **分层结构**:

  ```bash
  abp new Acme.BookStore -u mvc --tiered --database-provider mongodb -csf
  ```


## Blazor

以下命令用于创建Blazor项目:

* **Entity Framework Core**, 不创建移动端应用程序:

  ```bash
  abp new Acme.BookStore -t app -u blazor --mobile none
  ```

* **Entity Framework Core**, **拆分Identity Server**, 包含移动端应用程序:
  
  ```bash
  abp new Acme.BookStore -u blazor --separate-identity-server
  ```

* 在新文件夹中创建项目, **MongoDB**, 不创建移动端应用程序:

  ```bash
  abp new Acme.BookStore -u blazor --database-provider mongodb --mobile none -csf
  ```

## Blazor Server

以下命令用于创建Blazor项目:

* **Entity Framework Core**, 不创建移动端应用程序:

  ```bash
  abp new Acme.BookStore -t app -u blazor-server --mobile none
  ```

* **Entity Framework Core**, **拆分Identity Server**, **拆分API Host**, 包含移动端应用程序:
  
  ```bash
  abp new Acme.BookStore -u blazor-server --tiered
  ```

* 在新文件夹中创建项目, **MongoDB**, 不创建移动端应用程序:

  ```bash
  abp new Acme.BookStore -u blazor --database-provider mongodb --mobile none -csf
  ```
  
## 无UI 

在默认应用程序模板中, 始终有一个前端项目. 在这个选项中没有前端项目. 它有一个`HttpApi.Host`项目为你的HTTP WebAPI提供服务. 这个选项适合在你想创建一个WebAPI服务时使用.

* 在新文件夹中创建项目, **Entity Framework Core**, 拆分Identity Server:

    ```bash
    abp new Acme.BookStore -u none --separate-identity-server -csf
    ```
* **MongoDB**, 不创建移动端应用程序:

    ```bash
    abp new Acme.BookStore -u none --mobile none --database-provider mongodb
    ```
    


## 控制台应用程序

这是一个基于.NET控制台应用程序的模板, 集成了ABP模块架构. 要创建控制台应用程序, 请使用以下命令:

* 项目由以下文件组成: `Acme.BookStore.csproj`, `appsettings.json`, `BookStoreHostedService.cs`, `BookStoreModule.cs`, `HelloWorldService.cs` 和 `Program.cs`.

  ```bash
  abp new Acme.BookStore -t console -csf
  ```

## 模块

模块是主项目使用的可重用子应用程序. 如果你正在构建微服务解决方案, 使用ABP模块是最佳方案. 由于模块不是最终的应用程序, 每个模块都有前端UI项目和数据库提供程序. 模块模板带有MVC UI, 可以在没有最终解决方案的情况下进行开发. 但是, 如果要在最终解决方案下开发模块, 可以添加`--no-ui`参数来去除MVC UI项目.

* 包含前端: `MVC`, `Angular`, `Blazor`. 包含数据库提供程序: `Entity Framework Core`, `MongoDB`. 包含MVC启动项目.

  ```bash
  abp new Acme.IssueManagement -t module
  ```
* 与上面相同, 但不包括MVC启动项目.

  ```bash
  abp new Acme.IssueManagement -t module --no-ui
  ```
  
* 创建模块并将其添加到解决方案中

  ```bash
  abp new Acme.IssueManagement -t module --add-to-solution-file
  ```

## 从特定版本创建解决方案

创建解决方案时, 它总是使用最新版本创建. 要从旧版本创建项目, 可以使用`--version`参数.

* 使用v3.3.0版本创建解决方案, 包含Angular UI和Entity Framework Core.

  ```bash
  abp new Acme.BookStore -t app -u angular -m none --database-provider ef -csf --version 3.3.0
  ```

要获取ABP版本列表, 请查看以下链接: https://www.nuget.org/packages/Volo.Abp.Core/

## 从自定义模板创建

ABP CLI使用默认的[应用程序模板](https://github.com/abpframework/abp/tree/dev/templates/app)创建项目. 如果要从自定义模板创建新的解决方案, 可以使用参数`--template-source`. 

* 在`c:\MyProjects\templates\app`目录中使用模板, MVC UI, Entity Framework Core, 不创建移动端应用程序. 

  ```bash
  abp new Acme.BookStore -t app -u mvc --mobile none --database-provider ef --template-source "c:\MyProjects\templates\app"
  ```

* 除了此命令从URL `https://myabp.com/app-template.zip` 检索模板之外, 与上一个命令相同.

  ```bash
  abp new Acme.BookStore -t app -u mvc --mobile none --database-provider ef --template-source https://myabp.com/app-template.zip
  ```

## 创建预览版本

ABP CLI始终使用最新版本. 要从预览(RC)版本创建解决方案, 请添加`--preview`参数.

* 在新文件夹中创建项目, Blazor UI, Entity Framework Core, 不创建移动端应用程序, **使用最新版本**:

  ```bash
  abp new Acme.BookStore -t app -u blazor --mobile none -csf --preview
  ```

## 选择数据库管理系统

默认的数据库管理系统是 `Entity Framework Core` / ` SQL Server`. 你可以通过使用`--database-management-system`参数选择DBMS. [可用的值](https://github.com/abpframework/abp/blob/dev/framework/src/Volo.Abp.Cli.Core/Volo/Abp/Cli/ProjectBuilding/Building/DatabaseManagementSystem.cs) 包括 `SqlServer`, `MySQL`, `SQLite`, `Oracle`, `Oracle-Devart`, `PostgreSQL`. 默认值是 `SqlServer`.

* 在新文件夹中创建项目, Angular UI, **PostgreSQL** 数据库:

  ```bash
  abp new Acme.BookStore -u angular --database-management-system PostgreSQL -csf
  ```

## 使用静态HTTP端口

ABP CLI始终为项目分配随机端口. 如果需要保留默认端口并且创建解决方案始终使用相同的HTTP端口, 请添加参数`--no-random-port`.

* 在新文件夹中创建项目, MVC UI,  Entity Framework Core, **静态端口**:

  ```bash
  abp new Acme.BookStore --no-random-port -csf
  ```

## 引用本地ABP框架

在ABP解决方案中, 默认情况下从NuGet引用ABP库. 有时, 你需要在本地将ABP库引用到你的解决方案中. 这利于调试框架本身. 本地ABP框架的根目录必须有`Volo.Abp.sln`文件. 你可以将以下目录的内容复制到你的文件系统中

* MVC UI,  Entity Framework Core, **引用本地的ABP库**:

本地路径必须是ABP存储库的根目录. 
如果`C:\source\abp\framework\Volo.Abp.sln`是你的框架解决方案的路径, 那么你必须设置`--abp-path`参数值为`C:\source\abp`.

  ```bash
  abp new Acme.BookStore --local-framework-ref --abp-path C:\source\abp
  ```

**输出**:

如下所示, 引用本地ABP框架库项目.

```xml
<ItemGroup>
	<ProjectReference Include="C:\source\abp\framework\src\Volo.Abp.Autofac\Volo.Abp.Autofac.csproj" />
	<ProjectReference Include="C:\source\abp\framework\src\Volo.Abp.AspNetCore.Serilog\Volo.Abp.AspNetCore.Serilog.csproj" />
	<ProjectReference Include="C:\source\abp\framework\src\Volo.Abp.AspNetCore.Authentication.JwtBearer\Volo.Abp.AspNetCore.Authentication.JwtBearer.csproj" />
	<ProjectReference Include="..\Acme.BookStore.Application\Acme.BookStore.Application.csproj" />
	<ProjectReference Include="..\Acme.BookStore.HttpApi\Acme.BookStore.HttpApi.csproj" />
	<ProjectReference Include="..\Acme.BookStore.EntityFrameworkCore\Acme.BookStore.EntityFrameworkCore.csproj" />
</ItemGroup>    
```

## 另请参阅

* [ABP CLI文档](CLI.md)
