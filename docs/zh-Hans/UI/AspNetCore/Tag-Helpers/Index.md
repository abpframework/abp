# ABP Tag Helpers

ABP框架定义了一组**标签助手组件**. 简化开发ASP.NET Core (MVC / Razor Pages) 应用程序界面.

## bootstrap 组件包装

大多数标签助手是[Bootstrap](https://getbootstrap.com/) (v5+)的包装. 编写bootstrap代码并不是那么简单,其中包含太多的重复HTML标签并且也没有类型安全. ABP标签助手使其 **简单** 并且 **类型安全**.

我们的目标并不是100%的包装bootstrap组件. 仍然可以编写 **原生bootstrap代码** (实际上标签助手生成的也是原生的bootstrap代码), 但我们建议尽量使用标签助手.

ABP框架还向标准bootstrap组件添加了一些**实用的功能**.

这里是ABP框架包装的组件列表:

* [Alerts](Alerts.md)
* [Buttons](Buttons.md)
* [Cards](Cards.md)
* [Collapse](Collapse.md)
* [Dropdowns](Dropdowns.md)
* [Grids](Grids.md)
* [List Groups](List-Groups.md)
* [Modals](Modals.md)
* [Paginator](Paginator.md)
* [Popovers](Popovers.md)
* [Progress Bars](Progress-Bars.md)
* [Tabs](Tabs.md)
* [Tooltips](Tooltips.md)
* ...

> 在为所有的标签助手完成文档之前,你可以访问 https://bootstrap-taghelpers.abp.io/ 查看在线示例.

## 表单元素

参阅 [demo](https://bootstrap-taghelpers.abp.io/Components/FormElements).

## 动态表单

参阅 [demo](https://bootstrap-taghelpers.abp.io/Components/DynamicForms).