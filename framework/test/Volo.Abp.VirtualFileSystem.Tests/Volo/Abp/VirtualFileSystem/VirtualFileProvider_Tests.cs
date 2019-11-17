using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.VirtualFileSystem
{
    public class VirtualFileProvider_Tests : AbpIntegratedTest<VirtualFileProvider_Tests.TestModule>
    {
        private readonly IVirtualFileProvider _virtualFileProvider;

        public VirtualFileProvider_Tests()
        {
            _virtualFileProvider = ServiceProvider.GetRequiredService<IVirtualFileProvider>();
        }

        [Fact]
        public void Should_Define_And_Get_Embedded_Resources()
        {
            //Act
            var resource = _virtualFileProvider.GetFileInfo("/js/jquery-3-1-1-min.js");

            //Assert
            resource.ShouldNotBeNull();
            resource.Exists.ShouldBeTrue();

            using (var stream = resource.CreateReadStream())
            {
                Encoding.UTF8.GetString(stream.GetAllBytes()).ShouldBe("//jquery-3-1-1-min.js-contents");
            }
        }

        [Fact]
        public void Should_Define_And_Get_Embedded_Directory_Contents()
        {
            //Act
            var contents = _virtualFileProvider.GetDirectoryContents("/js");

            //Assert
            contents.Exists.ShouldNotBeNull();

            var contentList = contents.ToList();
            contentList.ShouldContain(x => x.Name == "jquery-3-1-1-min.js");
        }

        [DependsOn(typeof(AbpVirtualFileSystemModule))]
        public class TestModule : AbpModule
        {
            public override void ConfigureServices(ServiceConfigurationContext context)
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.AddEmbedded<TestModule>("Volo.Abp.VirtualFileSystem.MyResources");
                });
            }
        }
    }
}
