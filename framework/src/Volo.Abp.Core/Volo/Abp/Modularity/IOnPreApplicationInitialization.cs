using JetBrains.Annotations;

namespace Volo.Abp.Modularity;

public interface IOnPreApplicationInitialization
{
    void OnPreApplicationInitialization([NotNull] ApplicationInitializationContext context);
}
