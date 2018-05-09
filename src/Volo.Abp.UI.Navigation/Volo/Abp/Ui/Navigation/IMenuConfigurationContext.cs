using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.UI.Navigation
{
    public interface IMenuConfigurationContext : IServiceProviderAccessor
    {
        ApplicationMenu Menu { get; }

        //TODO: Add Localization, Authorization components since they are most used components on menu creation!
    }
}