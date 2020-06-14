using System;
using Shouldly;
using Volo.Abp.MultiTenancy;
using Xunit;

namespace Volo.Abp.BlobStoring.Azure
{
    public class AzureBlobNameCalculator_Tests : AbpBlobStoringAzureTestCommonBase
    {
        private readonly IAzureBlobNameCalculator _calculator;
        private readonly ICurrentTenant _currentTenant;

        private const string AzureContainerName = "/";
        private const string AzureSeparator = "/";

        public AzureBlobNameCalculator_Tests()
        {
            _calculator = GetRequiredService<IAzureBlobNameCalculator>();
            _currentTenant = GetRequiredService<ICurrentTenant>();
        }

        [Fact]
        public void Default_Settings()
        {
            _calculator.Calculate(
                GetArgs("my-container", "my-blob")
            ).ShouldBe($"host{AzureSeparator}my-blob");
        }

        [Fact]
        public void Default_Settings_With_TenantId()
        {
            var tenantId = Guid.NewGuid();

            using (_currentTenant.Change(tenantId))
            {
                _calculator.Calculate(
                    GetArgs("my-container", "my-blob")
                ).ShouldBe($"tenants{AzureSeparator}{tenantId:D}{AzureSeparator}my-blob");
            }
        }

        private static BlobProviderArgs GetArgs(
            string containerName,
            string blobName)
        {
            return new BlobProviderGetArgs(
                containerName,
                new BlobContainerConfiguration().UseAzure(x =>
                {
                    x.ContainerName = containerName;
                }),
                blobName
            );
        }
    }
}
