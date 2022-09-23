using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

public interface IAbpStringLocalizer : IStringLocalizer
{
    IEnumerable<LocalizedString> GetAllStrings(
        bool includeParentCultures,
        bool includeBaseLocalizers,
        bool includeDynamicContributors
    );

    Task<IEnumerable<LocalizedString>> GetAllStringsAsync(
        bool includeParentCultures
    );
    
    Task<IEnumerable<LocalizedString>> GetAllStringsAsync(
        bool includeParentCultures,
        bool includeBaseLocalizers,
        bool includeDynamicContributors
    );

    Task<IEnumerable<string>> GetSupportedCulturesAsync();
}