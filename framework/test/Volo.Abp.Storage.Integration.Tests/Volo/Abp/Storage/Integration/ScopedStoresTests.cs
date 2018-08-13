using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.Storage.Integration
{
    [Collection(nameof(IntegrationCollection))]
    [Trait("Operation", "ScopedStores"), Trait("Kind", "Integration")]
    public class ScopedStoresTests
    {
        private AbpStoresTestFixture _storeFixture;

        public ScopedStoresTests(AbpStoresTestFixture storeFixture)
        {
            _storeFixture = storeFixture;
        }

        [Theory(DisplayName = nameof(ScopedStoreUpdate)), InlineData("ScopedStore1"), InlineData("ScopedStore2")]
        public async Task ScopedStoreUpdate(string storeName)
        {
            var storageFactory = _storeFixture.Services.GetRequiredService<IAbpStorageFactory>();

            var formatArg = Guid.NewGuid();
            var store = storageFactory.GetScopedStore(storeName, formatArg);

            await store.InitAsync();

            var textToWrite = "The answer is 42";
            var filePath = "Update/42.txt";

            await store.SaveBlobAsync(Encoding.UTF8.GetBytes(textToWrite), filePath, "text/plain");

            var readFromWrittenFile = await store.ReadBlobTextAsync(filePath);

            Assert.Equal(textToWrite, readFromWrittenFile);
        }
    }
}
