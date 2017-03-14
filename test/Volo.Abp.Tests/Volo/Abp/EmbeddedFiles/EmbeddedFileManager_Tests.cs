using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.TestBase;
using Xunit;

namespace Volo.Abp.EmbeddedFiles
{
    public class EmbeddedFileManager_Tests : AbpIntegratedTest<EmbeddedFileManager_Tests.TestModule>
    {
        private readonly IEmbeddedFileManager _embeddedFileManager;

        public EmbeddedFileManager_Tests()
        {
            _embeddedFileManager = ServiceProvider.GetRequiredService<IEmbeddedFileManager>();
        }

        [Fact]
        public void Should_Define_And_Get_Embedded_Resources()
        {
            var resource = _embeddedFileManager.FindFile("/MyApp/MyResources/js/MyScriptFile1.js");

            resource.ShouldNotBeNull();
            Assert.Equal(resource.Assembly, GetType().GetTypeInfo().Assembly);
            Assert.True(resource.Content.Length > 0);
        }

        public class TestModule : AbpModule
        {
            public override void ConfigureServices(IServiceCollection services)
            {
                services.Configure<EmbeddedFileOptions>(options =>
                {
                    options.Sources.Add(
                        new EmbeddedFileSet(
                            "/MyApp/MyResources/",
                            GetType().GetTypeInfo().Assembly,
                            "Volo.Abp.EmbeddedFiles.MyResources"
                        )
                    );
                });
            }
        }
    }
}
