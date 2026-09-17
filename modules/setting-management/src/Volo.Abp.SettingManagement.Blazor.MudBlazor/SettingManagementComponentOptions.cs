using System.Collections.Generic;

namespace Volo.Abp.SettingManagement.Blazor.MudBlazor;

public class SettingManagementComponentOptions
{
    public List<ISettingComponentContributor> Contributors { get; }

    public SettingManagementComponentOptions()
    {
        Contributors = new List<ISettingComponentContributor>();
    }
}
