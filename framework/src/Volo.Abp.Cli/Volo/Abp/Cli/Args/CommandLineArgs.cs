using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Volo.Abp.Cli.Args;

namespace Volo.Abp.Cli
{
    public class CommandLineArgs
    {
        [CanBeNull]
        public string Command { get; }

        [CanBeNull]
        public string Target { get; }

        [NotNull]
        public CommandLineOptions Options { get; }

        public CommandLineArgs([CanBeNull] string command = null, [CanBeNull] string target = null)
        {
            Command = command;
            Target = target;
            Options = new CommandLineOptions();
        }

        public static CommandLineArgs Empty()
        {
            return new CommandLineArgs();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Command != null)
            {
                sb.AppendLine($"Command: {Command}");
            }

            if (Target != null)
            {
                sb.AppendLine($"Target: {Target}");
            }

            if (Options.Any())
            {
                sb.AppendLine("Options:");
                foreach (var option in Options)
                {
                    sb.AppendLine($" - {option.Key} = {option.Value}");
                }
            }

            if (sb.Length <= 0)
            {
                sb.Append("<EMPTY>");
            }

            return sb.ToString();
        }
    }
}