using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Modularity;

public interface IOnPostApplicationInitialization
{
    Task OnPostApplicationInitializationAsync([NotNull] ApplicationInitializationContext context);

    void OnPostApplicationInitialization([NotNull] ApplicationInitializationContext context);
}
