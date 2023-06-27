using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement;

public class SettingDefinitionSerializer : ISettingDefinitionSerializer, ITransientDependency
{
    protected IGuidGenerator GuidGenerator { get; }
    protected ILocalizableStringSerializer LocalizableStringSerializer { get; }

    public SettingDefinitionSerializer(IGuidGenerator guidGenerator, ILocalizableStringSerializer localizableStringSerializer)
    {
        GuidGenerator = guidGenerator;
        LocalizableStringSerializer = localizableStringSerializer;
    }

    public virtual Task<SettingDefinitionRecord> SerializeAsync(SettingDefinition setting)
    {
        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            var record = new SettingDefinitionRecord(
                GuidGenerator.Create(),
                setting.Name,
                LocalizableStringSerializer.Serialize(setting.DisplayName),
                LocalizableStringSerializer.Serialize(setting.Description),
                setting.DefaultValue,
                setting.IsVisibleToClients,
                SerializeProviders(setting.Providers),
                setting.IsInherited,
                setting.IsEncrypted);

            foreach (var property in setting.Properties)
            {
                record.SetProperty(property.Key, property.Value);
            }

            return Task.FromResult(record);
        }
    }

    public virtual async Task<List<SettingDefinitionRecord>> SerializeAsync(IEnumerable<SettingDefinition> settings)
    {
        var records = new List<SettingDefinitionRecord>();
        foreach (var setting in settings)
        {
            records.Add(await SerializeAsync(setting));
        }

        return records;
    }

    protected virtual string SerializeProviders(ICollection<string> providers)
    {
        return providers.Any()
            ? providers.JoinAsString(",")
            : null;
    }
}
