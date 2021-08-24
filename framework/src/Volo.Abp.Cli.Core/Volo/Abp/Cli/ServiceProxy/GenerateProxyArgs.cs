using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Cli.ServiceProxy
{
    public class GenerateProxyArgs
    {
        [NotNull]
        public string CommandName { get; }

        [NotNull]
        public string WorkDirectory { get; }

        public string Module { get; }

        public string Url { get; }

        public string Output { get; }

        public string Target { get; }

        public string ApiName { get; }

        public string Source { get; }

        [NotNull]
        public Dictionary<string, string> ExtraProperties { get; set; }

        public GenerateProxyArgs(
            [NotNull] string commandName,
            [NotNull] string workDirectory,
            string module,
            string url,
            string output,
            string target,
            string apiName,
            string source,
            Dictionary<string, string> extraProperties = null)
        {
            CommandName = Check.NotNullOrWhiteSpace(commandName, nameof(commandName));
            WorkDirectory = Check.NotNullOrWhiteSpace(workDirectory, nameof(workDirectory));
            Module = module;
            Url = url;
            Output = output;
            Target = target;
            ApiName = apiName;
            Source = source;
            ExtraProperties = extraProperties ?? new Dictionary<string, string>();
        }
    }
}
