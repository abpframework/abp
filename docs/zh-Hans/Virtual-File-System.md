## 虚拟文件系统

虚拟文件系统使得管理物理上不存在于文件系统中(磁盘)的文件成为可能. 它主要用于将(js, css, image...)文件嵌入到程序集中, 并在运行时将它们像物理文件一样使用.

## 安装

> 大多数情况下你不需要手动安装这个包,因为[应用程序启动模板](Startup-Templates/Application.md)已经预先安装.

[Volo.Abp.VirtualFileSystem](https://www.nuget.org/packages/Volo.Abp.VirtualFileSystem) 是虚拟文件系统的NuGet主页.

使用ABP CLIi添加包到你的项目:

* 安装[ABP CLI](https://docs.abp.io/en/abp/latest/CLI),如果你还没有安装.
* 在你想要添加 `Volo.Abp.VirtualFileSystem` 包的项目的 `.csproj` 文件目录打开命令行(终端).
* 运行 `abp add-package Volo.Abp.VirtualFileSystem` 命令.

如果你想要手动安装,安装[Volo.Abp.VirtualFileSystem](https://www.nuget.org/packages/Volo.Abp.VirtualFileSystem)NuGet包到你的项目并且添加`[DependsOn(typeof(AbpVirtualFileSystemModule))]`到你项目的[ABP Module](Module-Development-Basics.md)类.

## 与嵌入式文件工作

### 嵌入文件

要将文件嵌入到程序集中, 首先需要把该文件标记为**嵌入式资源**. 最简单的方式是在 **解决方案管理器** 中选择文件, 然后找到 **"属性"** 窗口将 **"生成操作"** 设置为 **"嵌入式资源"**.
例如:

![build-action-embedded-resource-sample](images/build-action-embedded-resource-sample.png)

如果需要添加多个文件, 这样做会很乏味. 作为选择, 你可以直接编辑 **.csproj** 文件:

````C#
<ItemGroup>
  <EmbeddedResource Include="MyResources\**\*.*" />
  <Content Remove="MyResources\**\*.*" />
</ItemGroup>
````

此配置以递归方式添加项目的 **MyResources** 文件夹下的所有文件(包括将来新添加的文件).

如果文件名包含一些特殊字符,在项目/程序集中嵌入文件可能会导致问题. 为了克服这个限制;

1. 将[Microsoft.Extensions.FileProviders.Embedded](https://www.nuget.org/packages/Microsoft.Extensions.FileProviders.Embedded) NuGet包添加到包含嵌入式资源的项目中.
2. 添加 `<GenerateEmbeddedFilesManifest>true</GenerateEmbeddedFilesManifest>` 到 `.csproj` 文件的 `<PropertyConfig>...</PropertyConfig>` 部分中.

> 尽管这两个步骤是可选的,并且ABP无需这些配置即可工作,但强烈建议你这样做.

### 配置AbpVirtualFileSystemOptions

使用 `AbpVirtualFileSystemOptions` [选项类](Options.md)可以在[模块](Module-Development-Basics.md)的 `ConfigureServices` 方法中将嵌入式文件注册到虚拟文件系统.

**示例: 添加嵌入式文件到虚拟文件系统**

````csharp
Configure<AbpVirtualFileSystemOptions>(options =>
{
    options.FileSets.AddEmbedded<MyModule>();
});
````

`AddEmbedded` 扩展方法需要一个类, 从给定**类的程序集中查找所有嵌入文件**, 并将它们注册到虚拟文件系统. 通常将模块类作为通用参数传递.

`AddEmbedded` 有两个可选参数;

* `baseNamespace`: 仅在你没有配置上面说明的 `GenerateEmbeddedFilesManifest` 并且你的根名称空间不为空时,才可能需要这样做. 在这种情况下,请在此处设置你的根名称空间.
* `baseFolder`: 如果你不想公开项目中的所有嵌入式文件,只希望公开特定的文件夹(和子文件夹/文件),可以相对于项目根页面设置基本文件夹.

**示例: 添加项目中 `MyFiles` 目录下的文件**

````csharp
Configure<AbpVirtualFileSystemOptions>(options =>
{
    options.FileSets.AddEmbedded<MyModule>(
        baseNamespace: "Acme.BookStore.MyFiles",
        baseFolder: "/MyFiles"
    );
});
````

这个例子假设;

* 你的项目根(default)命令空间是 `Acme.BookStore`.
* 你的项目有一个名为 `MyFiles` 的目录.
* 你只想添加 `MyFiles` 目录到虚拟文件系统.

## IVirtualFileProvider

将文件嵌入到程序集中并注册到虚拟文件系统后,可以使用 `IVirtualFileProvider` 接口来获取文件或目录内容:

````C#
public class MyService : ITransientDependency
{
    private readonly IVirtualFileProvider _virtualFileProvider;

    public MyService(IVirtualFileProvider virtualFileProvider)
    {
        _virtualFileProvider = virtualFileProvider;
    }

    public void Test()
    {
        //Getting a single file
        var file = _virtualFileProvider
            .GetFileInfo("/MyResources/js/test.js");

        var fileContent = file.ReadAsString();

        //Getting all files/directories under a directory
        var directoryContents = _virtualFileProvider
            .GetDirectoryContents("/MyResources/js");
    }
}
````

### ASP.NET Core 集成

虚拟文件系统与 ASP.NET Core 无缝集成:

* 虚拟文件可以像Web应用程序上的物理(静态)文件一样使用.
* Js, css, 图像文件和所有其他Web内容可以嵌入到程序集中并像物理文件一样使用.
* 应用程序(或其他模块)可以**覆盖模块的虚拟文件**, 就像将具有相同名称和扩展名的文件放入虚拟文件的同一文件夹中一样.

#### 静态虚拟文件夹

默认情况下,ASP.NET Core仅允许 `wwwroot` 文件夹包含客户端使用的静态文件. 当你使用虚拟文件系统时以下文件夹也可以包含静态文件:

* Pages
* Views
* Components
* Themes

这允许你可以在 `.cshtml` 文件附近添加 `.js`, `.css`... 文件,更易于开发和维护你的项目.

#### 在开发过程中处理嵌入式文件

将文件嵌入到模块程序集中并能够通过引用程序集(或添加nuget包)在另一个项目中使用它对于创建可重用模块非常有价值. 但是, 这使得开发模块本身变得有点困难.

假设你正在开发一个包含嵌入式JavaScript文件的模块. 当你更改文件时, 你必须重新编译项目, 重新启动应用程序并刷新浏览器页面以使更改生效. 显然, 这是非常耗时和乏味的.

我们需要的是应用程序在开发时直接使用物理文件的能力, 让浏览器刷新时同步JavaScript文件的任何更改. `ReplaceEmbeddedByPhysical` 方法使其成为可能.

下面的示例展示了应用程序依赖于包含嵌入文件的模块("MyModule"), 并且应用程序可以在开发过程中直接使用模块的源代码.

````C#
[DependsOn(typeof(MyModule))]
public class MyWebAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment()) //only for development time
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.ReplaceEmbeddedByPhysical<MyModule>(
                    Path.Combine(
                        hostingEnvironment.ContentRootPath,
                        string.Format(
                            "..{0}MyModuleProject",
                            Path.DirectorySeparatorChar
                        )
                    )
                );
            });
        }
    }
}
````

上面的代码假设`MyWebAppModule`和`MyModule`是Visual Studio解决方案中的两个不同的项目, `MyWebAppModule`依赖于`MyModule`.

> [应用程序启动模板]已经为本地化文件应用这个方法,所以当你更改一个本地化文件时,它会自动检测到更改.

## 替换/重写虚拟文件

虚拟文件系统在运行时创建一个统一的文件系统,其中实际的文件在开发时被分配到不同的模块中.

如果两个模块将文件添加到相同的虚拟路径(如`my-path/my-file.css`),之后添加的模块将替换/替换前一个([模块依赖](Module-Development-Basics.md)顺序决定了添加文件的顺序).

此功能允许你的应用程序可以覆盖/替换定义应用程序所使用的模块的任何虚拟文件. 这是ABP框架的基本可扩展性功能之一.

因此,如果需要替换模块的文件,只需在模块/应用程序中完全相同的路径中创建该文件.

### 物理文件

物理文件总是覆盖虚拟文件. 这意味着如果你把一个文件放在 `/wwwroot/my-folder/my-file.css`,它将覆盖虚拟文件系统相同位置的文件.因此你需要知道在模块中定义的文件路径来覆盖它们.
