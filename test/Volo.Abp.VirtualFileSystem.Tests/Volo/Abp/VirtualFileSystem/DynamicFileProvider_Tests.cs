using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.VirtualFileSystem
{
    public class DynamicFileProvider_Tests : AbpIntegratedTest<DynamicFileProvider_Tests.TestModule>
    {
        private readonly IDynamicFileProvider _dynamicFileProvider;

        public DynamicFileProvider_Tests()
        {
            _dynamicFileProvider = GetRequiredService<IDynamicFileProvider>();
        }

        [Fact]
        public void Should_Get_Created_Files()
        {
            const string fileContent = "Hello World";

            _dynamicFileProvider.AddOrUpdate(
                new InMemoryFileInfo(
                    fileContent.GetBytes(),
                    "/my-files/test.txt",
                    "test.txt"
                )
            );

            var fileInfo = _dynamicFileProvider.GetFileInfo("/my-files/test.txt");
            fileInfo.ShouldNotBeNull();
            fileInfo.ReadAsString().ShouldBe(fileContent);
        }

        [DependsOn(typeof(AbpVirtualFileSystemModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(IServiceCollection services)
            {
                
            }
        }
    }
}
