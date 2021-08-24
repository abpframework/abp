using JetBrains.Annotations;
using Volo.Abp.Cli.Args;

namespace Volo.Abp.Cli.ServiceProxy
{
    public class GenerateProxyArgs
    {
        [NotNull]
        public string CommandName { get; }

        public string Module { get; }

        public string Url { get; }

        [NotNull]
        public AbpCommandLineOptions ExtraProperties { get; set; }

        public GenerateProxyArgs(
            [NotNull] string commandName,
             string module,
             string url,
            AbpCommandLineOptions extraProperties = null)
        {
            CommandName = Check.NotNullOrWhiteSpace(commandName, nameof(commandName));
            Module = module;
            Url = url;
            ExtraProperties = extraProperties ?? new AbpCommandLineOptions();
        }
    }
}
