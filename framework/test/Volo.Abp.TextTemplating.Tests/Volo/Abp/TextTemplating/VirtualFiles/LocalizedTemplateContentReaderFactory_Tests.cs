using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Shouldly;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace Volo.Abp.TextTemplating.VirtualFiles
{
    public class LocalizedTemplateContentReaderFactory_Tests: AbpTextTemplatingTestBase
    {
        private readonly ITemplateDefinitionManager _templateDefinitionManager;

        public LocalizedTemplateContentReaderFactory_Tests()
        {
            _templateDefinitionManager = GetRequiredService<ITemplateDefinitionManager>();
        }

        [Fact]
        public async Task Create_Should_Work_With_PhysicalFileProvider()
        {
            var localizedTemplateContentReaderFactory = new LocalizedTemplateContentReaderFactory(
                new PhysicalFileVirtualFileProvider(
                    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),
                        "Volo", "Abp", "TextTemplating"))));

            var reader = await localizedTemplateContentReaderFactory.CreateAsync(_templateDefinitionManager.Get(TestTemplates.WelcomeEmail));

            reader.GetContentOrNull("en").ShouldBe("Welcome {{model.name}} to the abp.io!");
            reader.GetContentOrNull("tr").ShouldBe("Merhaba {{model.name}}, abp.io'ya hoşgeldiniz!");
        }

        class PhysicalFileVirtualFileProvider : IVirtualFileProvider
        {
            private readonly PhysicalFileProvider _physicalFileProvider;

            public PhysicalFileVirtualFileProvider(PhysicalFileProvider physicalFileProvider)
            {
                _physicalFileProvider = physicalFileProvider;
            }

            public IFileInfo GetFileInfo(string subpath)
            {
                return _physicalFileProvider.GetFileInfo(subpath);
            }

            public IDirectoryContents GetDirectoryContents(string subpath)
            {
                return _physicalFileProvider.GetDirectoryContents(subpath);
            }

            public IChangeToken Watch(string filter)
            {
                return _physicalFileProvider.Watch(filter);
            }
        }
    }
}
