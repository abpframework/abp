using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TextTemplating;

public class TemplateDefinitionManager : ITemplateDefinitionManager, ISingletonDependency
{
    protected readonly IStaticTemplateDefinitionStore StaticStore;
    protected readonly IDynamicTemplateDefinitionStore DynamicStore;

    public TemplateDefinitionManager(IStaticTemplateDefinitionStore staticStore, IDynamicTemplateDefinitionStore dynamicStore)
    {
        StaticStore = staticStore;
        DynamicStore = dynamicStore;
    }

    public virtual async Task<TemplateDefinition> GetAsync(string name)
    {
        var permission = await GetOrNullAsync(name);
        if (permission == null)
        {
            throw new AbpException("Undefined Template: " + name);
        }

        return permission;
    }

    public virtual async Task<TemplateDefinition?> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        return await StaticStore.GetOrNullAsync(name) ?? await DynamicStore.GetOrNullAsync(name);
    }

    public virtual async Task<IReadOnlyList<TemplateDefinition>> GetAllAsync()
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
