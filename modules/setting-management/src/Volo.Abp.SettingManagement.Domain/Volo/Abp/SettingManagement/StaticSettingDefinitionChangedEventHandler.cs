using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Settings;
using Volo.Abp.StaticDefinitions;
using Volo.Abp.Threading;

namespace Volo.Abp.SettingManagement;

public class StaticSettingDefinitionChangedEventHandler : ILocalEventHandler<StaticSettingDefinitionChangedEvent>, ITransientDependency
{
    protected IStaticDefinitionCache<SettingDefinition, Dictionary<string, SettingDefinition>> DefinitionCache { get; }
    protected SettingDynamicInitializer SettingDynamicInitializer { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public StaticSettingDefinitionChangedEventHandler(
        IStaticDefinitionCache<SettingDefinition, Dictionary<string, SettingDefinition>> definitionCache,
        SettingDynamicInitializer settingDynamicInitializer,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        DefinitionCache = definitionCache;
        SettingDynamicInitializer = settingDynamicInitializer;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public virtual async Task HandleEventAsync(StaticSettingDefinitionChangedEvent eventData)
    {
        await DefinitionCache.ClearAsync();
        await SettingDynamicInitializer.InitializeAsync(false, CancellationTokenProvider.Token);
    }
}
