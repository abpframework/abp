using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

public class FixedLocalizableString : ILocalizableString
{
    public string Value { get; }

    public FixedLocalizableString(string value)
    {
        Check.NotNull(value, nameof(value));
        Value = value;
    }

    public LocalizedString Localize(IStringLocalizerFactory stringLocalizerFactory)
    {
        return new LocalizedString(Value, Value);
    }
}
