using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings;

public class SettingDefinitionManager : ISettingDefinitionManager, ISingletonDependency
{
    protected readonly IStaticSettingDefinitionStore StaticStore;
    protected readonly IDynamicSettingDefinitionStore DynamicStore;

    public SettingDefinitionManager(IStaticSettingDefinitionStore staticStore, IDynamicSettingDefinitionStore dynamicStore)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
    }

    public virtual async Task<SettingDefinition> GetAsync(string name)
    {
        var permission = await GetOrNullAsync(name);
        if (permission == null)
        {
            throw new AbpException("Undefined setting: " + name);
        }

        return permission;
    }

    public virtual async Task<SettingDefinition> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        return await StaticStore.GetOrNullAsync(name) ?? await DynamicStore.GetOrNullAsync(name);
    }

    public virtual async Task<IReadOnlyList<SettingDefinition>> GetAllAsync()
    {
        var staticTemplates = await StaticStore.GetAllAsync();
        var staticTemplateNames = staticTemplates
            .Select(p => p.Name)
            .ToImmutableHashSet();

        var dynamicTemplates = await DynamicStore.GetAllAsync();

        /* We prefer static Templates over dynamics */
        return staticTemplates.Concat(dynamicTemplates.Where(d => !staticTemplateNames.Contains(d.Name))).ToImmutableList();
    }
}
