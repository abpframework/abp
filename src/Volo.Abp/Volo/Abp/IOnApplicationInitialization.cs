using JetBrains.Annotations;
using Volo.Abp.Modularity;

namespace Volo.Abp
{
    public interface IOnApplicationInitialization
    {
        void OnApplicationInitialization([NotNull] ApplicationInitializationContext context);
    }
}