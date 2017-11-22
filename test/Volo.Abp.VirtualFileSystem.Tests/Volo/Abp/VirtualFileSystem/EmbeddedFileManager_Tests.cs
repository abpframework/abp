using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.TestBase;
using Volo.Abp.VirtualFileSystem.Embedded;
using Xunit;

namespace Volo.Abp.VirtualFileSystem
{
    public class EmbeddedFileManager_Tests : AbpIntegratedTest<EmbeddedFileManager_Tests.TestModule>
    {
        private readonly IVirtualFileProvider _embeddedFileManager;

        public EmbeddedFileManager_Tests()
        {
            _embeddedFileManager = ServiceProvider.GetRequiredService<IVirtualFileProvider>();
        }

        [Fact]
        public void Should_Define_And_Get_Embedded_Resources()
        {
            //Act
            var resource = _embeddedFileManager.GetFileInfo("/js/jquery-3.1.1.min.js");

            //Assert
            resource.ShouldNotBeNull();
            resource.Exists.ShouldBeTrue();

            using (var stream = resource.CreateReadStream())
            {
                Encoding.UTF8.GetString(stream.GetAllBytes()).ShouldBe("//jquery-3.1.1.min.js-contents");
            }
        }

        [DependsOn(typeof(AbpVirtualFileSystemModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(IServiceCollection services)
            {
                services.Configure<VirtualFileSystemOptions>(options =>
                {
                    options.FileSets.Add(
                        new EmbeddedFileSet(
                            GetType().Assembly,
                            "Volo.Abp.VirtualFileSystem.MyResources"
                        )
                    );
                });
            }
        }
    }
}
