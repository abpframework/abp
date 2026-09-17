using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

public interface IRouteBasedCultureNavigationHelper
{
    Task NavigateToNewCultureAsync(
        NavigationManager navigationManager,
        LanguageInfo newLanguage,
        IEnumerable<LanguageInfo> allLanguages);
}
