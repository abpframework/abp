using Volo.Abp.Cli.Commands;

namespace Volo.Abp.Cli.Args;

public static class CommandLineArgsExtensions
{
    public static bool IsMcpCommand(this CommandLineArgs args)
    {
        return args.IsCommand(McpCommand.Name);
    }
}
