## Virtual File System

Virtual File System makes possible to manage files those are not physically exists in the file system (disk). It's mainly used to embed files into assemblies and use them like physical files on runtime.

### Volo.Abp.VirtualFileSystem Package

Volo.Abp.VirtualFileSystem it the core package of the virtual file system. Install it to your project using the package manager console (PMC):

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

Then the module should configure `VirtualFileSystemOptions` to register embedded files to the virtual file system. Example:

````C#
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpVirtualFileSystemModule))]
    public class MyModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<VirtualFileSystemOptions>(options =>
            {
                //Register all embedded files of this assembly to the virtual file system
                options.FileSets.AddEmbedded<MyModule>();
            });

            //...
        }
    }
}
````

`AddEmbedded` extension method takes a class, finds all embedded files from the assembly of the given class and register to the virtual file system. It's a shortcut and could be written as shown below:

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

#### Dealing Embedded Files On The Development

Embedding a file into a module assembly and using it from another project by just referencing the assembly (or adding a nuget package) is very valuable for creating a re-usable module. However, it makes a bit hard to develop the module itself.

Assume that you are developing a module that contains an embedded javascript file. Whenever you change the file you must re-compile the project, re-start the application and refresh the browser page to take the change. Obviously, it is very time consuming and tedious.

What if the application could directly use the physical file in the development time? Thus, you could just refresh the browser page to see any change in the javascript file. `ReplaceEmbeddedByPyhsical` method makes that possible. 

The example below shows an application depends on a module (`MyModule`) that contains embedded files and the application can reach the source code of the module on the development time. 

````C#
[DependsOn(typeof(MyModule))]
public class MyWebAppModule : AbpModule
{
    public override void ConfigureServices(IServiceCollection services)
    {
        var hostingEnvironment = services.GetHostingEnvironment();

        if (hostingEnvironment.IsDevelopment()) //only for development time
        {
            services.Configure<VirtualFileSystemOptions>(options =>
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

