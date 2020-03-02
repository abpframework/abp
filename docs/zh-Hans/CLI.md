# ABP CLI

ABP CLI (命令行接口) 是一个命令行工具,用来执行基于ABP解决方案的一些常见操作.

## Installation

ABP CLI 是一个 [dotnet global tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools). 使用命令行窗口安装:

````bash
dotnet tool install -g Volo.Abp.Cli
````

更新最新版本:

````bash
dotnet tool update -g Volo.Abp.Cli
````

## Commands

### new

生成基于ABP[启动模板](Startup-Templates/Index.md)的新解决方案.

基本用法:

````bash
abp new <解决方案名称> [options]
````

示例:

````bash
abp new Acme.BookStore
````

* Acme.BookStore是解决方案的名称.
* 常见的命名方式类似于 *YourCompany.YourProject*. 不过你可以使用自己喜欢的方式,如 *YourProject* (单级命名空间) 或 *YourCompany.YourProduct.YourModule* (三级命名空间).

#### Options

* `--template` 或者 `-t`: 指定模板. 默认的模板是 `app`,会生成web项目.可用的模板有:
  * `app` (default): [应用程序模板](Startup-Templates/Application.md). 其他选项:
    * `--ui` 或者 `-u`: 指定ui框架.默认`mvc`框架.其他选项：
      * `mvc`: ASP.NET Core MVC.此模板的其他选项：
        * `--tiered`: 创建分层解决方案，Web和Http Api层在物理上是分开的.如果未指定会创建一个分层的解决方案，此解决方案没有那么复杂,适合大多数场景.
      * `angular`: Angular. 这个模板还有一些额外的选项：
        * `--separate-identity-server`: 将Identity Server应用程序与API host应用程序分开. 如果未指定,则服务器端将只有一个端点.
      * `none`: 无UI. 这个模板还有一些额外的选项：
        * `--separate-identity-server`: 将Identity Server应用程序与API host应用程序分开. 如果未指定,则服务器端将只有一个端点.
    * `--database-provider` 或者 `-d`: 指定数据库提供程序.默认是 `ef`.其他选项:
      * `ef`: Entity Framework Core.
      * `mongodb`: MongoDB.
  * `module`: [Module template](Startup-Templates/Module.md). 其他选项:
    * `--no-ui`: 不包含UI.仅创建服务模块(也称为微服务 - 没有UI).
* `--output-folder` 或者 `-o`: 指定输出文件夹，默认是当前目录.
* `--version` 或者 `-v`: 指定ABP和模板的版本.它可以是 [release tag](https://github.com/abpframework/abp/releases) 或者 [branch name](https://github.com/abpframework/abp/branches). 如果没有指定，则使用最新版本.大多数情况下，您会希望使用最新的版本.

### add-package

通过以下方式将ABP包添加到项目中

* 添加nuget包做为项目的依赖项目.
* 添加 `[DependsOn(...)]` attribute到项目的模块类 (请参阅 [模块开发文档](Module-Development-Basics.md)).

> 需要注意的是添加的模块可能需要额外的配置,通常会在包的文档中指出.

基本用法:

````bash
abp add-package <包名> [options]
````

示例:

````
abp add-package Volo.Abp.MongoDB
````

* 示例中将Volo.Abp.MongoDB包添加到项目中.

#### Options

* `--project` 或 `-p`: 指定项目 (.csproj) 路径. 如果未指定,Cli会尝试在当前目录查找.csproj文件.

### add-module

通过查找模块的所有包,查找解决方案中的相关项目,并将每个包添加到解决方案中的相应项目,从而将[多包应用程序模块](Modules/Index)添加到解决方案中.

> 由于分层,不同的数据库提供程序选项或其他原因,业务模块通常由多个包组成. 使用`add-module`命令可以大大简化向模块添加模块的过程. 但是每个模块可能需要一些其他配置,这些配置通常在相关模块的文档中指出.

基本用法:

````bash
abp add-module <模块名称> [options]
````

示例:

```bash
abp add-module Volo.Blogging
```

* 示例中将Volo.Blogging模块添加到解决方案中.

#### Options

* `--solution` 或 `-s`: 指定解决方案 (.sln) 路径. 如果未指定,CLI会尝试在当前目录中寻找.sln文件.
* `--skip-db-migrations`: 对于EF Core 数据库提供程序,它会自动添加新代码的第一次迁移 (`Add-Migration`) 并且在需要时更新数据库 (`Update-Database`). 指定此选项可跳过此操作.
* `-sp` 或 `--startup-project`: 启动项目的项目文件夹的相对路径. 默认值是当前文件夹.

### update

更新所有ABP相关的包可能会很繁琐,框架和模块都有很多包. 此命令自动将解决方案或项目中所有ABP相关的包更新到最新版本.

用法:

````bash
abp update [options]
````

* 如果你的文件夹中有.sln文件,运行命令会将解决方案中所有项目ABP相关的包更新到最新版本.
* 如果你的文件夹中有.csproj文件,运行命令会将项目中所有ABP相关的包更新到最新版本.

#### Options

* `--include-previews` 或 `-p`: 将预览版, 测试版本 和 rc 包 同时更新到最新版本.
* `--npm`: 仅更新NPM包
* `--nuget`: 仅更新的NuGet包

### login

CLI的一些功能需要登录到abp.io平台. 使用你的用户名登录

```bash
abp login <username>
```

请注意,新的登录将终止先前的会话并创建一个新的会话.

### logout

通过从计算机中删除会话令牌来注销.

```
abp logout
```

### help

CLI的基本用法信息.

用法:

````bash
abp help [命令名]
````

示例:

````bash
abp help        # 显示常规帮助.
abp help new    # 显示有关 "New" 命令的帮助.
````