using System;
using JetBrains.Annotations;

namespace Volo.Abp.Http.Modeling;

public class PropertyApiDescriptionModelContributionContext
{
    [NotNull]
    public PropertyApiDescriptionModel Model { get; }

    [NotNull]
    public Type DeclaringType { get; }

    public PropertyApiDescriptionModelContributionContext(
        [NotNull] PropertyApiDescriptionModel model,
        [NotNull] Type declaringType)
    {
        Model = Check.NotNull(model, nameof(model));
        DeclaringType = Check.NotNull(declaringType, nameof(declaringType));
    }
}
