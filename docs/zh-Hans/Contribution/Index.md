# 贡献指南

ABP是[开源](https://github.com/abpframework)和社区驱动项目. 本指南旨在帮助任何想要为项目做出贡献的人.

## community.abp.io

如果你可编写文章或关于ASP框架和ASP.NET Core的 "如何" 指南,请提交你的文章到[community.abp.io](https://community.abp.io/)网站.

## 贡献代码

你可以将Pull request(拉取请求)发送到Github存储库.

- 从Github克隆[ABP存储库](https://github.com/abpframework/abp/).
- 进行必要的更改.
- 发送Pull request(拉取请求).

在进行任何更改之前,请在[Github问题](https://github.com/abpframework/abp/issues)上进行讨论. 通过这种方式, 其他开发人员将不会处理同一个问题, 你的PR将有更好的机会被接受.

### Bug修复 & 增强功能

你可能希望修复已知Bug或处理计划的增强功能. 请参阅Github上的[问题列表](https://github.com/abpframework/abp/issues).

### 功能请求

如果你对框架或模块有功能的想法, 请在Github上[创建一个问题](https://github.com/abpframework/abp/issues/new)或参加现有的讨论. 如果它被社区所接受你就可以实现它.

## 文档翻译

你可能希望将完整的[文档](https://abp.io/documents/)(包括本文)翻译成你的母语. 请按照下列步骤操作:

* 从Github克隆[ABP存储库](https://github.com/abpframework/abp/).
* 要添加新语言,请在[docs](https://github.com/abpframework/abp/tree/master/docs)文件夹中创建一个新文件夹. 文件夹名称可以是" en","es","fr","tr"等(参见[所有文化代码](https://msdn.microsoft.com/en-us/library/hh441729.aspx)).
* 获取["en"文件夹](https://github.com/abpframework/abp/tree/master/docs/en)作为文件名和文件夹结构的参考. 如果要翻译相同的文档, 请保持相同的命名.
* 翻译任何文档后发送拉取请求(PR). 请翻译文件后及时发送PR. 不要等到完成所有文件的翻译.

在[ABP文档网站](https://docs.abp.io)上新添加语言之前,需要翻译一些基本文档:

* 入门文档
* 教程
* CLI

完成了这些基本的翻译后,将添加一种新的语言

## 资源本地化

ABP框架具有灵活的[本地化系统](../Localization.md). 你可以为自己的应用程序创建本地化用户界面.

除此之外,框架和预构建模块已经本地化了文本.请参阅[Volo.Abp.UI包的本地化文本](https://github.com/abpframework/abp/blob/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi/en.json).

### 使用 "abp translate" 命令

这是推荐的方法,因为它会自动查找所有缺少的文本的特定文化,让你在一个地方翻译.

* 从Github克隆[ABP存储库](https://github.com/abpframework/abp/).
* 安装[ABP CLI](https://docs.abp.io/en/abp/latest/CLI).
* 在abp仓储的根文件夹为你的语言运行`abp translate -c <culture-name>`命令. 例如对法语使用 `abp translate -c fr`, 检查[文档](https://docs.microsoft.com/en-us/bingmaps/rest-services/common-parameters-and-types/supported-culture-codes)找到你所用语言的文化代码.
* 命令会在同一文件夹下创建 `abp-translation.json` 文件, 使用你喜欢的编辑器打开这个文件并填写缺少的文本值.
* 一旦你完成了翻译,使用 `abp translate -a` 命令应用更改到相关的文件.
* 在GitHub上发送PR.

### 手动翻译

如果你想更改特定的资源文件,你可以自己找到这个文件进行必要的更改(或为你的语言创建新文件),并在GitHub上发送PR.

## Bug 报告

如果你发现任何Bug, 请[在Github存储库上创建一个问题](https://github.com/abpframework/abp/issues/new).