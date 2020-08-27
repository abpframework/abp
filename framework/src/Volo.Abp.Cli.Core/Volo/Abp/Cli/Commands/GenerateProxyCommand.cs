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

            var prompt = commandLineArgs.Options.ContainsKey("p") || commandLineArgs.Options.ContainsKey("prompt");
            var defaultValue = prompt ? null : "__default";

            var module = commandLineArgs.Options.GetOrNull(Options.Module.Short, Options.Module.Long) ?? defaultValue;
            var source = commandLineArgs.Options.GetOrNull(Options.Source.Short, Options.Source.Long) ?? defaultValue;
            var target = commandLineArgs.Options.GetOrNull(Options.Target.Short, Options.Target.Long) ?? defaultValue;

            var commandBuilder = new StringBuilder("npx ng g @abp/ng.schematics:proxy");

            if (module != null)
            {
                commandBuilder.Append($" --module {module}");
            }

            if (source != null)
            {
                commandBuilder.Append($" --source {source}");
            }

            if (target != null)
            {
                commandBuilder.Append($" --target {target}");
            }

            CmdHelper.RunCmd(commandBuilder.ToString());

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
            sb.AppendLine("Options:");
            sb.AppendLine("");
            sb.AppendLine("-m|--module <module-name>          (default: 'app') The name of the backend module you wish to generate proxies for.");
            sb.AppendLine("-s|--source <source-name>          (default: 'defaultProject') Angular project name to resolve the root namespace & API definition URL from.");
            sb.AppendLine("-t|--target <target-name>          (default: 'defaultProject') Angular project name to place generated code in.");
            sb.AppendLine("-p|--prompt                        Asks the options from the command line prompt (for the missing options)");
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

            public static class Prompt
            {
                public const string Short = "p";
                public const string Long = "prompt";
            }
        }
    }
}
