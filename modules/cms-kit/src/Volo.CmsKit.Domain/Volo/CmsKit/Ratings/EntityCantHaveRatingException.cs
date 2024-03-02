using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.Ratings;

public class EntityCantHaveRatingException : BusinessException
{
    public EntityCantHaveRatingException([NotNull] string entityType)
    {
        Code = CmsKitErrorCodes.Ratings.EntityCantHaveRating;
        EntityType = Check.NotNullOrEmpty(entityType, nameof(entityType));
        WithData(nameof(EntityType), EntityType);
    }

    public string EntityType { get; }
}
