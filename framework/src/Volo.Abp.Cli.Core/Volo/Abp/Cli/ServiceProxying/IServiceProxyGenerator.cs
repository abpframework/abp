using System.Threading.Tasks;

namespace Volo.Abp.Cli.ServiceProxying;

public interface IServiceProxyGenerator
{
    Task GenerateProxyAsync(GenerateProxyArgs args);
}
