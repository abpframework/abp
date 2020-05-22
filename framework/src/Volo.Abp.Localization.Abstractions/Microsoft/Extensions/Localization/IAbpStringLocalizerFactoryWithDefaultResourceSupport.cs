using JetBrains.Annotations;

namespace Microsoft.Extensions.Localization
{
    public interface IAbpStringLocalizerFactoryWithDefaultResourceSupport
    {
        [CanBeNull]
        IStringLocalizer CreateDefaultOrNull();
    }
}