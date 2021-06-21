
## ASP.NET Core MVC 捆绑 & 压缩

有许多方法可以捆绑&压缩客户端资源(JavaScript和CSS文件). 最常见的方式是:

* 使用Visual Studio[捆绑&压缩](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BundlerMinifier)扩展或者其它的[NuGet相关包](https://www.nuget.org/packages/BuildBundlerMinifier/).

* 使用[Gulp](https://gulpjs.com/)/[Grunt](https://gruntjs.com/)及其插件.

ABP内置了简单,动态,强大,模块化的方式.

### Volo.Abp.AspNetCore.Mvc.UI.Bundling 包

> 默认情况下已在启动模板安装此软件包. 大多数情况下,你不需要手动安装它.

将`Volo.Abp.AspNetCore.Mvc.UI.Bundling` nuget包安装到你的项目中:

````
install-package Volo.Abp.AspNetCore.Mvc.UI.Bundling
````

然后将`AbpAspNetCoreMvcUiBundlingModule`依赖项添加到你的模块上:

````C#
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiBundlingModule))]
    public class MyWebModule : AbpModule
    {
        //...
    }
}
````

### Razor Bundling Tag Helpers

创建bundle的最简单方法是使用`abp-script-bundle`或`abp-style-bundle` tag helpers. 例如:

````html
<abp-style-bundle name="MyGlobalBundle">
    <abp-style src="/libs/bootstrap/css/bootstrap.css" />
    <abp-style src="/libs/font-awesome/css/font-awesome.css" />
    <abp-style src="/libs/toastr/toastr.css" />
    <abp-style src="/styles/my-global-style.css" />
</abp-style-bundle>
````

`abp-script-bundle`定义了一个带有**唯一名称**的样式包:`MyGlobalBundle`. 使用方法很容易理解. 让我们看看它是如何*工作的*:

* 当首次请求时,ABP从提供的文件中 **(延迟)lazy** 创建. 后续将从 **缓存** 中返回内容. 这意味着如果你有条件地将文件添加到包中,它只执行一次, 并且条件的任何更改都不会影响下一个请求的包.
* 在`development`环境中ABP会将包文件**单独**添加到页面中, 其他环境(`staging`,`production`...)会自动捆绑和压缩.
* 捆绑文件可以是**物理**文件或[**虚拟/嵌入**](../../Virtual-File-System.md)的文件.
* ABP自动将 **版本查询字符串(version query string)** 添加到捆绑文件的URL中,以防止浏览器缓存. 如:?_v=67872834243042(从文件的上次更改日期生成). 即使捆绑文件单独添加到页面(在`development`环境中), 版本控制仍然有效.

#### 导入 Bundling Tag Helpers

> 默认情况下已在启动模板导入. 大多数情况下,你不需要手动安装它.

要使用`bundle tag helpers`, 你需要将其添加到`_ViewImports.cshtml`文件或页面中:

````
@addTagHelper *, Volo.Abp.AspNetCore.Mvc.UI.Bundling
````

#### 未命名的 Bundles

对于razor bundle tag helpers, `name`是**可选**. 如果没有定义一个名字,它将根据使用的捆绑文件名自动**计算生成**(they are **concatenated** and **hashed**) 例:

````html
<abp-style-bundle>
    <abp-style src="/libs/bootstrap/css/bootstrap.css" />
    <abp-style src="/libs/font-awesome/css/font-awesome.css" />
    <abp-style src="/libs/toastr/toastr.css" />
    @if (ViewBag.IncludeCustomStyles != false)
    {
        <abp-style src="/styles/my-global-style.css" />
    }
</abp-style-bundle>
````

这将潜在地创建**两个不同的bundles**(一个包括`my-global-style.css`而另一个则不包括).

**未命名的** bundles优点:

* 可以**有条件地将项目**添加到捆绑包中. 但这可能会导致基于条件的捆绑的存在多种变化.

**命名** bundles优点:

* 其他模块可以通过其名称为捆绑包做出贡献(参见下面的部分).

#### 单个文件

如果你只需要在页面中添加一个文件, 你可以使用`abp-script`或`abp-style`而不需要包含在`abp-script-bundle`或`abp-style-bundle`中. 例:

````xml
<abp-script src="/scripts/my-script.js" />
````

