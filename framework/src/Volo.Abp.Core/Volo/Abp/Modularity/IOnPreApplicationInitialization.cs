using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Modularity;

public interface IOnPreApplicationInitialization
{
    Task OnPreApplicationInitializationAsync([NotNull] ApplicationInitializationContext context);

    void OnPreApplicationInitialization([NotNull] ApplicationInitializationContext context);
}
