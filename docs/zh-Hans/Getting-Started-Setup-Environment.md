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

## 设置你的开发环境

第一件事! 在创建项目之前, 让我们先设置你的开发环境.

### 先决条件

开发计算机上应安装以下工具:

* 一个集成开发环境 (比如: [Visual Studio](https://visualstudio.microsoft.com/vs/)) 它需要支持 [.NET 6.0+](https://dotnet.microsoft.com/download/dotnet) 的开发.
{{ if UI != "Blazor" }}
* [Node v12 或 v14](https://nodejs.org/)
* [Yarn v1.20+ (不是v2)](https://classic.yarnpkg.com/en/docs/install) <sup id="a-yarn">[1](#f-yarn)</sup> 或 npm v6+ (已跟随Node一起安装)
{{ end }}
{{ if Tiered == "Yes" }}
* [Redis](https://redis.io/) (启动解决方案使用 Redis 作为 [分布式缓存](Caching.md)).
{{ end }}

{{ if UI != "Blazor" }}

<sup id="f-yarn"><b>1</b></sup> _Yarn v2 工作方式不同, 不被支持._ <sup>[↩](#a-yarn)</sup>

{{ end }}

### 安装 ABP CLI

[ABP CLI](./CLI.md) 是一个命令行界面, 用于自动执行基于 ABP 的解决方案的一些常见任务. 首先, 你需要使用以下命令安装 ABP CLI：

````shell
dotnet tool install -g Volo.Abp.Cli
````

如果已安装, 则可以使用以下命令对其进行更新:

````shell
dotnet tool update -g Volo.Abp.Cli
````

## 下一步

* [创建新的解决方案](Getting-Started-Create-Solution.md)