using System.Threading.Tasks;

namespace Volo.Abp.TextTemplating
{
    public interface ITemplateContentContributor
    {
        Task<string> GetOrNullAsync(TemplateContentContributorContext context);
    }
}