using System.Text;
using Microsoft.Extensions.Options;
using Volo.Abp.Cli.ServiceProxy;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class GenerateProxyCommand : ProxyCommandBase
    {
        public const string Name = "generate-proxy";

        protected override string CommandName => Name;

        public GenerateProxyCommand(
            IOptions<AbpCliServiceProxyOptions> serviceProxyOptions,
            IHybridServiceScopeFactory serviceScopeFactory)
            : base(serviceProxyOptions, serviceScopeFactory)
        {
        }

        public override string GetUsageInfo()
        {
            var sb = new StringBuilder(base.GetUsageInfo());

            sb.AppendLine("");
            sb.AppendLine("Examples:");
            sb.AppendLine("");
            sb.AppendLine("  abp generate-proxy -t ng");
            sb.AppendLine("  abp generate-proxy -t js -m identity -o Pages/Identity/client-proxies.js -url https://localhost:44302/");
            sb.AppendLine("  abp generate-proxy -t csharp --folder MyProxies/InnerFolder -url https://localhost:44302/");

            return sb.ToString();
        }

        public override string GetShortDescription()
        {
            return "Generates client service proxies and DTOs to consume HTTP APIs.";
        }
    }
}
