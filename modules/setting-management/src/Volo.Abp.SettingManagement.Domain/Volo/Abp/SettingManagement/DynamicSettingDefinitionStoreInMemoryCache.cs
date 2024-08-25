using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement;

public class DynamicSettingDefinitionStoreInMemoryCache : IDynamicSettingDefinitionStoreInMemoryCache, ISingletonDependency
{
    public string CacheStamp { get; set; }

    protected IDictionary<string, SettingDefinition> SettingDefinitions { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public SemaphoreSlim SyncSemaphore { get; } = new(1, 1);

    public DateTime? LastCheckTime { get; set; }

    public DynamicSettingDefinitionStoreInMemoryCache(ILocalizableStringSerializer localizableStringSerializer)
    {
        LocalizableStringSerializer = localizableStringSerializer;
        SettingDefinitions = new Dictionary<string, SettingDefinition>();
    }

    public Task FillAsync(List<SettingDefinitionRecord> settingRecords)
    {
        SettingDefinitions.Clear();

        foreach (var record in settingRecords)
        {
            var settingDefinition = new SettingDefinition(
                record.Name,
                record.DefaultValue,
                LocalizableStringSerializer.Deserialize(record.DisplayName),
                record.Description != null ? LocalizableStringSerializer.Deserialize(record.Description) : null,
                record.IsVisibleToClients,
                record.IsInherited,
                record.IsEncrypted);

            if (!record.Providers.IsNullOrWhiteSpace())
            {
                settingDefinition.Providers.AddRange(record.Providers.Split(','));
            }

            foreach (var property in record.ExtraProperties)
            {
                settingDefinition.WithProperty(property.Key, property.Value);
            }

            SettingDefinitions[record.Name] = settingDefinition;
        }

        return Task.CompletedTask;
    }

    public SettingDefinition GetSettingOrNull(string name)
    {
        return SettingDefinitions.GetOrDefault(name);
    }

    public IReadOnlyList<SettingDefinition> GetSettings()
    {
        return SettingDefinitions.Values.ToList();
    }
}
