using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.SettingManagement.Blazor;

public class SettingComponentCreationContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public List<SettingComponentGroup> Groups { get; private set; }

    public SettingComponentCreationContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;

        Groups = new List<SettingComponentGroup>();
    }
    
    public void Normalize()
    {
        Order();
    }

    private void Order()
    {
        Groups = Groups.OrderBy(item => item.Order).ThenBy(item => item.DisplayName).ToList();
    }
}
