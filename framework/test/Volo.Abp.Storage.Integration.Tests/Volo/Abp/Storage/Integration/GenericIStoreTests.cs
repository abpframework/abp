using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.Storage.Integration
{
    [Collection(nameof(IntegrationCollection))]
    [Trait("Operation", "GenericIStore"), Trait("Kind", "Integration")]
    public class GenericIStoreTests : AbpIntegratedTest<AbpStorageTestModule>
    {
        [Fact]
        public async Task GenericListRootFiles()
        {
            var store = GetRequiredService<IAbpStore<TestStore>>();

            var expected = new[] { "TextFile.txt", "template.hbs" };

            var results = await store.ListBlobAsync(null);

            var missingFiles = expected.Except(results.Select(f => f.Path)).ToArray();

            var unexpectedFiles = results.Select(f => f.Path).Except(expected).ToArray();

            Assert.Empty(missingFiles);
            Assert.Empty(unexpectedFiles);
        }
    }
}
