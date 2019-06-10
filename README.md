# ABP

[![Build Status](http://vjenkins.dynu.net:5480/job/abp/badge/icon)](http://ci.volosoft.com:5480/blue/organizations/jenkins/abp/activity)

This project is the next generation of the [ASP.NET Boilerplate](https://aspnetboilerplate.com/) web application framework. See [the announcement](https://abp.io/blog/abp/Abp-vNext-Announcement).

See the official [web site (abp.io)](https://abp.io/) for more information.

### Status

This project is in **very early preview** stage and it's not suggested to use it in a real project. 

### Documentation

See the <a href="https://abp.io/documents/" target="_blank">documentation</a>.

### How to Build

- Run the `build-all.ps1`. It will build all the solutions in this repository.

### Development

#### Pre Requirements

- Visual Studio 2019 16.1.0+

#### Framework

Framework solution is located under the `framework` folder. It has no external dependency.

#### Modules/Templates

[Modules](modules/) and [Templates](templates/) have their own solutions and have **local references** to the framework and each other.

### Contribution

ABP is an open source platform. Check [the contribution guide](docs/en/Contribution/Index.md) if you want to contribute to the project.
