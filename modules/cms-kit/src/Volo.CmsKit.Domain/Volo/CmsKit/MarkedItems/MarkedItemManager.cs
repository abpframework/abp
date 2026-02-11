using System;
using JetBrains.Annotations;
using System.Threading.Tasks;
using Volo.Abp;
using System.Collections.Generic;
using System.Threading;

namespace Volo.CmsKit.MarkedItems;
public class MarkedItemManager : CmsKitDomainServiceBase
{
    protected IMarkedItemDefinitionStore MarkedItemDefinitionStore { get; set; }

    protected IUserMarkedItemRepository UserMarkedItemRepository { get; set; }

    public MarkedItemManager(
        IUserMarkedItemRepository userMarkedItemRepository,
        IMarkedItemDefinitionStore markedItemDefinitionStore)
    {
        UserMarkedItemRepository = userMarkedItemRepository;
        MarkedItemDefinitionStore = markedItemDefinitionStore;
    }

    public virtual async Task<bool> ToggleUserMarkedItemAsync(
        Guid creatorId,
        [NotNull] string entityType,
        [NotNull] string entityId)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
        Check.NotNullOrWhiteSpace(entityId, nameof(entityId));

        var markedItem = await UserMarkedItemRepository.FindAsync(creatorId, entityType, entityId);
        if (markedItem != null)
        {
            await UserMarkedItemRepository.DeleteAsync(markedItem);
            return false;
        }

        if (!await MarkedItemDefinitionStore.IsDefinedAsync(entityType))
        {
            throw new EntityCannotBeMarkedException(entityType);
        }

        await UserMarkedItemRepository.InsertAsync(
            new UserMarkedItem(
                GuidGenerator.Create(),
                entityType,
                entityId,
                creatorId,
                CurrentTenant.Id
                )
            );
        return true;
    }
    public async Task<List<string>> GetEntityIdsFilteredByUserAsync(
       [NotNull] Guid userId,
       [NotNull] string entityType,
       [CanBeNull] Guid? tenantId = null,
       CancellationToken cancellationToken = default)
    {
        return await UserMarkedItemRepository.GetEntityIdsFilteredByUserAsync(userId, entityType, tenantId, cancellationToken);
    }
}