对于上面的示例,包名称将是 *scripts.my-scripts*("/"替换为"."). 所有捆绑功能也可以按预期应用于单个文件.

### Bundling 选项

如果你需要在 **多个页面中使用相同的包** 或想要使用更多 **强大功能**, 你可以在[模块](../../Module-Development-Basics.md)类中进行**配置**.

#### 创建一个新的捆绑包

用法示例:

````C#
[DependsOn(typeof(AbpAspNetCoreMvcUiBundlingModule))]
public class MyWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .ScriptBundles
                .Add("MyGlobalBundle", bundle => {
                    bundle.AddFiles(
                        "/libs/jquery/jquery.js",
                        "/libs/bootstrap/js/bootstrap.js",
                        "/libs/toastr/toastr.min.js",
                        "/scripts/my-global-scripts.js"
                    );
                });                
        });
    }
}
````

> 你可以在脚本和样式包中使用相同的名称(*MyGlobalBundle*), 因为它们被添加到不同的集合(`ScriptBundles`和`StyleBundles`).

在定义bundle之后, 可以使用上面定义的相同tag helpers将其包括在页面中. 例如:

````html
<abp-script-bundle name="MyGlobalBundle" />
````

这次tag helper定义中没有定义文件, 因为捆绑文件是由代码定义的.

#### 配置现有的 Bundle

ABP也支持[模块化](../../Module-Development-Basics.md)捆绑. 模块可以修改由依赖模块创建的捆绑包.
例如:

````C#
[DependsOn(typeof(MyWebModule))]
public class MyWebExtensionModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .ScriptBundles
                .Configure("MyGlobalBundle", bundle => {
                    bundle.AddFiles(
                        "/scripts/my-extension-script.js"
                    );
                });                
        });
    }
}
````

你也可以使用 `ConfigureAll` 方法配置所有现有的捆绑包:

````C#
[DependsOn(typeof(MyWebModule))]
public class MyWebExtensionModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options
                .ScriptBundles
                .ConfigureAll(bundle => {
                    bundle.AddFiles(
                        "/scripts/my-extension-script.js"
                    );
                });
        });
    }
}
````

### Bundle 贡献者

将文件添加到现有bundle似乎很有用. 如果你需要**替换**bundle中的文件或者你想**有条件地**添加文件怎么办?  定义bundle贡献者可为此类情况提供额外的功能.

一个bundle的贡献者使用自定义版本bootstrap.css替换示例:

````C#
public class MyExtensionGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.ReplaceOne(
            "/libs/bootstrap/css/bootstrap.css",
            "/styles/extensions/bootstrap-customized.css"
        );
    }
}
````

然后你可以按照下面的代码使用这个贡献者:

````C#
services.Configure<AbpBundlingOptions>(options =>
{
    options
        .ScriptBundles
        .Configure("MyGlobalBundle", bundle => {
            bundle.AddContributors(typeof(MyExtensionGlobalStyleContributor));
        });        
});
````

贡献者也可以在bundle tag helpers中使用.
例如:

````xml
<abp-style-bundle>
    <abp-style type="@typeof(BootstrapStyleContributor)" />
    <abp-style src="/libs/font-awesome/css/font-awesome.css" />
    <abp-style src="/libs/toastr/toastr.css" />
</abp-style-bundle>
````

`abp-style`和`abp-script`标签可以使用`type`属性(而不是`src`属性), 如本示例所示. 添加bundle贡献者时, 其依赖关系也会自动添加到bundle中.

#### 贡献者依赖关系

bundle贡献者可以与其他贡献者具有一个或多个依赖关系.
例如:

````C#
[DependsOn(typeof(MyDependedBundleContributor))] //Define the dependency
public class MyExtensionStyleBundleContributor : BundleContributor
{
    //...
}
````

添加bundle贡献者时,其依赖关系将 **自动并递归** 添加. **依赖顺序** 通过阻止 **重复** 添加的依赖关系. 即使它们处于分离的bundle中,也会阻止重复. ABP在页面中组织所有bundle并消除重复.

创建贡献者和定义依赖关系是一种跨不同模块组织bundle创建的方法.


#### 贡献者扩展

