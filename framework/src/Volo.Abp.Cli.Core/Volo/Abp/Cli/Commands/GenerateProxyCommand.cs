using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class GenerateProxyCommand : IConsoleCommand, ITransientDependency
    {
        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            CheckAngularJsonFile();
            CheckNgSchematics();

            var module = commandLineArgs.Options.GetOrNull(Options.Module.Short, Options.Module.Long) ?? "__default";
            var source = commandLineArgs.Options.GetOrNull(Options.Source.Short, Options.Source.Long) ?? "__default";
            var target = commandLineArgs.Options.GetOrNull(Options.Target.Short, Options.Target.Long) ?? "__default";

            CmdHelper.RunCmd($"npx ng g @abp/ng.schematics:proxy --module {module} --source {source} --target {target}");

            return Task.CompletedTask;
        }

        private void CheckNgSchematics()
        {
            var packageJsonPath = $"package.json";

            if (!File.Exists(packageJsonPath))
            {
                throw new CliUsageException(
                    "package.json file not found" +
                    Environment.NewLine +
                    GetUsageInfo()
                );
            }

            var schematicsPackageNode =
                (string) JObject.Parse(File.ReadAllText(packageJsonPath))["devDependencies"]?["@abp/ng.schematics"];

            if (schematicsPackageNode == null)
            {
                throw new CliUsageException(
                    "\"@abp/ng.schematics\" NPM package should be installed to the devDependencies before running this command!" +
                    Environment.NewLine +
                    GetUsageInfo()
                );
            }
        }

        private void CheckAngularJsonFile()
        {
            var angularPath = $"angular.json";
            if (!File.Exists(angularPath))
            {
                throw new CliUsageException(
                    "angular.json file not found. You must run this command in the angular folder." +
                    Environment.NewLine + Environment.NewLine +
                    GetUsageInfo()
                );
            }
        }

        public string GetUsageInfo()
        {
            var sb = new StringBuilder();

            sb.AppendLine("");
            sb.AppendLine("Usage:");
            sb.AppendLine("");
            sb.AppendLine("  abp generate-proxy");
            sb.AppendLine("");
            sb.AppendLine("Examples:");
            sb.AppendLine("");
            sb.AppendLine("  abp generate-proxy");
            sb.AppendLine("");
            sb.AppendLine("See the documentation for more info: https://docs.abp.io/en/abp/latest/CLI");

            return sb.ToString();
        }

        public string GetShortDescription()
        {
            return "Generates Angular service proxies and DTOs to consume HTTP APIs.";
        }

        public static class Options
        {
            public static class Module
            {
                public const string Short = "m";
                public const string Long = "module";
            }

            public static class Source
            {
                public const string Short = "s";
                public const string Long = "source";
            }

            public static class Target
            {
                public const string Short = "t";
                public const string Long = "target";
            }
        }
    }
}
