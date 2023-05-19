using Microsoft.CodeAnalysis;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.TextTemplating.Razor;

[DependsOn(
    typeof(AbpTextTemplatingTestModule)
)]
public class RazorTextTemplatingTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<RazorTextTemplatingTestModule>("Volo.Abp.TextTemplating.Razor");
        });

        Configure<AbpRazorTemplateCSharpCompilerOptions>(options =>
        {
            options.References.Add(MetadataReference.CreateFromFile(typeof(RazorTextTemplatingTestModule).Assembly.Location));
        });
    }
}
