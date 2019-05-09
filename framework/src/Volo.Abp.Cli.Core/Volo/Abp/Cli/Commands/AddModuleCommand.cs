using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ProjectModification;

namespace Volo.Abp.Cli.Commands
{
    public class AddModuleCommand : IConsoleCommand, ITransientDependency
    {
        public ILogger<AddModuleCommand> Logger { get; set; }

        protected ModuleAdder ModuleAdder { get; }

        public AddModuleCommand(ModuleAdder moduleAdder)
        {
            ModuleAdder = moduleAdder;
            Logger = NullLogger<AddModuleCommand>.Instance;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target == null)
            {
                Logger.LogWarning("Module name is missing.");
                AddCommandHelper.WriteUsage(Logger);
                return;
            }

            await ModuleAdder.AddModuleAsync(
                new AddModuleArgs(
                    commandLineArgs.Target,
                    commandLineArgs.Options.GetOrNull(Options.Solution.Short, Options.Solution.Long),
                    commandLineArgs.Options.GetOrNull(Options.Project.Short, Options.Project.Long)
                )
            );
        }

        public static class Options
        {
            public static class Solution
            {
                public const string Short = "s";
                public const string Long = "solution";
            }

            public static class Project
            {
                public const string Short = "p";
                public const string Long = "project";
            }
        }
    }
}
