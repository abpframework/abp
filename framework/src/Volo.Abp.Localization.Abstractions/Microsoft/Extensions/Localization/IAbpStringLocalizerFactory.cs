using JetBrains.Annotations;

namespace Microsoft.Extensions.Localization;

public interface IAbpStringLocalizerFactory
{
    [CanBeNull]
    IStringLocalizer CreateDefaultOrNull();

    [CanBeNull]
    IStringLocalizer CreateByResourceNameOrNull([NotNull] string resourceName);
}
