using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NuGet.Versioning;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.Utils;
using Volo.Abp.Cli.Version;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ServiceProxying.Angular;

public class AngularServiceProxyGenerator : ServiceProxyGeneratorBase<AngularServiceProxyGenerator>, ITransientDependency
{
    public const string Name = "NG";

    private readonly CliVersionService _cliVersionService;
    private readonly ICmdHelper _cmdhelper;

    public AngularServiceProxyGenerator(
        CliHttpClientFactory cliHttpClientFactory,
        IJsonSerializer jsonSerializer,
        ICmdHelper cmdhelper,
        CliVersionService cliVersionService) :
        base(cliHttpClientFactory, jsonSerializer)
    {
        _cmdhelper = cmdhelper;
        _cliVersionService = cliVersionService;
    }

    public async override Task GenerateProxyAsync(GenerateProxyArgs args)
    {
        CheckAngularJsonFile();
        await CheckNgSchematicsAsync();

        var schematicsCommandName = args.CommandName == RemoveProxyCommand.Name ? "proxy-remove" : "proxy-add";
        var prompt = args.ExtraProperties.ContainsKey("p") || args.ExtraProperties.ContainsKey("prompt");
        var defaultValue = prompt ? null : "__default";

        var module = defaultValue;
        if (args.ExtraProperties.ContainsKey("t") || args.ExtraProperties.ContainsKey("module"))
        {
            module = args.Module;
        }

        var apiName = args.ApiName ?? defaultValue;
        var source = args.Source ?? defaultValue;
        var target = args.Target ?? defaultValue;
        var url = args.Url ?? defaultValue;
        var entryPoint = args.EntryPoint ?? defaultValue;

        var commandBuilder = new StringBuilder("npx ng g @abp/ng.schematics:" + schematicsCommandName);

        if (module != null)
        {
            commandBuilder.Append($" --module {module}");
        }

        if (apiName != null)
        {
            commandBuilder.Append($" --api-name {apiName}");
        }

        if (source != null)
        {
            commandBuilder.Append($" --source {source}");
        }

        if (target != null)
        {
            commandBuilder.Append($" --target {target}");
        }

        if (url != null)
        {
            commandBuilder.Append($" --url {url}");
        }

        if (entryPoint != null)
        {
            commandBuilder.Append($" --entry-point {entryPoint}");
        }

        var serviceType = GetServiceType(args) ?? Volo.Abp.Cli.ServiceProxying.ServiceType.Application;
        commandBuilder.Append($" --service-type {serviceType.ToString().ToLower()}");


        _cmdhelper.RunCmd(commandBuilder.ToString());
    }

    protected override ServiceType? GetDefaultServiceType(GenerateProxyArgs args)
    {
        return ServiceType.Application;
    }

    private async Task CheckNgSchematicsAsync()
    {
        var packageJsonPath = $"package.json";

        if (!File.Exists(packageJsonPath))
        {
            throw new CliUsageException(
                "package.json file not found"
            );
        }

        var schematicsVersion =
            (string)JObject.Parse(File.ReadAllText(packageJsonPath))["devDependencies"]?["@abp/ng.schematics"];

        if (schematicsVersion == null)
        {
            throw new CliUsageException(
                "\"@abp/ng.schematics\" NPM package should be installed to the devDependencies before running this command!"
            );
        }

        var parsed = SemanticVersion.TryParse(schematicsVersion.TrimStart('~', '^', 'v'), out var semanticSchematicsVersion);
        if (!parsed)
        {
            Logger.LogWarning("Couldn't determinate version of \"@abp/ng.schematics\" package.");
            return;
        }

        var cliVersion = await _cliVersionService.GetCurrentCliVersionAsync();
        if (semanticSchematicsVersion < cliVersion)
        {
            Logger.LogWarning("\"@abp/ng.schematics\" version is lower than ABP Cli version.");
        }
    }

    private static void CheckAngularJsonFile()
    {
        var angularPath = $"angular.json";
        if (!File.Exists(angularPath))
        {
            throw new CliUsageException(
                "angular.json file not found. You must run this command in the angular folder."
            );
        }
    }
}
