using System.Threading.Tasks;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement
{
    public interface ISettingPageContributor
    {
        Task ConfigureAsync(SettingPageCreationContext context);

        Task<bool> CheckPermissionsAsync(SettingPageCreationContext context);
    }
}