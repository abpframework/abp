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

* An IDE (e.g. [Visual Studio](https://visualstudio.microsoft.com/vs/)) that supports [.NET 6.0+](https://dotnet.microsoft.com/download/dotnet) development.
{{ if UI != "Blazor" }}
* [Node v12 or v14](https://nodejs.org/)
* [Yarn v1.20+ (not v2)](https://classic.yarnpkg.com/en/docs/install) <sup id="a-yarn">[1](#f-yarn)</sup> or npm v6+ (already installed with Node)
{{ end }}
{{ if Tiered == "Yes" }}
* [Redis](https://redis.io/) (the startup solution uses the Redis as the [distributed cache](Caching.md)).
{{ end }}

{{ if UI != "Blazor" }}

<sup id="f-yarn"><b>1</b></sup> _Yarn v2 works differently and is not supported._ <sup>[↩](#a-yarn)</sup>

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