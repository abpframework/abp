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

        public override string GetShortDescription()
        {
            return "Generates Angular service proxies and DTOs to consume HTTP APIs.";
        }
    }
}
