using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp;

public interface IOnApplicationInitialization
{
    Task OnApplicationInitializationAsync([NotNull] ApplicationInitializationContext context);

    void OnApplicationInitialization([NotNull] ApplicationInitializationContext context);
}
