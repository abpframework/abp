using System;
using Volo.Abp.Cli.Args;

namespace Volo.Abp.Cli.Commands;

public interface ICommandSelector
{
    Type Select(CommandLineArgs commandLineArgs);
}