在某些高级应用场景中, 当用到一个bundle贡献者时,你可能想做一些额外的配置. 贡献者扩展可以和被扩展的贡献者无缝衔接.

下面的示例为 prism.js 脚本库添加一些样式:

````csharp
public class MyPrismjsStyleExtension : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/libs/prismjs/plugins/toolbar/prism-toolbar.css");
    }
}
````

然后你可以配置 `AbpBundleContributorOptions` 去扩展已存在的 `PrismjsStyleBundleContributor`.

````csharp
Configure<AbpBundleContributorOptions>(options =>
{
    options
        .Extensions<PrismjsStyleBundleContributor>()
        .Add<MyPrismjsStyleExtension>();
});
````

任何时候当 `PrismjsStyleBundleContributor` 被添加到bundle中时, `MyPrismjsStyleExtension` 也会被自动添加.

#### 访问 IServiceProvider

虽然很少需要它, 但是`BundleConfigurationContext`有一个`ServiceProvider`属性, 你可以在`ConfigureBundle`方法中解析服务依赖.

#### 标准包装贡献者

将特定的NPM包资源(js,css文件)添加到包中对于该包非常简单. 例如, 你总是为bootstrap NPM包添加`bootstrap.css`文件.

所有[标准NPM包](Client-Side-Package-Management.md)都有内置的贡献者. 例如,如果你的贡献者依赖于bootstrap,你可以声明它,而不是自己添加bootstrap.css.

````C#
[DependsOn(typeof(BootstrapStyleContributor))] //Define the bootstrap style dependency
public class MyExtensionStyleBundleContributor : BundleContributor
{
    //...
}
````

使用标准包的内置贡献者:

* 防止你输入**无效的资源路径**.
* 如果资源 **路径发生变化** (依赖贡献者将处理它),则防止更改你的贡献者.
* 防止多个模块添加**重复文件**.
* 以递归方式管理依赖项(如果需要,添加依赖项的依赖项).

##### Volo.Abp.AspNetCore.Mvc.UI.Packages 包

> 默认情况下已在启动模板安装此软件包. 大多数情况下,你不需要手动安装它.

标准包贡献者在`Volo.Abp.AspNetCore.Mvc.UI.Packages` NuGet包中定义.
将它安装到你的项目中:

````
install-package Volo.Abp.AspNetCore.Mvc.UI.Packages
````

然后将`AbpAspNetCoreMvcUiPackagesModule`模块依赖项添加到你的模块中;

````C#
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAspNetCoreMvcUiPackagesModule))]
    public class MyWebModule : AbpModule
    {
        //...
    }
}
````

#### Bundle 继承

在某些特定情况下, 可能需要从其他bundle创建一个 **新** bundle **继承**, 从bundle继承(递归)会继承该bundle的所有文件/贡献者. 然后派生的bundle可以添加或修改文件/贡献者**而无需修改**原始bundle.
例如:

````c#
services.Configure<AbpBundlingOptions>(options =>
{
    options
        .StyleBundles
        .Add("MyTheme.MyGlobalBundle", bundle => {
            bundle
                .AddBaseBundles("MyGlobalBundle") //Can add multiple
                .AddFiles(
                    "/styles/mytheme-global-styles.css"
                );
        });                
});
````

### 主题

主题使用标准包贡献者将库资源添加到页面布局. 主题还可以定义一些标准/全局包, 因此任何模块都可以为这些标准/全局包做出贡献. 有关更多信息, 请参阅[主题文档](Theming.md).

### 最佳实践 & 建议

建议为应用程序定义多个包, 每个包用于不同的目的.

* **全局包**: 应用程序中的每个页面都包含全局样式/脚本包. 主题已经定义了全局样式和脚本包. 你的模块可以为他们做出贡献.
* **布局包**: 这是针对单个布局的特定包. 仅包含在所有页面之间共享的资源使用布局. 使用bundling tag helpers创建捆绑包是一种很好的做法.
* **模块包**: 用于单个模块页面之间的共享资源.
* **页面包**: 为每个页面创建的特定包. 使用bundling tag helpers创建捆绑包作为最佳实践.

在性能,网络带宽使用和捆绑包的数量之间建立平衡.

### 参见

* [客户端包管理](Client-Side-Package-Management.md)
* [主题](Theming.md)
