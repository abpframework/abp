using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.LIbs;
using Volo.Abp.Cli.ServiceProxy;
using Volo.Abp.Cli.ServiceProxy.Angular;
using Volo.Abp.Cli.ServiceProxy.CSharp;
using Volo.Abp.Cli.ServiceProxy.JavaScript;
using Volo.Abp.Domain;
using Volo.Abp.Http;
using Volo.Abp.IdentityModel;
using Volo.Abp.Json;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Minify;
using Volo.Abp.Modularity;

namespace Volo.Abp.Cli
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpJsonModule),
        typeof(AbpIdentityModelModule),
        typeof(AbpMinifyModule),
        typeof(AbpHttpModule)
    )]
    public class AbpCliCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClient(CliConsts.HttpClientName)
                .ConfigurePrimaryHttpMessageHandler(() => new CliHttpClientHandler());

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Configure<AbpSystemTextJsonSerializerOptions>(options =>
            {
                options.UnsupportedTypes.Add(typeof(ResourceMapping));
            });

            Configure<AbpCliOptions>(options =>
            {
                //TODO: Define constants like done for GenerateProxyCommand.Name.
                options.Commands["help"] = typeof(HelpCommand);
                options.Commands["prompt"] = typeof(PromptCommand);
                options.Commands["new"] = typeof(NewCommand);
                options.Commands["get-source"] = typeof(GetSourceCommand);
                options.Commands["update"] = typeof(UpdateCommand);
                options.Commands["add-package"] = typeof(AddPackageCommand);
                options.Commands["add-module"] = typeof(AddModuleCommand);
                options.Commands["list-modules"] = typeof(ListModulesCommand);
                options.Commands["login"] = typeof(LoginCommand);
                options.Commands["login-info"] = typeof(LoginInfoCommand);
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
                options.Commands["create-migration-and-run-migrator"] = typeof(CreateMigrationAndRunMigratorCommand);
                options.Commands["install-libs"] = typeof(InstallLibsCommand);
            });

            Configure<AbpCliServiceProxyOptions>(options =>
            {
                options.Generators[JavaScriptServiceProxyGenerator.Name] = typeof(JavaScriptServiceProxyGenerator);
                options.Generators[AngularServiceProxyGenerator.Name] = typeof(AngularServiceProxyGenerator);
                options.Generators[CSharpServiceProxyGenerator.Name] = typeof(CSharpServiceProxyGenerator);
            });
        }
    }
}
