using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp;

public interface IOnApplicationShutdown
{
    Task OnApplicationShutdownAsync([NotNull] ApplicationShutdownContext context);

    void OnApplicationShutdown([NotNull] ApplicationShutdownContext context);
}
