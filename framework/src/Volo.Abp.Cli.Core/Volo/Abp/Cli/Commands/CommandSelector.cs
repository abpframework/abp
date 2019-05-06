using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class CommandSelector : ICommandSelector, ITransientDependency
    {
        protected CliOptions Options { get; }

        public CommandSelector(IOptions<CliOptions> options)
        {
            Options = options.Value;
        }

        public Type Select(CommandLineArgs commandLineArgs)
        {
            return Options.Commands.GetOrDefault(commandLineArgs.Command) 
                   ?? typeof(HelpCommand);
        }
    }
}