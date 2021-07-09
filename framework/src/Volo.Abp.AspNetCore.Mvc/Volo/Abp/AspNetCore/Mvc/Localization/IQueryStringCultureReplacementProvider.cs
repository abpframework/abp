using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.Localization
{
    public interface IQueryStringCultureReplacementProvider
    {
        Task ReplaceAsync(QueryStringCultureReplacementContext context);
    }
}
