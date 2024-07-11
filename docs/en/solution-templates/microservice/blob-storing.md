# Microservice Solution: BLOB Storing

````json
//[doc-nav]
{
  "Next": {
    "Name": "CORS configuration in the Microservice solution",
    "Path": "solution-templates/microservice/cors-configuration"
  }
}
````

> You must have an ABP Business or a higher license to be able to create a microservice solution.

This document explains how to store BLOBs (Binary Large Objects) in a microservice solution. It is common to store files, images, videos, and other large objects in a distributed system. You can learn more about BLOB storage in the [BLOB Storing System](../../framework/infrastructure/blob-storing/index.md) documentation.

In the microservice solution template, the [Database Provider](../../framework/infrastructure/blob-storing/database.md) is used to store BLOBs in the database. The `Volo.Abp.BlobStoring.Database.EntityFrameworkCore` or `Volo.Abp.BlobStoring.Database.MongoDB` package provides the necessary implementations to store and retrieve BLOBs in the database. This setup is integrated into the microservice solution template and is used in all related projects. You can change the database configuration in the `appsettings.json` file of the related project. The default configuration is for SQL Server as follows:

```json
"AbpBlobStoring": "Server=localhost,1434; User Id=sa; Password=myPassw@rd; Database=MyProjectName_BlobStoring; TrustServerCertificate=true"
```

Afterwards, you can use the `IBlobContainer` or `IBlobContainer<T>` service to store and retrieve BLOBs. Here is an example of storing a BLOB:

```csharp
public class MyService : ITransientDependency
{
    private readonly IBlobContainer _blobContainer;

    public MyService(IBlobContainer blobContainer)
    {
        _blobContainer = blobContainer;
    }

    public async Task SaveBytesAsync(byte[] bytes)
    {
        await _blobContainer.SaveAsync("my-blob-1", bytes);
    }

    public async Task<byte[]> GetBytesAsync()
    {
        return await _blobContainer.GetAllBytesOrNullAsync("my-blob-1");
    }
}
```

The *File Management* module is optional and can be added to the solution during the creation process. It provides a user interface to manage folders and files. You can learn more about the module in the [File Management](../../modules/file-management.md) document.

![file-management](images/file-management-index-page.png)