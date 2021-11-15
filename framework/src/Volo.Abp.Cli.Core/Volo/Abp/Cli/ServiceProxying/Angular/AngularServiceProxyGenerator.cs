using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NuGet.Versioning;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ServiceProxying.Angular
{
    public class AngularServiceProxyGenerator : ServiceProxyGeneratorBase<AngularServiceProxyGenerator> , ITransientDependency
    {
        public const string Name = "NG";

        private readonly CliService _cliService;
        private readonly ICmdHelper _cmdhelper;

        public AngularServiceProxyGenerator(
            CliHttpClientFactory cliHttpClientFactory,
            IJsonSerializer jsonSerializer,
            ICmdHelper cmdhelper,
            CliService cliService) :
            base(cliHttpClientFactory, jsonSerializer)
        {
            _cmdhelper = cmdhelper;
            _cliService = cliService;
        }

        public override async Task GenerateProxyAsync(GenerateProxyArgs args)
        {
            CheckAngularJsonFile();
            await CheckNgSchematicsAsync();

            var schematicsCommandName = args.CommandName == RemoveProxyCommand.Name ? "proxy-remove" : "proxy-add";
            var prompt = args.ExtraProperties.ContainsKey("p") || args.ExtraProperties.ContainsKey("prompt");
            var defaultValue = prompt ? null : "__default";

            var module = args.Module ?? defaultValue;
            var apiName = args.ApiName ?? defaultValue;
            var source = args.Source ?? defaultValue;
            var target = args.Target ?? defaultValue;

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

            _cmdhelper.RunCmd(commandBuilder.ToString());
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
                (string) JObject.Parse(File.ReadAllText(packageJsonPath))["devDependencies"]?["@abp/ng.schematics"];

            if (schematicsVersion == null)
            {
                throw new CliUsageException(
                    "\"@abp/ng.schematics\" NPM package should be installed to the devDependencies before running this command!"
                );
            }

            var parseError = SemanticVersion.TryParse(schematicsVersion.TrimStart('~', '^', 'v'), out var semanticSchematicsVersion);
            if (parseError)
            {
                Logger.LogWarning("Couldn't determinate version of \"@abp/ng.schematics\" package.");
                return;
            }

            var cliVersion = await _cliService.GetCurrentCliVersionAsync(typeof(CliService).Assembly);
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
}
