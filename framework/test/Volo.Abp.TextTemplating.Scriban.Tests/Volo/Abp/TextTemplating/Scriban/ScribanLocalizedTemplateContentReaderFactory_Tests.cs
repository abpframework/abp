using System.IO;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.TextTemplating.VirtualFiles;

namespace Volo.Abp.TextTemplating.Scriban
{
    public class ScribanLocalizedTemplateContentReaderFactory_Tests : LocalizedTemplateContentReaderFactory_Tests<ScribanTextTemplatingTestModule>
    {
        public ScribanLocalizedTemplateContentReaderFactory_Tests()
        {
            LocalizedTemplateContentReaderFactory = new LocalizedTemplateContentReaderFactory(
                new PhysicalFileVirtualFileProvider(
                    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),
                        "Volo", "Abp", "TextTemplating", "Scriban"))));

            WelcomeEmailEnglishContent = "Welcome {{model.name}} to the abp.io!";
            WelcomeEmailTurkishContent = "Merhaba {{model.name}}, abp.io'ya hoşgeldiniz!";
        }
    }
}
