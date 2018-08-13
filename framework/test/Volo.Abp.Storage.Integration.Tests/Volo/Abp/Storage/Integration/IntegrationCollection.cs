using Xunit;

namespace Volo.Abp.Storage.Integration
{
    [CollectionDefinition(nameof(IntegrationCollection))]
    public class IntegrationCollection: ICollectionFixture<AbpStoresTestFixture>
    {
    }
}
