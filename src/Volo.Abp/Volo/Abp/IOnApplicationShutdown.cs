using JetBrains.Annotations;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    public interface IOnApplicationShutdown
    {
        void OnApplicationShutdown([NotNull] ApplicationShutdownContext context);
    }
}