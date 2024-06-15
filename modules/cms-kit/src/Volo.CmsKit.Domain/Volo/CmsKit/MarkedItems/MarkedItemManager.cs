using System;
using JetBrains.Annotations;
using System.Threading.Tasks;
using Volo.Abp;

namespace Volo.CmsKit.MarkedItems;
public class MarkedItemManager : CmsKitDomainServiceBase
{
    IMarkedItemDefinitionStore MarkedItemDefinitionStore { get; set; }
    IUserMarkedItemRepository UserMarkedItemRepository { get; set; }
    public MarkedItemManager(
        IUserMarkedItemRepository userMarkedItemRepository,
        IMarkedItemDefinitionStore markedItemDefinitionStore)
    {
        UserMarkedItemRepository = userMarkedItemRepository;
        MarkedItemDefinitionStore = markedItemDefinitionStore;
    }

    public virtual async Task<bool> ToggleAsync(
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

    public virtual async Task<MarkedItemEntityTypeDefinition> GetAsync(
        [NotNull] string entityType)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType));

        return await MarkedItemDefinitionStore.GetAsync(entityType);
    }
}
