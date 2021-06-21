# Getting Started

````json
//[doc-params]
{
    "UI": ["MVC", "Blazor", "BlazorServer", "NG"],
    "DB": ["EF", "Mongo"],
    "Tiered": ["Yes", "No"]
}
````

> This document assumes that you prefer to use **{{ UI_Value }}** as the UI framework and **{{ DB_Value }}** as the database provider. For other options, please change the preference on top of this document.

## Setup Your Development Environment

First things first! Let's setup your development environment before creating the project.

### Pre-Requirements

The following tools should be installed on your development machine:

* [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) (v16.8+) for Windows / [Visual Studio for Mac](https://visualstudio.microsoft.com/vs/mac/). <sup id="a-editor">[1](#f-editor)</sup>
* [.NET Core 5.0+](https://www.microsoft.com/net/download/dotnet-core/)
{{ if UI != "Blazor" }}
* [Node v12 or v14](https://nodejs.org/)
* [Yarn v1.20+ (not v2)](https://classic.yarnpkg.com/en/docs/install) <sup id="a-yarn">[2](#f-yarn)</sup> or npm v6+ (already installed with Node)
{{ end }}
{{ if Tiered == "Yes" }}
* [Redis](https://redis.io/) (the startup solution uses the Redis as the [distributed cache](Caching.md)).
{{ end }}

<sup id="f-editor"><b>1</b></sup> _You can use another editor instead of Visual Studio as long as it supports .NET Core and ASP.NET Core._ <sup>[↩](#a-editor)</sup>

{{ if UI != "Blazor" }}

<sup id="f-yarn"><b>2</b></sup> _Yarn v2 works differently and is not supported._ <sup>[↩](#a-yarn)</sup>

{{ end }}

### Install the ABP CLI

[ABP CLI](./CLI.md) is a command line interface that is used to automate some common tasks for ABP based solutions. First, you need to install the ABP CLI using the following command:

````shell
dotnet tool install -g Volo.Abp.Cli
````

If you've already installed, you can update it using the following command:

````shell
dotnet tool update -g Volo.Abp.Cli
````

## Next Step

* [Creating a new solution](Getting-Started-Create-Solution.md)