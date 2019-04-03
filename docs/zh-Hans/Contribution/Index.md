**草案**: 该文档已作为草稿创建.
请参见 https://help.github.com/en/articles/setting-guidelines-for-repository-contributors

# 贡献于ASP.NET Boilerplate

ASP.NET Boilerplate是一个[开源](https://github.com/aspnetboilerplate/aspnetboilerplate)和社区驱动的项目. 本指南旨在帮助任何人想要为项目做出贡献.

## 代码贡献

你始终可以将PR(Pull Request)发送到GitHub存储库.

 - 从GitHub克隆[ASP.NET Boilerplate存储库](https://github.com/aspnetboilerplate/aspnetboilerplate/).
 - 进行必要的更改.
 - 发送PR(Pull Request).

在进行任何更改之前，请在[GitHub Issue页面]（(ttps://github.com/aspnetboilerplate/aspnetboilerplate/issues)上进行讨论. 因此,其他开发人员不会处理相同的问题, 你的PR将有更好的机会被接受.

### 修复BUG和增强功能

你可能想要修复已知BUG或处理计划的增强功能. 请参阅GitHub上的[Issue列表](https://github.com/aspnetboilerplate/aspnetboilerplate/issues)

### 功能请求

如果你有一个关于框架或模块的功能想法,在GitHub上创建一个Issue(https://github.com/aspnetboilerplate/aspnetboilerplate/issues/new)或参加现有的讨论. 然后,如果它被社区所接受,你就可以实施它.

## 文档贡献

你可能希望改进[文档](https://aspnetboilerplate.com/Pages/Documents). 如果是,请按照下列步骤操作:

* 从GitHub克隆[ABP存储库](https://github.com/aspnetboilerplate/aspnetboilerplate/).
* 文档位于 [/aspnetboilerplate/doc](https://github.com/aspnetboilerplate/aspnetboilerplate/tree/master/doc/WebSite)文件夹中.
* 修改文件并发送PR(Pull Request).
* 如果要添加新文档,还需要将其添加到导航文档中. 导航文档位于[doc/WebSite/Navigation.md](https://github.com/aspnetboilerplate/aspnetboilerplate/blob/master/doc/WebSite/Navigation.md).

## 资源本地化

ASP.NET Boilerplate框架有一个[本地化系统](https://aspnetboilerplate.com/Pages/Documents/Localization). 本地化资源位于[Abp\Localization\Sources\AbpXmlSource](https://github.com/aspnetboilerplate/aspnetboilerplate/tree/dev/src/Abp/Localization/Sources/AbpXmlSource). 
你可以添加新翻译或更新现有翻译.
要添加缺少的翻译,请参阅[此示例请求](https://github.com/aspnetboilerplate/aspnetboilerplate/pull/2471)

## 编写新模块

该框架具有预构建模块, 你还可以添加新模块. [Abp.Dapper](https://github.com/aspnetboilerplate/aspnetboilerplate/tree/dev/src/Abp.Dapper)是一个贡献模块. 你可以检查Abp.Dapper模块来制作你自己的模块.

TODO：可以逐步添加模块开发指南.

## 博客文章和教程

如果你想为ASP.NET Boilerplate编写教程或博客文章,请告诉我们(通过创建GitHub Issue)(https://github.com/aspnetboilerplate/aspnetboilerplate/issues), 以便我们添加链接到你的教程/帖子在官方文档中,我们在官方[Twitter帐户](https://twitter.com/aspboilerplate).

## 报告BUG

如果您想报告BUG, 请[在GitHub存储库上创建一个Issue](https://github.com/aspnetboilerplate/aspnetboilerplate/issues/new)

你需要在发布BUG之前填写问题模板.

```markdown
### GitHub Issues

GitHub issues are for bug reports, feature requests and other discussions about the framework.

If you're creating a bug/problem report, please include followings:

* Your Abp package version.
* Your base framework: .Net Framework or .Net Core.
* Exception message and stack trace if available.
* Steps needed to reproduce the problem.

Please write in English.

### Stack Overflow

Please use Stack Overflow for your questions about using the framework, templates and samples:

https://stackoverflow.com/questions/tagged/aspnetboilerplate

Use aspnetboilerplate tag in your questions.

```
