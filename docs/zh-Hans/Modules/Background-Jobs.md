# 后台作业模块

后台作业模块实现了 `IBackgroundJobStore` 接口,并且可以使用ABP框架的默认后台作业管理.如果你不想使用这个模块,那么你需要自己实现 `IBackgroundJobStore` 接口.

> 本文档仅介绍后台作业模块,该模块将后台作业持久化到数据库.有关后台作业系统的更多信息,请参阅[后台作业](../Background-Jobs.md)文档.

## 如何使用

当你使用ABP框架[创建一个新的解决方案](https://abp.io/get-started)时,这个模块是（作为NuGet/NPM包）预先安装的.你可以继续将其作为软件包使用并轻松获取更新,也可以将其源代码包含到解决方案中（请参阅 `get-source` [CLI](../CLI.md)命令）以开发自定义模块.

### 源代码

此模块的源代码可在[此处](https://github.com/abpframework/abp/tree/dev/modules/background-jobs)访问.源代码是由[MIT](https://choosealicense.com/licenses/mit/)授权的,所以你可以自由使用和定制它.

## 内部结构

### 领域层

#### 聚合

- `BackgroundJobRecord` (聚合根): 表示后台工作记录.

#### 仓储

为该模块定义了以下自定义仓储:

- `IBackgroundJobRepository`

### 数据库提供程序

#### 通用

##### 表/集合的前缀与架构

默认情况下,所有表/集合都使用 `Abp` 前缀.如果需要更改表前缀或设置架构名称（如果数据库提供程序支持）,请在 `BackgroundJobsDbProperties` 类上设置静态属性.

##### 连接字符串

此模块使用 `AbpBackgroundJobs` 作为连接字符串名称.如果不使用此名称定义连接字符串,它将返回 `Default` 连接字符串.有关详细信息,请参阅[连接字符串](https://docs.abp.io/en/abp/latest/Connection-Strings)文档.

#### Entity Framework Core

##### 表

- **AbpBackgroundJobs**

#### MongoDB

##### 集合

- **AbpBackgroundJobs**

## 另请参阅

* [后台作业系统](../Background-Jobs.md)
