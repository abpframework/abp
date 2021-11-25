using System;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.BlobStoring.Aws;

public class AwsBlobNameCalculatorTests : AbpBlobStoringAwsTestCommonBase
{
    private readonly IAwsBlobNameCalculator _calculator;
    private readonly ICurrentTenant _currentTenant;

    private const string AwsContainerName = "/";
    private const string AwsSeparator = "/";

    public AwsBlobNameCalculatorTests()
    {
        _calculator = GetRequiredService<IAwsBlobNameCalculator>();
        _currentTenant = GetRequiredService<ICurrentTenant>();
    }

    [Fact]
    public void Default_Settings()
    {
        _calculator.Calculate(
            GetArgs("my-container", "my-blob")
        ).ShouldBe($"host{AwsSeparator}my-blob");
    }

    [Fact]
    public void Default_Settings_With_TenantId()
    {
        var tenantId = Guid.NewGuid();

        using (_currentTenant.Change(tenantId))
        {
            _calculator.Calculate(
                GetArgs("my-container", "my-blob")
            ).ShouldBe($"tenants{AwsSeparator}{tenantId:D}{AwsSeparator}my-blob");
        }
    }

    private static BlobProviderArgs GetArgs(
        string containerName,
        string blobName)
    {
        return new BlobProviderGetArgs(
            containerName,
            new BlobContainerConfiguration().UseAws(x =>
            {
                x.ContainerName = containerName;
            }),
            blobName
        );
    }
}
