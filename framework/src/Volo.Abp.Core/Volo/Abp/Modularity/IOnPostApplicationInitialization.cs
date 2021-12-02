using JetBrains.Annotations;

namespace Volo.Abp.Modularity;

public interface IOnPostApplicationInitialization
{
    void OnPostApplicationInitialization([NotNull] ApplicationInitializationContext context);
}
