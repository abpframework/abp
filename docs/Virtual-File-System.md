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



s