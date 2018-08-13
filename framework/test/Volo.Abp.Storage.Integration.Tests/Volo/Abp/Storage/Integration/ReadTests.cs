using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.Storage.Integration
{
    [Collection(nameof(IntegrationCollection))]
    [Trait("Operation", "Read"), Trait("Kind", "Integration")]
    public class ReadTests : AbpStoresTestBase
    {
        [Theory(DisplayName = nameof(ReadAllTextFromRootFile)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ReadAllTextFromRootFile(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expectedText = "42";

            var actualText = await store.ReadBlobTextAsync("TextFile.txt");

            Assert.Equal(expectedText, actualText);
        }

        [Theory(DisplayName = nameof(ReadAllTextFromRootFile)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ReadAllTextFromSubdirectoryFile(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expectedText = ">42";

            var actualText = await store.ReadBlobTextAsync("SubDirectory/TextFile2.txt");

            Assert.Equal(expectedText, actualText);
        }

        [Theory(DisplayName = nameof(ReadAllBytesFromSubdirectoryFile)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ReadAllBytesFromSubdirectoryFile(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expectedText = ">42";

            using (var reader = new StreamReader(new MemoryStream(await store.ReadBlobBytesAsync("SubDirectory/TextFile2.txt"))))
            {
                var actualText = reader.ReadToEnd();
                Assert.Equal(expectedText, actualText);
            }
        }

        [Theory(DisplayName = nameof(ReadAllBytesFromSubdirectoryFileUsingFileReference)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ReadAllBytesFromSubdirectoryFileUsingFileReference(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expectedText = ">42";

            var file = await store.GetBlobAsync("SubDirectory/TextFile2.txt");

            using (var reader = new StreamReader(new MemoryStream(await file.ReadBlobBytesAsync())))
            {
                var actualText = reader.ReadToEnd();
                Assert.Equal(expectedText, actualText);
            }
        }


        [Theory(DisplayName = nameof(ReadFileFromSubdirectoryFile)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ReadFileFromSubdirectoryFile(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expectedText = ">42";

            var file = await store.GetBlobAsync("SubDirectory/TextFile2.txt");

            string actualText;

            using (var reader = new StreamReader(await file.ReadBlobAsync()))
            {
                actualText = await reader.ReadToEndAsync();
            }

            Assert.Equal(expectedText, actualText);
        }

        [Theory(DisplayName = nameof(ReadAllTextFromSubdirectoryFileUsingFileReference)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ReadAllTextFromSubdirectoryFileUsingFileReference(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expectedText = ">42";

            var file = await store.GetBlobAsync("SubDirectory/TextFile2.txt");

            string actualText = await file.ReadBlobTextAsync();

            Assert.Equal(expectedText, actualText);
        }


        [Theory(DisplayName = nameof(ListThenReadAllTextFromSubdirectoryFile)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ListThenReadAllTextFromSubdirectoryFile(string storeName)
        {
            var storageFactory = GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expectedText = ">42";

            var files = await store.ListBlobAsync("SubDirectory");

            foreach (var file in files)
            {
                var actualText = await store.ReadBlobTextAsync(file);

                Assert.Equal(expectedText, actualText);
            }
        }
    }
}
