using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.StaticDefinitions;
using Volo.Abp.TextTemplating;
using Volo.Abp.Threading;

namespace Volo.Abp.TextTemplateManagement;

public class StaticTemplateDefinitionChangedEventHandler : ILocalEventHandler<StaticTemplateDefinitionChangedEvent>, ITransientDependency
{
    protected IStaticDefinitionCache<TemplateDefinition, Dictionary<string, TemplateDefinition>> DefinitionCache { get; }
    protected TextTemplateDynamicInitializer TextTemplateDynamicInitializer { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public StaticTemplateDefinitionChangedEventHandler(
        IStaticDefinitionCache<TemplateDefinition, Dictionary<string, TemplateDefinition>> definitionCache,
        TextTemplateDynamicInitializer textTemplateDynamicInitializer,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        DefinitionCache = definitionCache;
        TextTemplateDynamicInitializer = textTemplateDynamicInitializer;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public virtual async Task HandleEventAsync(StaticTemplateDefinitionChangedEvent eventData)
    {
        await DefinitionCache.ClearAsync();
        await TextTemplateDynamicInitializer.InitializeAsync(false, CancellationTokenProvider.Token);
    }
}
