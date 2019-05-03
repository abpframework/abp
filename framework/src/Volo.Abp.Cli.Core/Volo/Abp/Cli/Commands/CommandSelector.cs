using System;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class CommandSelector : ICommandSelector, ITransientDependency
    {
        public Type Select(CommandLineArgs commandLineArgs)
        {
            //TODO: Create options to define commands
            //TODO: Get from dependency injection instead of new?

            if (commandLineArgs.Command == "new")
            {
                return typeof(NewProjectCommand);
            }

            return typeof(MainHelpCommand);
        }
    }
}