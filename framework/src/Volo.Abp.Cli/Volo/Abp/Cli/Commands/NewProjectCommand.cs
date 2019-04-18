using System;
using System.Threading.Tasks;

namespace Volo.Abp.Cli
{
    public class NewProjectCommand : IConsoleCommand
    {
        protected CommandLineArgs CommandLineArgs { get; }

        public NewProjectCommand(CommandLineArgs commandLineArgs)
        {
            CommandLineArgs = commandLineArgs;
        }

        public Task ExecuteAsync()
        {
            Console.WriteLine("TODO: Create new project");
            return Task.CompletedTask;
        }
    }
}