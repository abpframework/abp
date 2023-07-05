using JetBrains.Annotations;

namespace Volo.Abp.Localization;

public interface IHasNameWithLocalizableDisplayName
{
    [NotNull]
    public string Name { get; }

    public ILocalizableString? DisplayName { get; }
}
