# 贡献指南

ABP是[开源](https://github.com/abpframework)和社区驱动项目. 本指南旨在帮助任何想要为项目做出贡献的人.

## ABP 社区网站

如果你可编写文章或关于ASP框架和ASP.NET Core的 "如何" 指南,请提交你的文章到[community.abp.io](https://community.abp.io/)网站.

## 贡献代码

你可以将Pull request(拉取请求)发送到Github存储库.

- 从 Github 上 [Fork](https://docs.github.com/en/free-pro-team@latest/github/getting-started-with-github/fork-a-repo) [ABP 存储库](https://github.com/abpframework/abp/).
- 使用 `/build/build-all.ps1 -f` 构建一次存储库.
- 进行必要的更改，包括单元/集成测试。
- 发送一个 pull request.

> 在 Visual Studio 中完全打开解决方案后，可能需要在解决方案的根文件夹中执行一次`dotnet restore`。这是必需的，因为VS无法正确解析解决方案中对项目的本地引用。

### GitHub Issues

在进行任何更改之前,请在[Github Issues](https://github.com/abpframework/abp/issues)上进行讨论. 通过这种方式, 其他开发人员将不会处理同一个问题, 你的PR将有更好的机会被接受.

#### Bug修复 & 增强功能

你可能希望修复已知Bug或处理计划的增强功能. 请参阅Github上的[问题列表](https://github.com/abpframework/abp/issues).

#### 功能请求

如果你对框架或模块有功能的想法, 请在Github上[创建一个问题](https://github.com/abpframework/abp/issues/new)或参加现有的讨论. 如果它被社区所接受你就可以实现它.

## 文档翻译

你可能希望将完整的[文档](https://abp.io/documents/)(包括本文)翻译成你的母语. 请按照下列步骤操作:

* 从 Github 克隆 [ABP 存储库](https://github.com/abpframework/abp/).
* To add a new language, create a new folder inside the [docs](https://github.com/abpframework/abp/tree/master/docs) folder. Folder names can be "en", "es", "fr", "tr" and so on based on the language (see [all culture codes](https://msdn.microsoft.com/en-us/library/hh441729.aspx)).
* Get the ["en" folder](https://github.com/abpframework/abp/tree/master/docs/en) as a reference for the file names and folder structure. Keep the same naming if you are translating the same documentation.
* Send a pull request (PR) once you translate any document. Please translate documents & send PRs one by one. Don't wait to finish translations for all documents.

在[ABP文档网站](https://docs.abp.io)上新添加语言之前,需要翻译一些基本文档:

* Index (Home)
* Getting Started
* Web Application Development Tutorial

完成了这些基本的翻译后,将添加一种新的语言

## 资源本地化

ABP框架具有灵活的[本地化系统](../Localization.md). 你可以为自己的应用程序创建本地化用户界面.

除此之外，框架和[预构建模块](https://docs.abp.io/en/abp/latest/Modules/Index)都有本地化的文本。例如，请参阅 [Volo.Abp.UI 程序包的本地化文本](https://github.com/abpframework/abp/blob/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi/en.json). 

### 使用 "abp translate" 命令

这是推荐的方法，因为它会自动查找特定区域性的所有缺失文本，并允许您在一个位置进行翻译。

* 从Github克隆[ABP存储库](https://github.com/abpframework/abp/).
* 安装[ABP CLI](https://docs.abp.io/en/abp/latest/CLI).
* 在abp仓储的根文件夹为你的语言运行`abp translate -c <culture-name>`命令. 例如对法语使用 `abp translate -c fr`, 检查[文档](https://docs.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/supported-culture-codes)找到你所用语言的文化代码.
* 命令会在同一文件夹下创建 `abp-translation.json` 文件, 使用你喜欢的编辑器打开这个文件并填写缺少的文本值.
* 一旦你完成了翻译,使用 `abp translate -a` 命令应用更改到相关的文件.
* 在GitHub上发送PR.

### 手动翻译

如果你想更改特定的资源文件,你可以自己找到这个文件进行必要的更改(或为你的语言创建新文件),并在GitHub上发送PR.

## Bug 报告

如果你发现任何Bug, 请在 Github 存储库上[创建一个问题](https://github.com/abpframework/abp/issues/new).

## 设置前端开发环境

[如何作为前端开发人员为 abp.io 做出贡献](How-to-Contribute-abp.io-as-a-frontend-developer.md)

## 另请参见

* [ABP 社区 Talks 2022.4: 如何为 ABP 框架做出贡献？](https://www.youtube.com/watch?v=Wz4Z-O-YoPg&list=PLsNclT2aHJcOsPustEkzG6DywiO8eh0lB)