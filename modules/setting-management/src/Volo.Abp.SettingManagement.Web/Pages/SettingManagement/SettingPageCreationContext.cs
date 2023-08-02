using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

public class SettingPageCreationContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public List<SettingPageGroup> Groups { get; }

    public SettingPageCreationContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;

        Groups = new List<SettingPageGroup>();
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
