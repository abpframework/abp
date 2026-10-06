using System;
using System.IO;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.BlobStoring.TestObjects;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.BlobStoring.FileSystem;

public class FileSystemBlobContainer_Path_Tests : AbpBlobStoringFileSystemTestBase
{
    private readonly IBlobContainer<TestContainer1> _container;
    private readonly ICurrentTenant _currentTenant;
    private readonly string _basePath;

    public FileSystemBlobContainer_Path_Tests()
    {
        _container = GetRequiredService<IBlobContainer<TestContainer1>>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
        _basePath = GetRequiredService<IBlobContainerConfigurationProvider>()
            .Get<TestContainer1>()
            .GetFileSystemConfiguration()
            .BasePath;
    }

    [Fact]
    public async Task Should_Not_Access_Files_Outside_Container()
    {
        var outsideFilePath = Path.Combine(_basePath, "host", "outside.txt");
        Directory.CreateDirectory(Path.GetDirectoryName(outsideFilePath)!);
        File.WriteAllText(outsideFilePath, "outside");

        const string blobName = "../outside.txt";

        await Should.ThrowAsync<ArgumentException>(() => _container.SaveAsync(blobName, "changed".GetBytes(), overrideExisting: true));
        await Should.ThrowAsync<ArgumentException>(() => _container.GetAllBytesOrNullAsync(blobName));
        await Should.ThrowAsync<ArgumentException>(() => _container.ExistsAsync(blobName));
        await Should.ThrowAsync<ArgumentException>(() => _container.DeleteAsync(blobName));

        File.Exists(outsideFilePath).ShouldBeTrue();
        File.ReadAllText(outsideFilePath).ShouldBe("outside");
    }

    [Fact]
    public async Task Should_Not_Access_Blobs_Of_Another_Tenant()
    {
        var tenantId = Guid.NewGuid();
        var otherTenantId = Guid.NewGuid();
        var containerName = BlobContainerNameAttribute.GetContainerName<TestContainer1>();

        using (_currentTenant.Change(otherTenantId))
        {
            await _container.SaveAsync("my-blob", "other tenant".GetBytes());
        }

        using (_currentTenant.Change(tenantId))
        {
            var blobName = $"../../{otherTenantId:D}/{containerName}/my-blob";

            await Should.ThrowAsync<ArgumentException>(() => _container.GetAllBytesOrNullAsync(blobName));
            await Should.ThrowAsync<ArgumentException>(() => _container.DeleteAsync(blobName));
        }

        using (_currentTenant.Change(otherTenantId))
        {
            (await _container.GetAllBytesAsync("my-blob")).ShouldBe("other tenant".GetBytes());
        }
    }
}
