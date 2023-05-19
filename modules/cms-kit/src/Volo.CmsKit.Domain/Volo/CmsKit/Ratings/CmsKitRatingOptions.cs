using JetBrains.Annotations;
using System.Collections.Generic;

namespace Volo.CmsKit.Ratings;

public class CmsKitRatingOptions
{
    [NotNull]
    public List<RatingEntityTypeDefinition> EntityTypes { get; } = new();
}
