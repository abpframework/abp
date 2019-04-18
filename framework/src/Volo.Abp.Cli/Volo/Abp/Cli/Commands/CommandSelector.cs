using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli
{
    public class CommandSelector : ICommandSelector, ITransientDependency
    {
        public IConsoleCommand Select(CommandLineArgs commandLineArgs)
        {
            //TODO: Create options to define commands
            //TODO: Get from dependency injection instead of new?

            if (commandLineArgs.Command == "new")
            {
                return new NewProjectCommand(commandLineArgs);
            }

            return new MainHelpCommand(commandLineArgs);
        }
    }
}