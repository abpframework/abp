using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    public interface IQueryStringCultureReplacement
    {
        Task<string> ReplaceAsync(QueryStringCultureReplacementContext context);
    }
}
