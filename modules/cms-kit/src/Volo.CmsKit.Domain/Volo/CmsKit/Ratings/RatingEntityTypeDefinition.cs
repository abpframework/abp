using JetBrains.Annotations;

namespace Volo.CmsKit.Ratings
{
    public class RatingEntityTypeDefinition : EntityTypeDefinition
    {
        public RatingEntityTypeDefinition(
            [NotNull] string entityType) : base(entityType)
        {
        }
    }
}
