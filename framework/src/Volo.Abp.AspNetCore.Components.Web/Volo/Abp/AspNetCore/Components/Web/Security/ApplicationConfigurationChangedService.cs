using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.Security;

public delegate void ApplicationConfigurationChangedHandler();

public class ApplicationConfigurationChangedService : IScopedDependency
{
    public event ApplicationConfigurationChangedHandler Changed = default!;

    public void NotifyChanged()
    {
        Changed?.Invoke();
    }
}
