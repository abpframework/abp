using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.StaticDefinitions;
using Volo.Abp.Threading;

namespace Volo.Abp.PermissionManagement;

public class StaticPermissionDefinitionChangedEventHandler : ILocalEventHandler<StaticPermissionDefinitionChangedEvent>, ITransientDependency
{
    protected IStaticDefinitionCache<PermissionGroupDefinition, (Dictionary<string, PermissionGroupDefinition>, List<PermissionDefinition>)> GroupCache { get; }
    protected IStaticDefinitionCache<PermissionDefinition, Dictionary<string, PermissionDefinition>> DefinitionCache { get; }
    protected PermissionDynamicInitializer PermissionDynamicInitializer { get; }
    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public StaticPermissionDefinitionChangedEventHandler(
        IStaticDefinitionCache<PermissionGroupDefinition, (Dictionary<string, PermissionGroupDefinition>, List<PermissionDefinition>)> groupCache,
        IStaticDefinitionCache<PermissionDefinition, Dictionary<string, PermissionDefinition>> definitionCache,
        PermissionDynamicInitializer permissionDynamicInitializer,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        GroupCache = groupCache;
        DefinitionCache = definitionCache;
        PermissionDynamicInitializer = permissionDynamicInitializer;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    public virtual async Task HandleEventAsync(StaticPermissionDefinitionChangedEvent eventData)
    {
        await GroupCache.ClearAsync();
        await DefinitionCache.ClearAsync();
        await PermissionDynamicInitializer.InitializeAsync(false, CancellationTokenProvider.Token);
    }
}
