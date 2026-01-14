using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class CommandSelector : ICommandSelector, ITransientDependency
{
    protected AbpCliOptions Options { get; }

    public CommandSelector(IOptions<AbpCliOptions> options)
    {
        Options = options.Value;
    }

    public Type Select(CommandLineArgs commandLineArgs)
    {
        // Don't fall back to HelpCommand for MCP command to avoid corrupting stdout JSON-RPC stream
        if (commandLineArgs.IsMcpCommand())
        {
            return Options.Commands.GetOrDefault("mcp") ?? typeof(HelpCommand);
        }

        if (commandLineArgs.Command.IsNullOrWhiteSpace())
        {
            return typeof(HelpCommand);
        }

        return Options.Commands.GetOrDefault(commandLineArgs.Command)
               ?? typeof(HelpCommand);
    }
}
