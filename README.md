# ABP

[![Build Status](http://vjenkins.dynu.net:5480/job/abp/badge/icon)](http://ci.volosoft.com:5480/blue/organizations/jenkins/abp/activity)
[![NuGet](https://img.shields.io/nuget/v/Volo.Abp.Core.svg?style=flat-square)](https://www.nuget.org/packages/Volo.Abp.Core)
[![NuGet Download](https://img.shields.io/nuget/dt/Volo.Abp.Core.svg?style=flat-square)](https://www.nuget.org/packages/Volo.Abp.Core)
[![MyGet (with prereleases)](https://img.shields.io/myget/abp-nightly/vpre/Volo.Abp.svg?style=flat-square)](https://docs.abp.io/en/abp/latest/Nightly-Builds)

This project is the next generation of the [ASP.NET Boilerplate](https://aspnetboilerplate.com/) web application framework. See [the announcement](https://blog.abp.io/abp/Abp-vNext-Announcement).

See the official [web site (abp.io)](https://abp.io/) for more information.

### Documentation

See the <a href="https://docs.abp.io/" target="_blank">documentation</a>.

### Development

#### Pre Requirements

- Visual Studio 2019 16.3.0+

#### Framework

Framework solution is located under the `framework` folder. It has no external dependency.

#### Modules/Templates

[Modules](modules/) and [Templates](templates/) have their own solutions and have **local references** to the framework and each other.

Visual Studio can not work properly with the local references out of the solution folder. When you open a module/sample solution in the Visual Studio, you may get some errors related to the dependencies. In this case, run the `dotnet restore` on the command prompt for the related solution's folder. You need to run it after you first open the solution or change a dependency.

### Contribution

ABP is an open source platform. Check [the contribution guide](docs/en/Contribution/Index.md) if you want to contribute to the project.
