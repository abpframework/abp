using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Volo.Abp.Http.Modeling;

public class PropertyApiDescriptionModelContributionContext
{
    [NotNull]
    public PropertyApiDescriptionModel Model { get; }

    [NotNull]
    public PropertyInfo PropertyInfo { get; }

    [NotNull]
    public Type DeclaringType { get; }

    public PropertyApiDescriptionModelContributionContext(
        [NotNull] PropertyApiDescriptionModel model,
        [NotNull] PropertyInfo propertyInfo,
        [NotNull] Type declaringType)
    {
        Model = Check.NotNull(model, nameof(model));
        PropertyInfo = Check.NotNull(propertyInfo, nameof(propertyInfo));
        DeclaringType = Check.NotNull(declaringType, nameof(declaringType));
    }
}
