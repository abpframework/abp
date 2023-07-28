using Volo.Abp.Collections;

namespace Volo.Abp.SettingManagement;

public class SettingManagementOptions
{
    public ITypeList<ISettingManagementProvider> Providers { get; }

    /// <summary>
    /// Default: true.
    /// </summary>
    public bool SaveStaticSettingsToDatabase { get; set; } = true;

    /// <summary>
    /// Default: false.
    /// </summary>
    public bool IsDynamicSettingStoreEnabled { get; set; }

    public SettingManagementOptions()
    {
        Providers = new TypeList<ISettingManagementProvider>();
    }
}
