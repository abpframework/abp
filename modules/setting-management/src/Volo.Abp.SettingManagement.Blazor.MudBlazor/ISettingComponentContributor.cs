using System.Threading.Tasks;

namespace Volo.Abp.SettingManagement.Blazor.MudBlazor;

public interface ISettingComponentContributor
{
    Task ConfigureAsync(SettingComponentCreationContext context);

    Task<bool> CheckPermissionsAsync(SettingComponentCreationContext context);
}
