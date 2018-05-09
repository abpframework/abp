using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Threading;

namespace Volo.Abp.Settings
{
    public static class SettingManagerSyncExtensions
    {
        public static string GetOrNull([NotNull] this ISettingManager settingManager, [NotNull] string name)
        {
            Check.NotNull(settingManager, nameof(settingManager));

            return AsyncHelper.RunSync(() => settingManager.GetOrNullAsync(name));
        }

        public static List<SettingValue> GetAll([NotNull] this ISettingManager settingManager)
        {
            Check.NotNull(settingManager, nameof(settingManager));

            return AsyncHelper.RunSync(settingManager.GetAllAsync);
        }
    }
}
