using System;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class HelpCommand : IConsoleCommand, ITransientDependency
    {
        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine();
            Console.WriteLine("  abp <command> <target> [options]");
            Console.WriteLine();
            return Task.CompletedTask;
        }
    }
}