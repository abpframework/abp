using Shouldly;
using System;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.BlobStoring.Aliyun;

public class AliyunBlobNameCalculator_Tests : AbpBlobStoringAliyunTestCommonBase
{
    private readonly IAliyunBlobNameCalculator _calculator;
    private readonly ICurrentTenant _currentTenant;

    private const string AliyunSeparator = "/";

    public AliyunBlobNameCalculator_Tests()
    {
        _calculator = GetRequiredService<IAliyunBlobNameCalculator>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public void Default_Settings()
    {
        _calculator.Calculate(
            GetArgs("my-container", "my-blob")
        ).ShouldBe($"host{AliyunSeparator}my-blob");
    }

    [Fact]
    public void Default_Settings_With_TenantId()
    {
        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            _calculator.Calculate(
                GetArgs("my-container", "my-blob")
            ).ShouldBe($"tenants{AliyunSeparator}{tenantId:D}{AliyunSeparator}my-blob");
        }
    }

    private static BlobProviderArgs GetArgs(
        string containerName,
        string blobName)
    {
        return new BlobProviderGetArgs(
            containerName,
            new BlobContainerConfiguration().UseAliyun(x =>
            {
            }),
            blobName
        );
    }
}
