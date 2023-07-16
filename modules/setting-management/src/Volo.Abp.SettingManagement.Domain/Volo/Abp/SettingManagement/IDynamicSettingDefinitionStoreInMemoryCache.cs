using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement;

public interface IDynamicSettingDefinitionStoreInMemoryCache
{
    string CacheStamp { get; set; }

    SemaphoreSlim SyncSemaphore { get; }

    DateTime? LastCheckTime { get; set; }

    Task FillAsync(List<SettingDefinitionRecord> settingRecords);

    SettingDefinition GetSettingOrNull(string name);

    IReadOnlyList<SettingDefinition> GetSettings();
}
