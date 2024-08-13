using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Users;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.MarkedItems;
using Volo.CmsKit.Features;
using Volo.Abp.Features;

namespace Volo.CmsKit.Public.MarkedItems;
[RequiresFeature(CmsKitFeatures.MarkedItemEnable)]
[RequiresGlobalFeature(typeof(MarkedItemsFeature))]
public class MarkedItemPublicAppService : CmsKitPublicAppServiceBase, IMarkedItemPublicAppService
{
    protected IMarkedItemDefinitionStore MarkedItemDefinitionStore { get; }

    protected IUserMarkedItemRepository UserMarkedItemRepository { get; }

    protected MarkedItemManager MarkedItemManager { get; }

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
    public virtual async Task<MarkedItemWithToggleDto> GetForUserAsync(string entityType, string entityId)
    {
        var markedItem = await MarkedItemDefinitionStore.GetAsync(entityType);

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
        return await MarkedItemManager.ToggleUserMarkedItemAsync(
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
