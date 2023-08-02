using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.SettingManagement.Blazor;

public class SettingComponentCreationContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public List<SettingComponentGroup> Groups { get; }

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
        var orderedItems = Groups.OrderBy(item => item.Order).ThenBy(item => item.DisplayName).ToArray();
        Groups.Clear();
        Groups.AddRange(orderedItems);
    }
}
