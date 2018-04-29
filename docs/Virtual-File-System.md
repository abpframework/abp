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



A file should be first registered in order to use it in the application. Example:

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

AddEmbedded extension method takes a

s