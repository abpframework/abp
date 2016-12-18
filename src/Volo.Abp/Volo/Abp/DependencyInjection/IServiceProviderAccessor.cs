using System;

namespace Volo.Abp.DependencyInjection
{
    public interface IServiceProviderAccessor //TODO: Move to Volo.DependencyInjection package?
    {
        IServiceProvider ServiceProvider { get; }
    }
}