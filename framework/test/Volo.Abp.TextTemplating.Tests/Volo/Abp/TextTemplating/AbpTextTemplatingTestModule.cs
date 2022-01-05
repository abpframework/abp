using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.TextTemplating.Localization;
using Volo.Abp.TextTemplating.Razor;
using Volo.Abp.TextTemplating.Scriban;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating;

[DependsOn(
    typeof(AbpTextTemplatingScribanModule),
    typeof(AbpTextTemplatingRazorModule),
    typeof(AbpTestBaseModule),
    typeof(AbpAutofacModule),
    typeof(AbpLocalizationModule)
)]
public class AbpTextTemplatingTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpTextTemplatingTestModule>("Volo.Abp.TextTemplating");
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<TestLocalizationSource>("en")
                .AddVirtualJson("/Localization");
        });

        Configure<AbpCompiledViewProviderOptions>(options =>
        {
            options.TemplateReferences.Add(TestTemplates.HybridTemplateRazor,
                new List<PortableExecutableReference>()
                {
                        MetadataReference.CreateFromFile(typeof(AbpTextTemplatingTestModule).Assembly.Location)
                });
        });
    }
}
