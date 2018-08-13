using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Volo.Abp.Storage.Azure.Configuration;
using Xunit;

namespace Volo.Abp.Storage.Integration
{
    [Collection(nameof(IntegrationCollection))]
    [Trait("Operation", "SharedAccess"), Trait("Kind", "Integration")]
    public class SharedAccessTests
    {
        private readonly StoresFixture _storeFixture;

        public SharedAccessTests(StoresFixture fixture)
        {
            _storeFixture = fixture;
        }

        [Theory(DisplayName = nameof(StoreSharedAccess)), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task StoreSharedAccess(string storeName)
        {
            var storageFactory = _storeFixture.Services.GetRequiredService<IAbpStorageFactory>();
            var options = _storeFixture.Services.GetRequiredService<IOptions<AbpAzureParsedOptions>>();

            var store = storageFactory.GetStore(storeName);

            options.Value.ParsedStores.TryGetValue(storeName, out var storeOptions);

            var sharedAccessSignature = await store.GetBlobSasUrlAsync(new SharedAccessPolicy
            {
                ExpiryTime = DateTime.UtcNow.AddHours(24),
                Permissions = SharedAccessPermissions.List,
            });

            var account = CloudStorageAccount.Parse(storeOptions.ConnectionString);            

            var accountSAS = new StorageCredentials(sharedAccessSignature);
            var accountWithSAS = new CloudStorageAccount(accountSAS, account.Credentials.AccountName, endpointSuffix: null, useHttps: true);
            var blobClientWithSAS = accountWithSAS.CreateCloudBlobClient();
            var containerWithSAS = blobClientWithSAS.GetContainerReference(storeOptions.ContainerName);

            BlobContinuationToken continuationToken = null;
            var results = new List<IListBlobItem>();

            do
            {
                var response = await containerWithSAS.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                results.AddRange(response.Results);
            }
            while (continuationToken != null);

            var filesFromStore = await store.ListBlobAsync(null, false, false);

            Assert.Equal(filesFromStore.Length, results.OfType<ICloudBlob>().Count());
        }
    }
}
