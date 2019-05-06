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

            if (commandLineArgs.Command == "new")
            {
                return typeof(NewCommand);
            }

            if (commandLineArgs.Command == "add")
            {
                return typeof(AddCommand);
            }

            return typeof(HelpCommand);
        }
    }
}