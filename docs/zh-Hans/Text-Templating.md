# 文本模板

## 介绍

ABP框架提供了一个简单有效的文本模板系统,文本模板用于动态渲染基于模板和模型(数据对象)内容:

Template + Model =renderer=> Rendered Content

它非常类似于 ASP.NET Core Razor View (或 Page):

*RAZOR VIEW (或 PAGE) + MODEL ==render==> HTML CONTENT*

你可以将渲染的输出用于任何目的,例如发送电子邮件或准备一些报告.

模板渲染引擎非常强大:

* 它支持**条件逻辑**, **循环**等等.
* 模板内容**可以本地化**.
* 你可以为其他渲染模板定义**布局模板**。
* 对于高级场景,你可以传递任何对象到模板上下文.

ABP框架提供了两个模板引擎:

* **[Razor](Text-Templating-Razor.md)**
* **[Scriban](Text-Templating-Scriban.md)**

你可以在同一个应用应用程序中使用不同的模板引擎, 或者创建一个新的自定义模板引擎.

## 源码

查看开发和引用的[应用程序示例源码](https://github.com/abpframework/abp-samples/tree/master/TextTemplateDemo).

## 另请参阅

* 本文开发和引用的[应用程序示例源码](https://github.com/abpframework/abp-samples/tree/master/TextTemplateDemo).
* [本地化系统](Localization.md).
* [虚拟文件系统](Virtual-File-System.md).