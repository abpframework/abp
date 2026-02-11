using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Cli.Args;
using Volo.Abp.Cli.ServiceProxying;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public abstract class ProxyCommandBase<T> : IConsoleCommand, ITransientDependency where T : IConsoleCommand
{
    public ILogger<T> Logger { get; set; }

    protected abstract string CommandName { get; }

    protected AbpCliServiceProxyOptions ServiceProxyOptions { get; }

    protected IServiceScopeFactory ServiceScopeFactory { get; }

    public ProxyCommandBase(
        IOptions<AbpCliServiceProxyOptions> serviceProxyOptions,
        IServiceScopeFactory serviceScopeFactory)
    {
        ServiceScopeFactory = serviceScopeFactory;
        ServiceProxyOptions = serviceProxyOptions.Value;
        Logger = NullLogger<T>.Instance;
    }

    public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var generateType = commandLineArgs.Options.GetOrNull(Options.GenerateType.Short, Options.GenerateType.Long)?.ToUpper();

        if (string.IsNullOrWhiteSpace(generateType))
        {
            throw new CliUsageException("Option Type is required" +
                Environment.NewLine +
                GetUsageInfo());
        }

        if (!ServiceProxyOptions.Generators.ContainsKey(generateType))
        {
            throw new CliUsageException("Option Type value is invalid" +
                Environment.NewLine +
                GetUsageInfo());
        }

        using (var scope = ServiceScopeFactory.CreateScope())
        {
            var generatorType = ServiceProxyOptions.Generators[generateType];
            var serviceProxyGenerator = scope.ServiceProvider.GetService(generatorType).As<IServiceProxyGenerator>();

            await serviceProxyGenerator.GenerateProxyAsync(BuildArgs(commandLineArgs));
        }
    }

    private GenerateProxyArgs BuildArgs(CommandLineArgs commandLineArgs)
    {
        var url = commandLineArgs.Options.GetOrNull(Options.Url.Short, Options.Url.Long);
        var target = commandLineArgs.Options.GetOrNull(Options.Target.Long);
        var module = commandLineArgs.Options.GetOrNull(Options.Module.Short, Options.Module.Long) ?? "app";
        var output = commandLineArgs.Options.GetOrNull(Options.Output.Short, Options.Output.Long);
        var apiName = commandLineArgs.Options.GetOrNull(Options.ApiName.Short, Options.ApiName.Long);
        var source = commandLineArgs.Options.GetOrNull(Options.Source.Short, Options.Source.Long);
        var workDirectory = commandLineArgs.Options.GetOrNull(Options.WorkDirectory.Short, Options.WorkDirectory.Long) ?? Directory.GetCurrentDirectory();
        var folder = commandLineArgs.Options.GetOrNull(Options.Folder.Long);
        var serviceTypeArg = commandLineArgs.Options.GetOrNull(Options.ServiceType.Short, Options.ServiceType.Long);
        var entryPointArg = commandLineArgs.Options.GetOrNull(Options.EntryPoint.Short, Options.EntryPoint.Long);


        ServiceType? serviceType = null;
        if (!serviceTypeArg.IsNullOrWhiteSpace())
        {
            serviceType = serviceTypeArg.ToLower() == "application"
                ? ServiceType.Application
                : serviceTypeArg.ToLower() == "integration"
                    ? ServiceType.Integration
                    : ServiceType.All;
        }

        var withoutContracts = commandLineArgs.Options.ContainsKey(Options.WithoutContracts.Short) ||
                               commandLineArgs.Options.ContainsKey(Options.WithoutContracts.Long);

        return new GenerateProxyArgs(CommandName, workDirectory, module, url, output, target, apiName, source, folder, serviceType, entryPointArg,withoutContracts, commandLineArgs.Options);
    }

    public virtual string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("");
        sb.AppendLine($"  abp {CommandName}");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("");
        sb.AppendLine("-m|--module <module-name>                         (default: 'app') The name of the backend module you wish to generate proxies for.");
        sb.AppendLine("-wd|--working-directory <directory-path>          Execution directory.");
        sb.AppendLine("-u|--url <url>                                    API definition URL from.");
        sb.AppendLine("-t|--type <generate-type>                         The name of generate type (csharp, js, ng).");
        sb.AppendLine("  csharp");
        sb.AppendLine("     --without-contracts                               Avoid generating the application service interface, class, enum and dto types.");
        sb.AppendLine("     --folder <folder-name>                            (default: 'ClientProxies') Folder name to place generated CSharp code in.");
        sb.AppendLine("  js");
        sb.AppendLine("     -o|--output <output-name>                         JavaScript file path or folder to place generated code in.");
        sb.AppendLine("  ng");
        sb.AppendLine("     -a|--api-name <module-name>                       (default: 'default') The name of the API endpoint defined in the /src/environments/environment.ts.");
        sb.AppendLine("     -s|--source <source-name>                         (default: 'defaultProject') Angular project name to resolve the root namespace & API definition URL from.");
        sb.AppendLine("     -p|--prompt                                       Asks the options from the command line prompt (for the missing options)");
        sb.AppendLine("     --target <target-name>                            (default: 'defaultProject') Angular project name to place generated code in.");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://abp.io/docs/latest/cli");

        return sb.ToString();
    }

    public static class Options
    {
        public static class Module
        {
            public const string Short = "m";
            public const string Long = "module";
        }

        public static class GenerateType
        {
            public const string Short = "t";
            public const string Long = "type";
        }

        public static class ApiName
        {
            public const string Short = "a";
            public const string Long = "api-name";
        }

        public static class Source
        {
            public const string Short = "s";
            public const string Long = "source";
        }
        public static class Output
        {
            public const string Short = "o";
            public const string Long = "output";
        }

        public static class Target
        {
            public const string Long = "target";
        }

        public static class Prompt
        {
            public const string Short = "p";
            public const string Long = "prompt";
        }

        public static class Folder
        {
            public const string Long = "folder";
        }

        public static class Url
        {
            public const string Short = "u";
            public const string Long = "url";
        }

        public static class WorkDirectory
        {
            public const string Short = "wd";
            public const string Long = "working-directory";
        }


        public static class ServiceType
        {
            public const string Short = "st";
            public const string Long = "service-type";
        }

        public static class WithoutContracts
        {
            public const string Short = "c";
            public const string Long = "without-contracts";
        }
          public static class EntryPoint
        {
            public const string Short = "ep";
            public const string Long = "entry-point";
        }
    }
}
