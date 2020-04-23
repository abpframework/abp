using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.TextTemplating
{
    public interface ITemplateContributor
    {
        void Initialize(TemplateContributorInitializationContext context);

        Task<string> GetOrNullAsync([CanBeNull] string cultureName);
    }
}