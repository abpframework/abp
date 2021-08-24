using Microsoft.Extensions.Options;
using Volo.Abp.Cli.ServiceProxy;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands
{
    public class RemoveProxyCommand : ProxyCommandBase
    {
        public const string Name = "remove-proxy";

        protected override string CommandName => Name;

        public RemoveProxyCommand(
            IOptions<AbpCliServiceProxyOptions> serviceProxyOptions,
            IHybridServiceScopeFactory serviceScopeFactory)
            : base(serviceProxyOptions, serviceScopeFactory)
        {
        }

        public override string GetShortDescription()
        {
            return "Remove Angular service proxies and DTOs to consume HTTP APIs.";
        }
    }
}
