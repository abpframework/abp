using System;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.BlobStoring.HuaweiCloud;

public class HuaweiCloudBlobNameCalculator_Tests : AbpBlobStoringHuaweiCloudTestCommonBase
{
    private readonly IHuaweiCloudBlobNameCalculator _calculator;
    private readonly ICurrentTenant _currentTenant;

    private const string HuaweiCloudSeparator = "/";

    public HuaweiCloudBlobNameCalculator_Tests()
    {
        _calculator = GetRequiredService<IHuaweiCloudBlobNameCalculator>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public void Default_Settings()
    {
        _calculator.Calculate(
            GetArgs("my-container", "my-blob")
        ).ShouldBe($"host{HuaweiCloudSeparator}my-blob");
    }

    [Fact]
    public void Default_Settings_With_TenantId()
    {
        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            _calculator.Calculate(
                GetArgs("my-container", "my-blob")
            ).ShouldBe($"tenants{HuaweiCloudSeparator}{tenantId:D}{HuaweiCloudSeparator}my-blob");
        }
    }

    private static BlobProviderArgs GetArgs(
        string containerName,
        string blobName)
    {
        return new BlobProviderGetArgs(
            containerName,
            new BlobContainerConfiguration().UseHuaweiCloud(x =>
            {
                x.BucketName = containerName;
            }),
            blobName
        );
    }
}
