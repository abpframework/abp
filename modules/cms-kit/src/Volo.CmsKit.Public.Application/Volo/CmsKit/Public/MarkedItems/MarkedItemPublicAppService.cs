using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Users;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.MarkedItems;

namespace Volo.CmsKit.Public.MarkedItems;

[RequiresGlobalFeature(typeof(MarkedItemsFeature))]
public class MarkedItemPublicAppService : CmsKitPublicAppServiceBase, IMarkedItemPublicAppService
{
    public IMarkedItemDefinitionStore MarkedItemDefinitionStore { get; }

    public IUserMarkedItemRepository UserMarkedItemRepository { get; }

    public MarkedItemManager MarkedItemManager { get; }

    public MarkedItemPublicAppService(
        IMarkedItemDefinitionStore markedItemDefinitionStore,
        IUserMarkedItemRepository userMarkedItemRepository,
        MarkedItemManager markedItemManager)
    {
        MarkedItemDefinitionStore = markedItemDefinitionStore;
        UserMarkedItemRepository = userMarkedItemRepository;
        MarkedItemManager = markedItemManager;
    }

    [AllowAnonymous]
    public virtual async Task<MarkedItemWithToggleDto> GetForToggleAsync(string entityType, string entityId)
    {
        var markedItem = await MarkedItemManager.GetAsync(entityType);

        var userMarkedItem = CurrentUser.IsAuthenticated
            ? (await UserMarkedItemRepository
                .FindAsync(
                    CurrentUser.GetId(),
                    entityType,
                    entityId
                ))
            : null;

        return new MarkedItemWithToggleDto
        {
            MarkedItem = ConvertToMarkedItemDto(markedItem),
            IsMarkedByCurrentUser = userMarkedItem != null
        };
    }

    [Authorize]
    public virtual async Task<bool> ToggleAsync(string entityType, string entityId)
    {
        return await MarkedItemManager.ToggleAsync(
            CurrentUser.GetId(),
            entityType,
            entityId
        );
    }

    private MarkedItemDto ConvertToMarkedItemDto(MarkedItemEntityTypeDefinition markedItemEntityTypeDefinition)
    {
        return new MarkedItemDto
        {
            IconName = markedItemEntityTypeDefinition.IconName
        };
    }

}
