using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Users;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Reactions;

namespace Volo.CmsKit.Public.Reactions;

[RequiresGlobalFeature(typeof(ReactionsFeature))]
public class ReactionPublicAppService : CmsKitPublicAppServiceBase, IReactionPublicAppService
{
    protected IReactionDefinitionStore ReactionDefinitionStore { get; }

    protected IUserReactionRepository UserReactionRepository { get; }

    protected ReactionManager ReactionManager { get; }

    public ReactionPublicAppService(
        IReactionDefinitionStore reactionDefinitionStore,
        IUserReactionRepository userReactionRepository,
        ReactionManager reactionManager)
    {
        ReactionDefinitionStore = reactionDefinitionStore;
        UserReactionRepository = userReactionRepository;
        ReactionManager = reactionManager;
    }

    public virtual async Task<ListResultDto<ReactionWithSelectionDto>> GetForSelectionAsync(string entityType, string entityId)
    {
        var summaries = await ReactionManager.GetSummariesAsync(entityType, entityId);

        var userReactionsOrNull = CurrentUser.IsAuthenticated
            ? (await UserReactionRepository
                .GetListForUserAsync(
                    CurrentUser.GetId(),
                    entityType,
                    entityId
                )).ToDictionary(x => x.ReactionName, x => x)
            : null;

        var reactionWithSelectionDtos = new List<ReactionWithSelectionDto>();

        foreach (var summary in summaries)
        {
            reactionWithSelectionDtos.Add(
                new ReactionWithSelectionDto
                {
                    Reaction = ConvertToReactionDto(summary.Reaction),
                    Count = summary.Count,
                    IsSelectedByCurrentUser = userReactionsOrNull?.ContainsKey(summary.Reaction.Name) ?? false
                }
            );
        }

        return new ListResultDto<ReactionWithSelectionDto>(reactionWithSelectionDtos);
    }

    [Authorize]
    public virtual async Task CreateAsync(string entityType, string entityId, string reaction)
    {
        await ReactionManager.GetOrCreateAsync(
            CurrentUser.GetId(),
            entityType,
            entityId,
            reaction
        );
    }

    [Authorize]
    public virtual async Task DeleteAsync(string entityType, string entityId, string reaction)
    {
        await ReactionManager.DeleteAsync(
            CurrentUser.GetId(),
            entityType,
            entityId,
            reaction
        );
    }

    private ReactionDto ConvertToReactionDto(ReactionDefinition reactionDefinition)
    {
        return new ReactionDto
        {
            Name = reactionDefinition.Name,
            DisplayName = reactionDefinition.DisplayName?.Localize(StringLocalizerFactory)
        };
    }
}
