using System.Threading.Tasks;
using Microsoft.AspNetCore.Localization;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    public interface IQueryStringCultureReplacement
    {
        Task<string> ReplaceAsync(string returnUrl, RequestCulture requestCulture);
    }
}
