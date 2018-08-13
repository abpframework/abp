using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.Storage.Integration
{
    [Collection(nameof(IntegrationCollection))]
    [Trait("Operation", "List"), Trait("Kind", "Integration")]
    public class ListTests
    {
        private AbpStoresTestFixture _storeFixture;

        public ListTests(AbpStoresTestFixture storeFixture)
        {
            _storeFixture = storeFixture;
        }

        [Theory(DisplayName = nameof(ListRootFiles)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ListRootFiles(string storeName)
        {
            var storageFactory = _storeFixture.Services.GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expected = new[] { "TextFile.txt", "template.hbs" };

            var results = await store.ListBlobAsync(null);

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            Assert.Empty(missingFiles);
            Assert.Empty(unexpectedFiles);
        }

        [Theory(DisplayName = nameof(ListEmptyPathFiles)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ListEmptyPathFiles(string storeName)
        {
            var storageFactory = _storeFixture.Services.GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expected = new[] { "TextFile.txt", "template.hbs" };

            var results = await store.ListBlobAsync("");

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            Assert.Empty(missingFiles);
            Assert.Empty(unexpectedFiles);
        }

        [Theory(DisplayName = nameof(ListSubDirectoryFiles)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ListSubDirectoryFiles(string storeName)
        {
            var storageFactory = _storeFixture.Services.GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expected = new[] { "SubDirectory/TextFile2.txt" };

            var results = await store.ListBlobAsync("SubDirectory");

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            Assert.Empty(missingFiles);
            Assert.Empty(unexpectedFiles);
        }

        [Theory(DisplayName = nameof(ListSubDirectoryFilesWithTrailingSlash)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ListSubDirectoryFilesWithTrailingSlash(string storeName)
        {
            var storageFactory = _storeFixture.Services.GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expected = new[] { "SubDirectory/TextFile2.txt" };

            var results = await store.ListBlobAsync("SubDirectory/");

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            Assert.Empty(missingFiles);
            Assert.Empty(unexpectedFiles);
        }

        [Theory(DisplayName = nameof(ExtensionGlobbing)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task ExtensionGlobbing(string storeName)
        {
            var storageFactory = _storeFixture.Services.GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expected = new[] { "Globbing/template.hbs", "Globbing/template-header.hbs" };

            var results = await store.ListBlobAsync("Globbing", "*.hbs");

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            Assert.Empty(missingFiles);
            Assert.Empty(unexpectedFiles);
        }

        [Theory(DisplayName = nameof(FileNameGlobbing)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task FileNameGlobbing(string storeName)
        {
            var storageFactory = _storeFixture.Services.GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expected = new[] { "Globbing/template.hbs", "Globbing/template.mustache" };

            var results = await store.ListBlobAsync("Globbing", "template.*");

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            Assert.Empty(missingFiles);
            Assert.Empty(unexpectedFiles);
        }

        [Theory(DisplayName = nameof(FileNameGlobbingAtRoot)), InlineData("Store1"), InlineData("Store2"), InlineData("Store3"), InlineData("Store4"), InlineData("Store5"), InlineData("Store6")]
        public async Task FileNameGlobbingAtRoot(string storeName)
        {
            var storageFactory = _storeFixture.Services.GetRequiredService<IAbpStorageFactory>();

            var store = storageFactory.GetStore(storeName);

            var expected = new[] { "template.hbs" };

            var results = await store.ListBlobAsync("", "template.*");

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            Assert.Empty(missingFiles);
            Assert.Empty(unexpectedFiles);
        }
    }
}
