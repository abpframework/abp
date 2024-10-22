using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Cli.ServiceProxying;

namespace Volo.Abp.Cli.Commands;

public class RemoveProxyCommand : ProxyCommandBase<RemoveProxyCommand>
{
    public const string Name = "remove-proxy";

    protected override string CommandName => Name;

    public RemoveProxyCommand(
        IOptions<AbpCliServiceProxyOptions> serviceProxyOptions,
        IServiceScopeFactory serviceScopeFactory)
        : base(serviceProxyOptions, serviceScopeFactory)
    {
    }

    public override string GetUsageInfo()
    {
        var sb = new StringBuilder(base.GetUsageInfo());

        sb.AppendLine("");
        sb.AppendLine("Examples:");
        sb.AppendLine("");
        sb.AppendLine("  abp remove-proxy -t ng");
        sb.AppendLine("  abp remove-proxy -t js -m identity -o Pages/Identity/client-proxies.js");
        sb.AppendLine("  abp remove-proxy -t csharp --folder MyProxies/InnerFolder");

        return sb.ToString();
    }

    public static string GetShortDescription()
    {
        return "Remove client service proxies and DTOs to consume HTTP APIs.";
    }
}
