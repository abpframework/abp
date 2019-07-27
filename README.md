# ABP

[![Build Status](http://vjenkins.dynu.net:5480/job/abp/badge/icon)](http://ci.volosoft.com:5480/blue/organizations/jenkins/abp/activity)
[![NuGet](https://img.shields.io/nuget/v/Volo.Abp.Core.svg?style=flat-square)](https://www.nuget.org/packages/Volo.Abp.Core)
[![NuGet Download](https://img.shields.io/nuget/dt/Volo.Abp.Core.svg?style=flat-square)](https://www.nuget.org/packages/Volo.Abp.Core)
[![MyGet (with prereleases)](https://img.shields.io/myget/abp-nightly/vpre/Volo.Abp.Core.svg?style=flat-square)](https://docs.abp.io/en/abp/latest/Nightly-Builds)

This project is the next generation of the [ASP.NET Boilerplate](https://aspnetboilerplate.com/) web application framework. See [the announcement](https://abp.io/blog/abp/Abp-vNext-Announcement).

See the official [web site (abp.io)](https://abp.io/) for more information.

## 前后端分离的用户角色授权管理
- [https://github.com/luoyunchong/abp/tree/master/samples/Microservice/modules/app-microservice](https://github.com/luoyunchong/abp/tree/master/samples/Microservice/modules/app-microservice)
- 基于iview的SPA界面、从MicroserviceDemo中修改，
## 效果图如下
## 登录
![](https://pic.superbed.cn/item/5d3bd731451253d178c18d8f.png)

## 个人资料-修改密码
![](https://pic.superbed.cn/item/5d3bd9f4451253d178c1bc80.png)
## 个人资料-用户基本信息
![](https://pic.superbed.cn/item/5d3bd731451253d178c18d98.png)

## 用户管理
![](https://ae01.alicdn.com/kf/H77457b26dcbb4d08b5d26a629d8078b7o.png)
## 用户管理-编辑用户-配置角色-配置组织机构
![](https://pic.superbed.cn/item/5d3bda8f451253d178c1c0e7.png)
## 用户管理-用户权限配置
![](https://pic.superbed.cn/item/5d3bd82f451253d178c1a06f.png)

## 角色管理
![](https://pic2.superbed.cn/item/5d3bd8ce451253d178c1aafb.png)
## 角色管理-编辑角色
![](https://pic.superbed.cn/item/5d3bd9bb451253d178c1b9e6.png)
## 角色管理-角色权限配置
![](https://ae01.alicdn.com/kf/H973c2b0a50a545808db2d0b2f75250aa3.png)

## 部门管理-部门信息列表-部门新增，编辑
![](https://pic.superbed.cn/item/5d3bd9ce451253d178c1bb73.png)

## 审计日志
![](https://pic.superbed.cn/item/5d3bda81451253d178c1c058.png)

## 审计日志-日志详情
![](https://pic.superbed.cn/item/5d3bda81451253d178c1c05a.png)


## 基础资料字典-新增，列表
![](https://pic.superbed.cn/item/5d3bd9b5451253d178c1b997.png)
## 基础资料字典类别管理-新增，列表
![](https://pic.superbed.cn/item/5d3bd731451253d178c18d9b.png)



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
