using JetBrains.Annotations;

namespace Volo.Abp.Modularity
{
    public interface IOnApplicationInitialization
    {
        void OnApplicationInitialization([NotNull] ApplicationInitializationContext context);
    }
}