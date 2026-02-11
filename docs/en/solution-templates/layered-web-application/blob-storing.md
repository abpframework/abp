```json
//[doc-seo]
{
    "Description": "Learn how to effectively store BLOBs in a layered solution using ABP Framework, enhancing your application's file management capabilities."
}
```

# Layered Solution: BLOB Storing

```json
//[doc-nav]
{
  "Previous": {
    "Name": "Multi-Tenancy",
    "Path": "solution-templates/layered-web-application/multi-tenancy"
  },
  "Next": {
    "Name": "CORS Configuration",
    "Path": "solution-templates/layered-web-application/cors-configuration"
  }
}
```

> Some of the features mentioned in this document may not be available in the free version. We're using the **\*** symbol to indicate that a feature is available in the **[Team](https://abp.io/pricing)** and **[Higher](https://abp.io/pricing)** licenses.

This document explains how to store BLOBs (Binary Large Objects) in a layered solution. It is common to store files, images, videos, and other large objects in a distributed system. You can learn more about BLOB storage in the [BLOB Storing System](../../framework/infrastructure/blob-storing/index.md) documentation.

In the layered solution template, the [Database Provider](../../framework/infrastructure/blob-storing/database.md) is used to store BLOBs in the database. The `Volo.Abp.BlobStoring.Database.EntityFrameworkCore` or `Volo.Abp.BlobStoring.Database.MongoDB` package provides the necessary implementations to store and retrieve BLOBs in the database. This setup is integrated into the layered solution template and is used in all related projects. You can change the database configuration in the `appsettings.json` file of the related project.

You can use the `IBlobContainer` or `IBlobContainer<T>` service to store and retrieve BLOBs. Here is an example of storing a BLOB:

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

## File Management Module

The *File Management* module is optional and can be added to the solution during the creation process. It provides a user interface to manage folders and files. You can learn more about the module in the [File Management *](../../modules/file-management.md) document.

![file-management](images/file-management-index-page.png)
