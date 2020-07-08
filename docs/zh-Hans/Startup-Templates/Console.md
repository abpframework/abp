# 控制台应用程序启动模板

此模板用于创建一个最小的依赖关系的ABP控制台应用程序项目.

## 如何开始?

首先,如果你没有安装[ABP CLI](../CLI.md),请先安装它:

````bash
dotnet tool install -g Volo.Abp.Cli
````

在一个空文件夹使用 `abp new` 命令创建新解决方案:

````bash
abp new Acme.MyConsoleApp -t console
````

`Acme.MyConsoleApp` 是解决方案的名称,  如*YourCompany.YourProduct*. 你可以使用单级或多级名称.