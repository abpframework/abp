using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

public interface IAsyncLocalizableString
{
    Task<LocalizedString> LocalizeAsync(IStringLocalizerFactory stringLocalizerFactory);
}