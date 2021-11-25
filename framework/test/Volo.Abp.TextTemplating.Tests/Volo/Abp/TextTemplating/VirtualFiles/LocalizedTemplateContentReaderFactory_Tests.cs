using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Shouldly;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Xunit;

namespace Volo.Abp.TextTemplating.VirtualFiles;

public abstract class LocalizedTemplateContentReaderFactory_Tests<TStartupModule> : AbpTextTemplatingTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected readonly ITemplateDefinitionManager TemplateDefinitionManager;
    protected LocalizedTemplateContentReaderFactory LocalizedTemplateContentReaderFactory;
    protected string WelcomeEmailEnglishContent;
    protected string WelcomeEmailTurkishContent;

    protected LocalizedTemplateContentReaderFactory_Tests()
    {
        TemplateDefinitionManager = GetRequiredService<ITemplateDefinitionManager>();
    }

    [Fact]
    public async Task Create_Should_Work_With_PhysicalFileProvider()
    {
        var reader = await LocalizedTemplateContentReaderFactory.CreateAsync(TemplateDefinitionManager.Get(TestTemplates.WelcomeEmail));

        reader.GetContentOrNull("en").ShouldBe(WelcomeEmailEnglishContent);
        reader.GetContentOrNull("tr").ShouldBe(WelcomeEmailTurkishContent);
    }

    public class PhysicalFileVirtualFileProvider : IVirtualFileProvider
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
