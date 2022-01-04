# 入门教程

````json
//[doc-params]
{
    "UI": ["MVC", "Blazor", "BlazorServer", "NG"],
    "DB": ["EF", "Mongo"],
    "Tiered": ["Yes", "No"]
}
````

> 本文档假设你更喜欢使用 **{{ UI_Value }}** 作为 UI 框架, 使用 **{{ DB_Value }}** 作为数据库提供程序. 对于其他选项, 请更改本文档顶部的首选项.

## 创建新项目

我们将使用 ABP CLI 创建一个新的 ABP 项目.

> 或者, 你可以使用[ABP Framework 网站](https://abp.io/get-started)页面上的选项轻松的 **创建并下载** 项目.

使用 ABP CLI 的 `new` 命令创建一个新项目:

````shell
abp new Acme.BookStore{{if UI == "NG"}} -u angular{{else if UI == "Blazor"}} -u blazor{{else if UI == "BlazorServer"}} -u blazor-server{{end}}{{if DB == "Mongo"}} -d mongodb{{end}}{{if Tiered == "Yes"}}{{if UI == "MVC" || UI == "BlazorServer"}} --tiered{{else}} --separate-identity-server{{end}}{{end}}
````

*你可以使用不同级别的命名空间, 例如: BookStore、Acme.BookStore或 Acme.Retail.BookStore.*

{{ if Tiered == "Yes" }}

{{ if UI == "MVC" || UI == "BlazorServer" }}

* `--tified` 参数用于创建认证服务器、 UI 和 API 实际分隔的 N-层解决方案.

{{ else }}

* `--separate-identity-server` 参数用于将Identity Server应用程序与API主机应用程序分隔开. 如果未指定, 则服务器上将只有一个端点.

{{ end }}

{{ end }}

> [ABP CLI 文档](./CLI.md) 涵盖了所有可用的命令和选项.

## 移动端开发

如果你想要在你的解决方案中包含 [React Native](https://reactnative.dev/) 项目, 将 `-m react-native` (or `--mobile react-native`) 参数添加到项目创建命令. 这是一个基础的 React Native 启动模板, 用于开发基于你的 ABP 后端的移动应用程序.

请参阅 [React Native 入门](Getting-Started-React-Native.md) 文档, 了解如何配置和运行 React Native 应用程序.

### 解决方案结构

该解决方案具有分层结构 (基于 [域驱动设计](Domain-Driven-Design.md)), 并包含单元 & 集成测试项目. 请参阅 [应用程序模板文档](Startup-Templates/Application.md) 以详细了解解决方案结构.

{{ if DB == "Mongo" }}

#### MongoDB 事务

[启动模板](Startup-templates/Index.md) 默认在`.MongoDB`项目中**禁用**事务. 如果你的MongoDB服务器支持事务, 你可以在*YourProjectMongoDbModule*类中的`ConfigureServices`方法开启它:

  ```csharp
Configure<AbpUnitOfWorkDefaultOptions>(options =>
{
    options.TransactionBehavior = UnitOfWorkTransactionBehavior.Auto;
});
  ```

> 或者你可以删除该代码, 因为 `Auto` 已经是默认行为.

{{ end }}

## 下一步

* [运行解决方案](Getting-Started-Running-Solution.md)