using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Localization
{
    public interface ILanguageProvider
    {
        Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync();
    }
}
