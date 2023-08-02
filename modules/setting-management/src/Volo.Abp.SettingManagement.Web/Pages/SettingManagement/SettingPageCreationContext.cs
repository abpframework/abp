using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

public class SettingPageCreationContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public List<SettingPageGroup> Groups { get; private set; }

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
        Groups = Groups.OrderBy(item => item.Order).ThenBy(item => item.DisplayName).ToList();
    }
}
