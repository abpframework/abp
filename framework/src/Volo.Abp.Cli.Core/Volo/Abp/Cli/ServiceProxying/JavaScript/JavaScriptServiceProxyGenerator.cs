using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Cli.Commands;
using Volo.Abp.Cli.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.ServiceProxying.JavaScript
{
    public class JavaScriptServiceProxyGenerator : ServiceProxyGeneratorBase<JavaScriptServiceProxyGenerator>, ITransientDependency
    {
        public const string Name = "JS";
        private const string EventTriggerScript = "abp.event.trigger('abp.serviceProxyScriptInitialized');";
        private const string DefaultOutput = "wwwroot/client-proxies";

        private readonly JQueryProxyScriptGenerator _jQueryProxyScriptGenerator;

        public JavaScriptServiceProxyGenerator(
            CliHttpClientFactory cliHttpClientFactory,
            IJsonSerializer jsonSerializer,
            JQueryProxyScriptGenerator jQueryProxyScriptGenerator) :
            base(cliHttpClientFactory, jsonSerializer)
        {
            _jQueryProxyScriptGenerator = jQueryProxyScriptGenerator;
        }

        public override async Task GenerateProxyAsync(GenerateProxyArgs args)
        {
            CheckWorkDirectory(args.WorkDirectory);

            var output = Path.Combine(args.WorkDirectory, DefaultOutput, $"{args.Module}-proxy.js");
            if (!args.Output.IsNullOrWhiteSpace())
            {
                output = args.Output.EndsWith(".js") ? Path.Combine(args.WorkDirectory, args.Output) : Path.Combine(args.WorkDirectory, Path.GetDirectoryName(args.Output), $"{args.Module}-proxy.js");
            }

            if (args.CommandName == RemoveProxyCommand.Name)
            {
                RemoveProxy(args, output);
                return;
            }

            var applicationApiDescriptionModel = await GetApplicationApiDescriptionModelAsync(args);
            var script = RemoveInitializedEventTrigger(_jQueryProxyScriptGenerator.CreateScript(applicationApiDescriptionModel));

            Directory.CreateDirectory(Path.GetDirectoryName(output));

            using (var writer = new StreamWriter(output))
            {
                await writer.WriteAsync(script);
            }

            Logger.LogInformation($"Create {GetLoggerOutputPath(output, args.WorkDirectory)}");
        }

        private void RemoveProxy(GenerateProxyArgs args, string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            Logger.LogInformation($"Delete {GetLoggerOutputPath(filePath, args.WorkDirectory)}");
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
