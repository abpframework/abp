## 贡献指南

ABP是[开源](https://github.com/abpframework)和社区驱动项目. 本指南旨在帮助任何想要为项目做出贡献的人.

### 贡献代码

你可以将Pull request(拉取请求)发送到Github存储库.

- 从Github克隆[ABP存储库](https://github.com/abpframework/abp/).
- 进行必要的更改.
- 发送Pull request(拉取请求).

在进行任何更改之前,请在[Github问题](https://github.com/abpframework/abp/issues)上进行讨论. 通过这种方式, 其他开发人员将不会处理同一个问题, 你的PR将有更好的机会被接受.

#### Bug修复 & 增强功能

你可能希望修复已知Bug或处理计划的增强功能. 请参阅Github上的[问题列表](https://github.com/abpframework/abp/issues).

#### 功能请求

如果你对框架或模块有功能的想法, 请在Github上[创建一个问题](https://github.com/abpframework/abp/issues/new)或参加现有的讨论. 如果它被社区所接受你就可以实现它.

### 文档翻译

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

### 资源本地化

ABP框架具有灵活的[本地化系统](../Localization.md). 你可以为自己的应用程序创建本地化用户界面.

除此之外,框架和预构建模块已经本地化了文本.请参阅[Volo.Abp.UI包的本地化文本](https://github.com/abpframework/abp/blob/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi/en.json).你可以在[相同文件夹](https://github.com/abpframework/abp/tree/master/framework/src/Volo.Abp.UI/Localization/Resources/AbpUi)中创建一个新文件进行翻译.

* 从Github克隆[ABP存储库](https://github.com/abpframework/abp/).
* 为本地化文本(json)文件(en.json文件同目录下)创建目标语言的新文件.
* 复制en.json文件中的所有文本.
* 翻译文本.
* 在Github上发送拉取请求(Pull request).

ABP是一个模块化框架. 所以有很多本地化文本资源, 每个模块都有一个. 要查找所有.json文件,可以在克隆存储库后搜索"en.json". 你还可以检查[此列表](Localization-Text-Files.md)以获取本地化文本文件列表.

### 博客文章和教程

如果你发布了一些ABP框架的教程或博客帖子, 请通知我们(通过创建[Github问题](https://github.com/abpframework/abp/issues)), 我们可能会在官方文档中添加指向你的教程或博客帖子的链接和在[推特](https://twitter.com/abpframework)上公布.

### Bug 报告

如果你发现任何Bug, 请[在Github存储库上创建一个问题](https://github.com/abpframework/abp/issues/new).