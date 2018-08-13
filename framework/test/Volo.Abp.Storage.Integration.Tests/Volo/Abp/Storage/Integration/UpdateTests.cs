using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.Storage.Integration
{
    [Collection(nameof(IntegrationCollection))]
    [Trait("Operation", "Update"), Trait("Kind", "Integration")]
    public class UpdateTests : AbpStoresTestBase
    {
        [Theory(DisplayName = nameof(WriteAllText)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task WriteAllText(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);
            var textToWrite = "The answer is 42";
            var filePath = "Update/42.txt";

            await store.SaveBlobAsync(Encoding.UTF8.GetBytes(textToWrite), filePath, "text/plain");

            string readFromWrittenFile = await store.ReadBlobTextAsync(filePath);

            Assert.Equal(textToWrite, readFromWrittenFile);
        }

        [Theory(DisplayName = nameof(ETagShouldBeTheSameWithSameContent)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ETagShouldBeTheSameWithSameContent(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);
            var textToWrite = "ETag Test Compute";
            var filePath = "Update/etag-same.txt";

            var savedReference = await store.SaveBlobAsync(Encoding.UTF8.GetBytes(textToWrite), filePath, "text/plain");
            var readReference = await store.GetBlobAsync(filePath, withMetadata: true);

            Assert.Equal(savedReference.BlobDescriptor.ETag, readReference.BlobDescriptor.ETag);
        }

        [Theory(DisplayName = nameof(ETagShouldBeDifferentWithDifferentContent)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ETagShouldBeDifferentWithDifferentContent(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);
            var textToWrite = "ETag Test Compute";
            var filePath = "Update/etag-different.txt";
            var textToUpdate = "ETag Test Compute 2";

            var savedReference = await store.SaveBlobAsync(Encoding.UTF8.GetBytes(textToWrite), filePath, "text/plain");
            var updatedReference = await store.SaveBlobAsync(Encoding.UTF8.GetBytes(textToUpdate), filePath, "text/plain");

            Assert.NotEqual(savedReference.BlobDescriptor.ETag, updatedReference.BlobDescriptor.ETag);
        }

        [Theory(DisplayName = nameof(SaveStream)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task SaveStream(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);
            var textToWrite = "The answer is 42";
            var filePath = "Update/42-2.txt";

            await store.SaveBlobAsync(new MemoryStream(Encoding.UTF8.GetBytes(textToWrite)), filePath, "text/plain");

            var readFromWrittenFile = await store.ReadBlobTextAsync(filePath);

            Assert.Equal(textToWrite, readFromWrittenFile);
        }

        [Theory(DisplayName = nameof(AddMetatadaRoundtrip)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task AddMetatadaRoundtrip(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var testFile = "Metadata/TextFile.txt";

            var file = await store.GetBlobAsync(testFile, withMetadata: true);

            var id = Guid.NewGuid().ToString();

            file.BlobDescriptor.Metadata.Add("newid", id);

            await file.SaveBlobDescriptorAsync();

            file = await store.GetBlobAsync(testFile, withMetadata: true);

            var actualId = file.BlobDescriptor.Metadata["newid"];

            Assert.Equal(id, actualId);
        }

        [Theory(DisplayName = nameof(SaveMetatadaRoundtrip)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task SaveMetatadaRoundtrip(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var testFile = "Metadata/TextFile.txt";

            var file = await store.GetBlobAsync(testFile, withMetadata: true);

            var id = Guid.NewGuid().ToString();

            file.BlobDescriptor.Metadata["id"] = id;

            await file.SaveBlobDescriptorAsync();

            file = await store.GetBlobAsync(testFile, withMetadata: true);

            var actualId = file.BlobDescriptor.Metadata["id"];

            Assert.Equal(id, actualId);
        }

        [Theory(DisplayName = nameof(SaveEncodedMetatadaRoundtrip)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task SaveEncodedMetatadaRoundtrip(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var testFile = "Metadata/TextFile.txt";

            var file = await store.GetBlobAsync(testFile, withMetadata: true);

            var name = "ï";

            file.BlobDescriptor.Metadata["name"] = name;

            await file.SaveBlobDescriptorAsync();

            file = await store.GetBlobAsync(testFile, withMetadata: true);

            var actualName = file.BlobDescriptor.Metadata["name"];

            Assert.Equal(name, actualName);
        }

        [Theory(DisplayName = nameof(ListMetatadaRoundtrip)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ListMetatadaRoundtrip(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var testFile = "Metadata/TextFile.txt";

            var file = await store.GetBlobAsync(testFile, withMetadata: true);

            var id = Guid.NewGuid().ToString();

            file.BlobDescriptor.Metadata["id"] = id;

            await file.SaveBlobDescriptorAsync();

            var files = await store.ListBlobAsync("Metadata", withMetadata: true);

            string actualId = null;

            foreach (var aFile in files)
            {
                if (aFile.Path == testFile)
                {
                    actualId = aFile.BlobDescriptor.Metadata["id"];
                }
            }

            Assert.Equal(id, actualId);
        }
    }
}
