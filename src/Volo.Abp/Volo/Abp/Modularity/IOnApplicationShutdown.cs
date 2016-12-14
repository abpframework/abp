using JetBrains.Annotations;

namespace Volo.Abp.Modularity
{
    public interface IOnApplicationShutdown
    {
        void OnApplicationShutdown([NotNull] ApplicationShutdownContext context);
    }
}