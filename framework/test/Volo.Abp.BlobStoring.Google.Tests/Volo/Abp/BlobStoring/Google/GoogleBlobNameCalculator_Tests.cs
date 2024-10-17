using System;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.BlobStoring.Google;

public class GoogleBlobNameCalculator_Tests : AbpBlobStoringGoogleTestCommonBase
{
    private readonly IGoogleBlobNameCalculator _calculator;
    private readonly ICurrentTenant _currentTenant;

    private const string GoogleContainerName = "/";
    private const string GoogleSeparator = "/";

    public GoogleBlobNameCalculator_Tests()
    {
        _calculator = GetRequiredService<IGoogleBlobNameCalculator>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public void Default_Settings()
    {
        _calculator.Calculate(
            GetArgs("my-container", "my-blob")
        ).ShouldBe($"host{GoogleSeparator}my-blob");
    }

    [Fact]
    public void Default_Settings_With_TenantId()
    {
        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            _calculator.Calculate(
                GetArgs("my-container", "my-blob")
            ).ShouldBe($"tenants{GoogleSeparator}{tenantId:D}{GoogleSeparator}my-blob");
        }
    }

    private static BlobProviderArgs GetArgs(
        string containerName,
        string blobName)
    {
        return new BlobProviderGetArgs(
            containerName,
            new BlobContainerConfiguration().UseGoogle(x =>
            {
                x.ContainerName = containerName;
            }),
            blobName
        );
    }
}