using System;
using System.Threading.Tasks;

namespace Volo.Abp.SettingManagement.Web.Pages.SettingManagement;

public interface ISettingPageContributor
{
    Task ConfigureAsync(SettingPageCreationContext context);

    [Obsolete("Use SettingPageContributorBase as base class and call `RequiredPermissions` or `RequiredFeatures` for better performance.")]
    Task<bool> CheckPermissionsAsync(SettingPageCreationContext context);
}
