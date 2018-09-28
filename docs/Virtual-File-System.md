## Virtual File System

The Virtual File System makes it possible to manage files that do not physically exists on the file system (disk). It's mainly used to embed (js, css, image, cshtml...) files into assemblies and use them like physical files on runtime.

### Volo.Abp.VirtualFileSystem Package

Volo.Abp.VirtualFileSystem is the core package of the virtual file system. Install it to your project using the package manager console (PMC):

```
Install-Package Volo.Abp.VirtualFileSystem
```

> This package is already installed by default with the startup template. So, most of the time, you don't need to install it manually.

Then you can add **AbpVirtualFileSystemModule** dependency to your module:

```c#
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpVirtualFileSystemModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
```

#### Registering Embedded Files

A file should be first marked as embedded resource to embed the file into the assembly. The easiest way to do it is to select the file from the **Solution Explorer** and set **Build Action** to **Embedded Resource** from the **Properties** window. Example:

![build-action-embedded-resource-sample](images/build-action-embedded-resource-sample.png)

If you want to add multiple files, this can be tedious. Alternatively, you can directly edit your **.csproj** file:

````C#
<ItemGroup>
  <None Remove="MyResources\**\*.*" />
</ItemGroup>
````

This configuration recursively adds all files under the **MyResources** folder of the project (including the files you will add in the future).

Then the module should be configured using `VirtualFileSystemOptions` to register the embedded files to the virtual file system. Example:

````C#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpVirtualFileSystemModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<VirtualFileSystemOptions>(options =>
            {
                //Register all embedded files of this assembly to the virtual file system
                options.FileSets.AddEmbedded<MyModule>();
            });

            //...
        }
    }
}
````

The `AddEmbedded` extension method takes a class, finds all embedded files from the assembly of the given class and registers them to the virtual file system. More concisely it could be written as shown below:

````C#
options.FileSets.Add(new EmbeddedFileSet(typeof(MyModule).Assembly));
````

#### Getting Virtual Files: IVirtualFileProvider

After embedding a file into an assembly and registering to the virtual file system, `IVirtualFileProvider` can be used to get files or directory contents:

````C#
public class MyService
{
    private readonly IVirtualFileProvider _virtualFileProvider;

    public MyService(IVirtualFileProvider virtualFileProvider)
    {
        _virtualFileProvider = virtualFileProvider;
    }

    public void Foo()
    {
        //Getting a single file
        var file = _virtualFileProvider.GetFileInfo("/MyResources/js/test.js");
        var fileContent = file.ReadAsString(); //ReadAsString is an extension method of ABP

        //Getting all files/directories under a directory
        var directoryContents = _virtualFileProvider.GetDirectoryContents("/MyResources/js");
    }
}
````

#### Dealing With Embedded Files During Development

Embedding a file into a module assembly and using it from another project by just referencing the assembly (or adding a nuget package) is very valuable for creating a re-usable module. However, it makes it a bit hard to develop the module itself.

Assume that you are developing a module that contains an embedded JavaScript file. Whenever you change the file you must re-compile the project, re-start the application and refresh the browser page to take the change. Obviously, it is very time consuming and tedious.

What is needed is the ability for the application to directly use the physical file at development time and a browser refresh shows any change in the JavaScript file. `ReplaceEmbeddedByPyhsical` method makes that possible. 

The example below shows an application depends on a module (`MyModule`) that contains embedded files and the application can reach the source code of the module on the development time. 

````C#
[DependsOn(typeof(MyModule))]
public class MyWebAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment()) //only for development time
        {
            context.Services.Configure<VirtualFileSystemOptions>(options =>
            {
                //ReplaceEmbeddedByPyhsical gets the root folder of the MyModule project
                options.FileSets.ReplaceEmbeddedByPyhsical<MyModule>(
                    Path.Combine(hostingEnvironment.ContentRootPath, "..\\MyModuleProject")
                );
            });
        }

        //...
    }
}
````

The code above assumes that `MyWebAppModule` and `MyModule` are two different projects in a Visual Studio solution and `MyWebAppModule` depends on the `MyModule`.

### ASP.NET Core Integration

Virtual File System is well integrated to ASP.NET Core:

* Virtual files can be used just like physical (static) files on a web application.
* Razor Views, Razor Pages, js, css, image files and all other web contents can be embedded into assemblies and used just like the physical files.
* An application (or another module) can override a virtual file of a module just like placing a file with the same name and extension into the same folder of the virtual file.

#### Virtual Files Middleware

Virtual Files Middleware is used to serve embedded (js, css, image...) files to clients/browsers just like physical files in the **wwwroot** folder. Add it just after the static file middleware as shown below:

````C#
app.UseVirtualFiles();
````

Adding virtual files middleware after the static files middleware makes it possible to override a virtual file by a physical file by placing in the same location.

> Virtual Files Middleware only serves the virtual wwwroot folder contents just like the static files.

#### Views & Pages

Embedded razor views/pages are available in the application without any configuration. Just place them into standard Views/Pages virtual folders in the module development.

An embedded view/page can be overrided if a module/application locates a new file into the same location.
