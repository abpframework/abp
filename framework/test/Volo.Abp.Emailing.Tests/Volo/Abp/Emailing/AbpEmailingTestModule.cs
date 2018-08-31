using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Emailing.Templates.Virtual;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Emailing
{
    [DependsOn(
        typeof(AbpEmailingModule),
        typeof(AbpTestBaseModule))]
    public class AbpEmailingTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<VirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<AbpEmailingTestModule>();
            });

            context.Services.Configure<EmailTemplateOptions>(options =>
            {
                options.Templates["template1"] =
                    new EmailTemplateDefinition("template1")
                        .SetVirtualFilePath("/Volo/Abp/Emailing/TestTemplates/template1.html");
            });
        }
    }
}
