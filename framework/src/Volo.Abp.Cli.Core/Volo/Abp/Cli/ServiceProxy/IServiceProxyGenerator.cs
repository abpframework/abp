using System.Threading.Tasks;

namespace Volo.Abp.Cli.ServiceProxy
{
    public interface IServiceProxyGenerator
    {
        Task GenerateProxyAsync(GenerateProxyArgs args);
    }
}
