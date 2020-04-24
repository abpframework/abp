using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating
{
    public interface ITemplateContentContributor
    {
        void Initialize(TemplateContentContributorInitializationContext context);

        Task<string> GetOrNullAsync([CanBeNull] string cultureName);
    }
}