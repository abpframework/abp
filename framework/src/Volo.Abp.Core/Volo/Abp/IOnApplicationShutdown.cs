using JetBrains.Annotations;

namespace Volo.Abp
{
    public interface IOnApplicationShutdown
    {
        void OnApplicationShutdown([NotNull] ApplicationShutdownContext context);
    }
}