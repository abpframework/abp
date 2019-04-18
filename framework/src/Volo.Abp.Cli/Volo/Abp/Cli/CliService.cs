using System;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli
{
    public class CliService : ITransientDependency
    {
        protected ICommandLineArgumentParser CommandLineArgumentParser { get; }
        protected ICommandSelector CommandSelector { get; }

        public CliService(
            ICommandLineArgumentParser commandLineArgumentParser, 
            ICommandSelector commandSelector)
        {
            CommandLineArgumentParser = commandLineArgumentParser;
            CommandSelector = commandSelector;
        }

        public async Task RunAsync(string[] args)
        {
            var commandLineArgs = CommandLineArgumentParser.Parse(args);
            var command = CommandSelector.Select(commandLineArgs);
            await command.ExecuteAsync();
            Console.WriteLine(commandLineArgs);
        }
    }
}