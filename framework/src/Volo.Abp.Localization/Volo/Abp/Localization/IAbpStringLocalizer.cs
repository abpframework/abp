using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

public interface IAbpStringLocalizer : IStringLocalizer
{
    IEnumerable<LocalizedString> GetAllStrings(
        bool includeParentCultures,
        bool includeBaseLocalizers
    );
}
