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

## 解决方案结构

使用以上命令创建解决方案后,你会得到如下所示的解决方案:

![basic-console-application-solution](../images/basic-console-application-solution.png)

* `HelloWorldService` 是一个实现了 `ITransientDependency` 接口的示例服务. 它会自动注册到[依赖注入](../Dependency-Injection.md)系统.