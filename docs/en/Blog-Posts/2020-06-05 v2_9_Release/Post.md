# ABP Framework v2.9 Has Been Released

The **ABP Framework** & and the **ABP Commercial** version 2.9 have been released, which is the last version before 3.0! This post will cover **what's new** with these this release and what you can expect from the version 3.0.

## What's New with the ABP Framework 2.9?

You can see all the changes on the [GitHub release notes](https://github.com/abpframework/abp/releases/tag/2.9.0). This post will only cover the important features/changes.

### Pre-Compiling Razor Pages

Pre-built pages (for [the application modules](https://docs.abp.io/en/abp/latest/Modules/Index)) and view components were compiling on runtime until this version. Now, they are pre-compiled and we've measured that the application startup time (for the MVC UI) has been 50% reduced. In other words, it has **two-times faster** than the previous version. Not only for the application startup, the speed change also effects when you visit a page for the first time.

You do nothing to get the benefit of the new system. [Overriding UI pages/components](https://docs.abp.io/en/abp/latest/UI/AspNetCore/Customization-User-Interface) are working just like before.

### New Blob Storing Package

We've created a new [Blob Storing package](https://www.nuget.org/packages/Volo.Abp.BlobStoring) to store arbitrary binary object. It is generally used to store files in your application. This package provides an abstraction, so any application or [module](https://docs.abp.io/en/abp/latest/Module-Development-Basics) can save and retrieve files independent from the actual storing provider.

There are two storing provider currently implemented:

* [Volo.Abp.BlobStoring.FileSystem](https://www.nuget.org/packages/Volo.Abp.BlobStoring.FileSystem) package stores objects/files in the local file system with a configured folder.
* [Volo.Abp.BlobStoring.Database](https://github.com/abpframework/abp/tree/dev/modules/blob-storing-database) module stores objects/files in a database. It currently supports [Entity Framework Core](https://docs.abp.io/en/abp/latest/Entity-Framework-Core) (so, you can use [any relational DBMS](https://docs.abp.io/en/abp/latest/Entity-Framework-Core-Other-DBMS)) and [MongoDB](https://docs.abp.io/en/abp/latest/MongoDB).

[Azure BLOB provider](https://github.com/abpframework/abp/issues/4098) will be available with v3.0. You can request other cloud providers or contribute yourself on the [GitHub repository](https://github.com/abpframework/abp/issues/new).

One of the benefit of the blob storing system it allows you to create multiple containers (each container is a blob storage) and use different storing providers for each container.

**Example: Use the default container to save and get a byte array**

````csharp
public class MyService : ITransientDependency
{
    private readonly IBlobContainer _container;

    public MyService(IBlobContainer container)
    {
        _container = container;
    }

    public async Task FooAsync()
    {
        //Save a BLOB
        byte[] bytes = GetBytesFromSomeWhere();
        await _container.SaveAsync("my-unique-blob-name", bytes);
        
        //Retrieve a BLOB
        bytes = await _container.GetAllBytesAsync("my-unique-blob-name");
    }
}
````

It can work with `byte[]` and `Stream` objects.

**Example: Use a typed (named) container to save and get a stream**

````csharp
public class MyService : ITransientDependency
{
    private readonly IBlobContainer<TestContainer1> _container;

    public MyService(IBlobContainer<TestContainer1> container)
    {
        _container = container;
    }

    public async Task FooAsync()
    {
        //Save a BLOB
        Stream stream = GetStreamFromSomeWhere();
        await _container.SaveAsync("my-unique-blob-name", stream);
        
        //Retrieve a BLOB
        stream = await _container.GetAsync("my-unique-blob-name");
    }
}
````

`TestContainer1` is an empty class that has no purpose than identifying the container. 

A typed (named) container can be configured to use a different storing provider than the default one. It is a good practice to always use a typed container while developing re-usable modules, so the final application can configure provider for this container without effecting the other containers.

**Example: Configure the File System provider for the `TestContainer1`**

````csharp
Configure<AbpBlobStoringOptions>(options =>
{
    options.Containers.Configure<TestContainer1>(configuration =>
    {
        configuration.UseFileSystem(fileSystem =>
        {
            fileSystem.BasePath = "C:\\MyStorageFolder";
        });
    });
});
````

See the [blob storing documentation](https://docs.abp.io/en/abp/latest/Blob-Storing) for more information.

### Oracle Integration Package for Entity Framework Core