using System;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class MainHelpCommand : ITransientDependency
    {
        public Task ExecuteAsync(CommandLineArgs commandLineArgs)
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