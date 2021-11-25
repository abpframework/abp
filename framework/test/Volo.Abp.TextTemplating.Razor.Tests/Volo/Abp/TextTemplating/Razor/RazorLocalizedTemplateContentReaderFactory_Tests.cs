using System;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.TextTemplating.VirtualFiles;

namespace Volo.Abp.TextTemplating.Razor;

public class RazorLocalizedTemplateContentReaderFactory_Tests : LocalizedTemplateContentReaderFactory_Tests<RazorTextTemplatingTestModule>
{
    public RazorLocalizedTemplateContentReaderFactory_Tests()
    {
        LocalizedTemplateContentReaderFactory = new LocalizedTemplateContentReaderFactory(
            new PhysicalFileVirtualFileProvider(
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),
                    "Volo", "Abp", "TextTemplating", "Razor"))));

        WelcomeEmailEnglishContent = "@inherits Volo.Abp.TextTemplating.Razor.RazorTemplatePageBase<Volo.Abp.TextTemplating.Razor.RazorTemplateRendererProvider_Tests.WelcomeEmailModel>" +
                         Environment.NewLine +
                         "Welcome @Model.Name to the abp.io!";

        WelcomeEmailTurkishContent = "@inherits Volo.Abp.TextTemplating.Razor.RazorTemplatePageBase<Volo.Abp.TextTemplating.Razor.RazorTemplateRendererProvider_Tests.WelcomeEmailModel>" +
                         Environment.NewLine +
                         "Merhaba @Model.Name, abp.io'ya hoşgeldiniz!";
    }
}
