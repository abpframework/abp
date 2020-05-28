using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.BlobStoring.TestObjects;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.BlobStoring
{
    public abstract class BlobContainer_Tests<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected IBlobContainer<TestContainer1> Container { get; }

        protected BlobContainer_Tests()
        {
            Container = GetRequiredService<IBlobContainer<TestContainer1>>();
        }
        
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }

        [Fact]
        public async Task SaveAsync()
        {
            var testContent = "test content".GetBytes();
            
            using (var memoryStream = new MemoryStream(testContent))
            {
                await Container.SaveAsync("test-blob-1", memoryStream);
            }

            using (var stream = await Container.GetAsync("test-blob-1"))
            {
                var result = await stream.GetAllBytesAsync();
                result.SequenceEqual(testContent).ShouldBeTrue();
            }
        }
    }
}