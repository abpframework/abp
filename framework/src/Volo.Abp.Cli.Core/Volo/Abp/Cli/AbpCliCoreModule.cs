using System.Text;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Domain;
using Volo.Abp.IdentityModel;
using Volo.Abp.Json;
using Volo.Abp.Minify;
using Volo.Abp.Modularity;

namespace Volo.Abp.Cli
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpJsonModule),
        typeof(AbpIdentityModelModule),
        typeof(AbpMinifyModule)
    )]
    public class AbpCliCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Configure<AbpCliOptions>(options =>
            {
                //TODO: Define constants like done for GenerateProxyCommand.Name.
                options.Commands["help"] = typeof(HelpCommand);
                options.Commands["new"] = typeof(NewCommand);
                options.Commands["get-source"] = typeof(GetSourceCommand);
                options.Commands["update"] = typeof(UpdateCommand);
                options.Commands["add-package"] = typeof(AddPackageCommand);
                options.Commands["add-module"] = typeof(AddModuleCommand);
                options.Commands["login"] = typeof(LoginCommand);
                options.Commands["logout"] = typeof(LogoutCommand);
                options.Commands[GenerateProxyCommand.Name] = typeof(GenerateProxyCommand);
                options.Commands[RemoveProxyCommand.Name] = typeof(RemoveProxyCommand);
                options.Commands["suite"] = typeof(SuiteCommand);
                options.Commands["switch-to-preview"] = typeof(SwitchToPreviewCommand);
                options.Commands["switch-to-stable"] = typeof(SwitchToStableCommand);
                options.Commands["switch-to-nightly"] = typeof(SwitchToNightlyCommand);
                options.Commands["translate"] = typeof(TranslateCommand);
                options.Commands["build"] = typeof(BuildCommand);
                options.Commands["bundle"] = typeof(BundleCommand);
                options.Commands["create-migration-and-run-migrator"] = typeof(CreateMigrationAndRunMigrator);
            });
        }
    }
}
