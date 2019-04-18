using System;
using System.Threading.Tasks;

namespace Volo.Abp.Cli
{
    public class MainHelpCommand : IConsoleCommand
    {
        public MainHelpCommand(CommandLineArgs commandLineArgs)
        {
            
        }

        public Task ExecuteAsync()
        {
            Console.WriteLine("*********** ABP CLI ****************");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine();
            Console.WriteLine("  abp <command> <target> [options]");
            return Task.CompletedTask;
        }
    }
}