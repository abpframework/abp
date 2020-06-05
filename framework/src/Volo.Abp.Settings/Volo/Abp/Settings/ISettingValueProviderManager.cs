using System.Collections.Generic;

namespace Volo.Abp.Settings
{
    public interface ISettingValueProviderManager
    {
        List<ISettingValueProvider> Providers { get; }
    }
}