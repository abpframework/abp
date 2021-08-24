using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Cli.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ServiceProxy.JavaScript
{
    public class JavaScriptServiceProxyGenerator : IServiceProxyGenerator, ITransientDependency
    {
        public const string Name = "JS";
        public const string EventTriggerScript = "abp.event.trigger('abp.serviceProxyScriptInitialized');";
        public const string DefaultOutput = "wwwroot/client-proxies";

        public IJsonSerializer JsonSerializer { get; }

        private readonly CliHttpClientFactory _cliHttpClientFactory;

        private readonly JQueryProxyScriptGenerator _jQueryProxyScriptGenerator;

        public JavaScriptServiceProxyGenerator(
            CliHttpClientFactory cliHttpClientFactory,
            IJsonSerializer jsonSerializer,
            JQueryProxyScriptGenerator jQueryProxyScriptGenerator)
        {
            JsonSerializer = jsonSerializer;
            _cliHttpClientFactory = cliHttpClientFactory;
            _jQueryProxyScriptGenerator = jQueryProxyScriptGenerator;
        }

        public async Task GenerateProxyAsync(GenerateProxyArgs args)
        {
            Check.NotNullOrWhiteSpace(args.Url, nameof(args.Url));
            CheckWorkDirectory(args.WorkDirectory);

            var apiDescriptionModel = await GetApplicationApiDescriptionModelAsync(args);
            var script = RemoveInitializedEventTrigger(_jQueryProxyScriptGenerator.CreateScript(apiDescriptionModel));

            var output = $"{args.WorkDirectory}/{DefaultOutput}/{args.Module}-proxy.js";
            if (!args.Output.IsNullOrWhiteSpace())
            {
                output = !args.Output.EndsWith(".js") ? $"{Path.GetDirectoryName(args.Output)}/{args.Module}-proxy.js" : args.Output;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(output));

            using (var writer = new StreamWriter(output))
            {
                await writer.WriteAsync(script);
            }
        }

        private async Task<ApplicationApiDescriptionModel> GetApplicationApiDescriptionModelAsync(GenerateProxyArgs args)
        {
            var client = _cliHttpClientFactory.CreateClient();

            var apiDefinitionResult = await client.GetStringAsync(CliUrls.GetApiDefinitionUrl(args.Url));
            var apiDefinition = JsonSerializer.Deserialize<ApplicationApiDescriptionModel>(apiDefinitionResult);

            if (!apiDefinition.Modules.TryGetValue(args.Module, out var moduleDefinition))
            {
                throw new CliUsageException($"Module name: {args.Module} is invalid");
            }

            var apiDescriptionModel = ApplicationApiDescriptionModel.Create();
            apiDescriptionModel.AddModule(moduleDefinition);

            return apiDescriptionModel;
        }

        private static void CheckWorkDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new CliUsageException("Specified directory does not exist.");
            }

            var projectFiles = Directory.GetFiles(directory, "*.csproj");
            if (!projectFiles.Any())
            {
                throw new CliUsageException(
                    "No project file found in the directory. The working directory must have a Web project file.");
            }
        }

        private static string RemoveInitializedEventTrigger(string script)
        {
            return script.Replace(EventTriggerScript, string.Empty);
        }
    }
}
