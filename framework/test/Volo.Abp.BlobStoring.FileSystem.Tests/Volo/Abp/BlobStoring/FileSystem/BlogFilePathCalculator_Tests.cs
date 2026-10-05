using System;
using System.IO;
using System.Runtime.InteropServices;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.BlobStoring.FileSystem;

public class BlogFilePathCalculator_Tests : AbpBlobStoringFileSystemTestBase
{
    private readonly IBlobFilePathCalculator _calculator;
    private readonly ICurrentTenant _currentTenant;

    public BlogFilePathCalculator_Tests()
    {
        _calculator = GetRequiredService<IBlobFilePathCalculator>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    private static readonly string BasePath = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "my-files"));

    [Fact]
    public void Default_Settings()
    {
        _calculator.Calculate(
            GetArgs(BasePath, "my-container", "my-blob")
        ).ShouldBe(Path.Combine(BasePath, "host", "my-container", "my-blob"));
    }

    [Fact]
    public void Default_Settings_With_TenantId()
    {
        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            _calculator.Calculate(
                GetArgs(BasePath, "my-container", "my-blob")
            ).ShouldBe(Path.Combine(BasePath, "tenants", tenantId.ToString("D"), "my-container", "my-blob"));
        }
    }

    [Fact]
    public void AppendContainerNameToBasePath_Set_To_False()
    {
        _calculator.Calculate(
            GetArgs(BasePath, "my-container", "my-blob", appendContainerNameToBasePath: false)
        ).ShouldBe(Path.Combine(BasePath, "host", "my-blob"));
    }

    [Fact]
    public void Relative_BasePath()
    {
        _calculator.Calculate(
            GetArgs("my-files", "my-container", "my-blob")
        ).ShouldBe(Path.GetFullPath(Path.Combine("my-files", "host", "my-container", "my-blob")));
    }

    [Theory]
    [InlineData("my-folder/my-blob")]
    [InlineData("my-folder/my-sub-folder/my-blob")]
    [InlineData("my..blob")]
    [InlineData("..my-blob")]
    public void Should_Allow_BlobName_Inside_Container(string blobName)
    {
        _calculator.Calculate(
            GetArgs(BasePath, "my-container", blobName)
        ).ShouldBe(Path.GetFullPath(Path.Combine(BasePath, "host", "my-container", blobName)));
    }

    [Theory]
    [InlineData("../my-blob")]
    [InlineData("../../my-blob")]
    [InlineData("../../../my-blob")]
    [InlineData("my-folder/../../my-blob")]
    [InlineData("my-folder/../my-blob")]
    [InlineData("./my-blob")]
    [InlineData(".")]
    [InlineData("./")]
    [InlineData("my-folder/..")]
    public void Should_Throw_If_BlobName_Contains_Navigation_Segments(string blobName)
    {
        Should.Throw<ArgumentException>(() => _calculator.Calculate(GetArgs(BasePath, "my-container", blobName)));
        Should.Throw<ArgumentException>(() => _calculator.Calculate(GetArgs(BasePath, "my-container", blobName.Replace('/', Path.DirectorySeparatorChar))));
    }

    [Fact]
    public void Should_Throw_If_BlobName_Is_Rooted()
    {
        Should.Throw<ArgumentException>(() => _calculator.Calculate(GetArgs(BasePath, "my-container", Path.Combine(BasePath, "host", "my-container", "my-blob"))));
        Should.Throw<ArgumentException>(() => _calculator.Calculate(GetArgs(BasePath, "my-container", Path.Combine(Path.GetTempPath(), "my-blob"))));
        Should.Throw<ArgumentException>(() => _calculator.Calculate(GetArgs(BasePath, "my-container", Path.DirectorySeparatorChar + "my-blob")));
    }

    [Theory]
    [InlineData("C:my-blob")]
    [InlineData("\\my-blob")]
    [InlineData("\\\\server\\share\\my-blob")]
    [InlineData("\\\\?\\C:\\my-blob")]
    public void Should_Throw_If_BlobName_Is_Rooted_On_Windows(string blobName)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }

        Should.Throw<ArgumentException>(() => _calculator.Calculate(GetArgs(BasePath, "my-container", blobName)));
    }

    [Fact]
    public void Should_Throw_If_BlobName_Points_To_Another_Tenant()
    {
        var tenantId = Guid.NewGuid();
        var otherTenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            Should.Throw<ArgumentException>(() => _calculator.Calculate(
                GetArgs(BasePath, "my-container", $"../../{otherTenantId:D}/my-container/my-blob")));

            Should.Throw<ArgumentException>(() => _calculator.Calculate(
                GetArgs(BasePath, "my-container", $"../{otherTenantId:D}/my-blob", appendContainerNameToBasePath: false)));
        }
    }

    [Theory]
    [InlineData("../my-container")]
    [InlineData("../../tenants/my-container")]
    [InlineData("my-folder/../my-container")]
    [InlineData(".")]
    public void Should_Throw_If_ContainerName_Contains_Navigation_Segments(string containerName)
    {
        Should.Throw<ArgumentException>(() => _calculator.Calculate(GetArgs(BasePath, containerName, "my-blob")));
    }

    [Fact]
    public void Should_Ignore_ContainerName_If_AppendContainerNameToBasePath_Set_To_False()
    {
        _calculator.Calculate(
            GetArgs(BasePath, "../my-container", "my-blob", appendContainerNameToBasePath: false)
        ).ShouldBe(Path.Combine(BasePath, "host", "my-blob"));
    }

    private static BlobProviderArgs GetArgs(
        string basePath,
        string containerName,
        string blobName,
        bool? appendContainerNameToBasePath = null)
    {
        return new BlobProviderGetArgs(
            containerName,
            new BlobContainerConfiguration()
                .UseFileSystem(fs =>
                {
                    fs.BasePath = basePath;
                    if (appendContainerNameToBasePath.HasValue)
                    {
                        fs.AppendContainerNameToBasePath = appendContainerNameToBasePath.Value;
                    }
                }),
            blobName
        );
    }
}
