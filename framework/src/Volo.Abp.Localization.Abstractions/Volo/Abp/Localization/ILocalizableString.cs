using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization
{
    public interface ILocalizableString
    {
        LocalizedString Localize(IStringLocalizerFactory stringLocalizerFactory);
    }
}