using System.Text;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Domain;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.Cli
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpJsonModule)
    )]
    public class AbpCliCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Configure<CliOptions>(options =>
            {
                options.Commands["help"] = typeof(HelpCommand);
                options.Commands["new"] = typeof(NewCommand);
                options.Commands["add"] = typeof(AddCommand);
                options.Commands["login"] = typeof(LoginCommand);
                options.Commands["logout"] = typeof(LogoutCommand);
            });
        }
    }
}
