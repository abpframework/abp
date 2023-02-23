using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ServiceProxying;
using Volo.Abp.Cli.ServiceProxying.Angular;
using Volo.Abp.Cli.ServiceProxying.CSharp;
using Volo.Abp.Cli.ServiceProxying.JavaScript;
using Volo.Abp.Domain;
using Volo.Abp.Http;
using Volo.Abp.IdentityModel;
using Volo.Abp.Json;
using Volo.Abp.Minify;
using Volo.Abp.Modularity;

namespace Volo.Abp.Cli;

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

        context.Services.AddHttpClient(CliConsts.GithubHttpClientName, client =>
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MyAgent/1.0");
        });

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Configure<AbpCliOptions>(options =>
        {
            options.Commands[HelpCommand.Name] = typeof(HelpCommand);
            options.Commands[PromptCommand.Name] = typeof(PromptCommand);
            options.Commands[NewCommand.Name] = typeof(NewCommand);
            options.Commands[GetSourceCommand.Name] = typeof(GetSourceCommand);
            options.Commands[UpdateCommand.Name] = typeof(UpdateCommand);
            options.Commands[AddPackageCommand.Name] = typeof(AddPackageCommand);
            options.Commands[AddModuleCommand.Name] = typeof(AddModuleCommand);
            options.Commands[ListModulesCommand.Name] = typeof(ListModulesCommand);
            options.Commands[ListTemplatesCommand.Name] = typeof(ListTemplatesCommand);
            options.Commands[LoginCommand.Name] = typeof(LoginCommand);
            options.Commands[LoginInfoCommand.Name] = typeof(LoginInfoCommand);
            options.Commands[LogoutCommand.Name] = typeof(LogoutCommand);
            options.Commands[GenerateProxyCommand.Name] = typeof(GenerateProxyCommand);
            options.Commands[RemoveProxyCommand.Name] = typeof(RemoveProxyCommand);
            options.Commands[SuiteCommand.Name] = typeof(SuiteCommand);
            options.Commands[SwitchToPreviewCommand.Name] = typeof(SwitchToPreviewCommand);
            options.Commands[SwitchToStableCommand.Name] = typeof(SwitchToStableCommand);
            options.Commands[SwitchToNightlyCommand.Name] = typeof(SwitchToNightlyCommand);
            options.Commands[TranslateCommand.Name] = typeof(TranslateCommand);
            options.Commands[BuildCommand.Name] = typeof(BuildCommand);
            options.Commands[BundleCommand.Name] = typeof(BundleCommand);
            options.Commands[CreateMigrationAndRunMigratorCommand.Name] = typeof(CreateMigrationAndRunMigratorCommand);
            options.Commands[InstallLibsCommand.Name] = typeof(InstallLibsCommand);
            options.Commands[CleanCommand.Name] = typeof(CleanCommand);
            options.Commands[CliCommand.Name] = typeof(CliCommand);
            
            options.DisabledModulesToAddToSolution.Add("Volo.Abp.LeptonXTheme.Pro");
            options.DisabledModulesToAddToSolution.Add("Volo.Abp.LeptonXTheme.Lite");
        });

        Configure<AbpCliServiceProxyOptions>(options =>
        {
            options.Generators[JavaScriptServiceProxyGenerator.Name] = typeof(JavaScriptServiceProxyGenerator);
            options.Generators[AngularServiceProxyGenerator.Name] = typeof(AngularServiceProxyGenerator);
            options.Generators[CSharpServiceProxyGenerator.Name] = typeof(CSharpServiceProxyGenerator);
        });
    }
}
