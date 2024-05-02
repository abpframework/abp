# 预览版

预览版在发布ABP 框架的主要版本或功能版本之前约 4 周发布。它们是为开发人员发布的，以尝试提供反馈以获得更稳定的版本。

预览版的版本控制是类似这样的：

* 3.1.0-rc.1
* 4.0.0-rc.1

在稳定版本（如 3.1.0）之前，可能会发布多个预览版本（如 3.1.0-rc.2 和 3.1.0-rc.3）。

## 使用预览版

### 新解决方案

要创建用于测试预览版的项目，您可以选择[下载页面](https://abp.io/get-started)上的“预览”选项或带有 **--preview** 参数的[ABP CLI](CLI.md) **abp new** 命令：

````bash
abp new Acme.BookStore --preview
````

此命令将使用最新的预览版 NuGet 包、NPM 包和解决方案模板创建一个新项目。每当发布稳定版本时，您都可以使用解决方案abp switch-to-stable根文件夹中的命令切换到解决方案的稳定版本。

### 现有解决方案

如果您已有解决方案并希望使用/测试最新的预览版，请在解决方案的根文件夹中使用以下 [ABP CLI](CLI.md) 命令.

````bash
abp switch-to-preview
````

您可以稍后使用 `abp switch-to-stable ` 命令返回最新的稳定版.

````bash
abp switch-to-stable
````

## 提供反馈

如果您发现错误或想要提供任何类型的反馈，您可以在[GitHub 存储库](https://github.com/abpframework/abp/issues/new)上打开一个问题。
