using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Authorization.Fluent;

public static class FluentAuthorizationConditionsExtensions
{
    public static void IsOwner(this IFluentAuthorizationConditions auth, IMayHaveOwner owner)
    {
        auth.IsGranted(null, new UserRequirement(owner.OwnerId));
    }

    public static void IsOwner(this IFluentAuthorizationConditions auth, IMustHaveOwner owner)
    {
        auth.IsGranted(null, new UserRequirement(owner.OwnerId));
    }

    public static void IsCreator(this IFluentAuthorizationConditions auth, IMayHaveCreator creator)
    {
        auth.IsGranted(null, new UserRequirement(creator.CreatorId));
    }

    public static void IsCreator(this IFluentAuthorizationConditions auth, IMustHaveCreator creator)
    {
        auth.IsGranted(null, new UserRequirement(creator.CreatorId));
    }
}