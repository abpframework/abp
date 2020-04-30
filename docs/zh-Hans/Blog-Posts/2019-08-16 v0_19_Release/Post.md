# 发布ABP v0.19包含Angular UI选项

ABP v0.19已发布,包含解决的[~90个问题](https://github.com/abpframework/abp/milestone/17?closed=1)和[600+次提交](https://github.com/abpframework/abp/compare/0.18.1...0.19.0).

## 新功能

### Angular UI

终于,ABP有了一个**SPA UI**选项,使用最新的[Angular](https://angular.io/)框架.Angular的集成不是简单地创建了一个启动模板.

* 创建了一个基础架构来处理ABP的模块化,主题和其他一些功能.此基础结构已部署为[NPM包](https://github.com/abpframework/abp/tree/dev/npm/ng-packs/packages).
* 为帐户,身份和租户管理等模块创建了Angular UI包.
* 创建了一个最小的启动模板,使用IdentityServer进行身份验证并使用ASP.NET Core做为后端.此模板使用上面提到的包.
* 更新了[ABP CLI](https://docs.abp.io/en/abp/latest/CLI)和[下载页面](https://abp.io/get-started),以便能够使用新的UI选项生成项目.
* 创建了[教程](https://docs.abp.io/en/abp/latest/Tutorials/Angular/Part-I)以使用新的UI选项快速入门.

我们基于最新的Angular工具和趋势创建了模板,文档和基础架构:

* 使用[NgBootstrap](https://ng-bootstrap.github.io/)和[PrimeNG](https://www.primefaces.org/primeng/)作为UI组件库.你可以使用自己喜欢的库,没问题,但预构建的模块可以使用这些库.
* 使用[NGXS](https://ngxs.gitbook.io/ngxs/)作为状态管理库.

Angular是第一个SPA UI选项,但它不是最后一个.在v1.0发布之后,我们将开始第二个UI选项的工作.虽然尚未决定,但候选的有Blazor,React和Vue.js. 等待你的反馈.你可以使用以下issue进行投票(thumb):

* [Blazor](https://github.com/abpframework/abp/issues/394)
* [Vue.js](https://github.com/abpframework/abp/issues/1168)
* [React](https://github.com/abpframework/abp/issues/1638)

### Widget系统

[Widget系统](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Widgets)允许为ASP.NET Core MVC应用程序**定义和重用**Widget.Widget可能有自己的脚本和样式资源以及由ABP框架管理的第三方库的依赖关系.

### 其他

我们已经解决了许多错误,并根据社区反馈开发了现有功能.有关所有已结束的问题,请参阅[v0.19里程碑](https://github.com/abpframework/abp/milestone/17?closed=1).

## 路线图

我们决定等待**ASP.NET Core 3.0**最终发布.微软已宣布将于9月23日至25日在[.NET Conf](https://www.dotnetconf.net/)上发布它.

我们已经计划完成我们的工作,并迁移到ASP.NET Core 3.0(预览版或RC版)在它发布之前.一旦Microsoft发布它,我们将立即开始升级并测试最终版本.

因此,你可以期待ABP **v1.0**将在**10月上半月**发布.我们非常兴奋并努力地工作着.

你可以关注[GitHub里程碑](https://github.com/abpframework/abp/milestones)的进度.

我们不会在v1.0之前添加主要功能.
