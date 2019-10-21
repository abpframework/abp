using System;
using System.Text;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Domain;
using Volo.Abp.IdentityModel;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.Cli
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpJsonModule),
        typeof(AbpIdentityModelModule)
    )]
    public class AbpCliCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // TODO: workaround until subsequent issues of https://github.com/dotnet/corefx/issues/30166 are resolved
            // a permanent fix will probably be published with the release of .net core 3.0: https://github.com/dotnet/corefx/issues/36553
            AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);

            Configure<AbpCliOptions>(options =>
            {
                options.Commands["help"] = typeof(HelpCommand);
                options.Commands["new"] = typeof(NewCommand);
                options.Commands["get-source"] = typeof(GetSourceCommand);
                options.Commands["update"] = typeof(UpdateCommand);
                options.Commands["add-package"] = typeof(AddPackageCommand);
                options.Commands["add-module"] = typeof(AddModuleCommand);
                options.Commands["login"] = typeof(LoginCommand);
                options.Commands["logout"] = typeof(LogoutCommand);
            });
        }
    }
}
