# ABP

[![Build Status](http://vjenkins.dynu.net:5480/job/abp/badge/icon)](http://ci.volosoft.com:5480/blue/organizations/jenkins/abp/activity)
[![NuGet](https://img.shields.io/nuget/v/Volo.Abp.Core.svg?style=flat-square)](https://www.nuget.org/packages/Volo.Abp.Core)
[![NuGet Download](https://img.shields.io/nuget/dt/Volo.Abp.Core.svg?style=flat-square)](https://www.nuget.org/packages/Volo.Abp.Core)
[![MyGet (with prereleases)](https://img.shields.io/myget/abp-nightly/vpre/Volo.Abp.Core.svg?style=flat-square)](https://docs.abp.io/en/abp/latest/Nightly-Builds)

This project is the next generation of the [ASP.NET Boilerplate](https://aspnetboilerplate.com/) web application framework. See [the announcement](https://abp.io/blog/abp/Abp-vNext-Announcement).

See the official [web site (abp.io)](https://abp.io/) for more information.

### Status

This project is in **preview** stage and it's not suggested to use it in production yet.

### Documentation

See the <a href="https://abp.io/documents/" target="_blank">documentation</a>.

### Development

#### Pre Requirements

- Visual Studio 2019 16.1.0+

#### Framework

Framework solution is located under the `framework` folder. It has no external dependency.

#### Modules/Templates

[Modules](modules/) and [Templates](templates/) have their own solutions and have **local references** to the framework and each other.

### Contribution

ABP is an open source platform. Check [the contribution guide](docs/en/Contribution/Index.md) if you want to contribute to the project.
