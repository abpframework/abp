using System.Threading.Tasks;

namespace Volo.Abp.Auditing
{
    public interface IAuditInfoContributor
    {
        Task Contribute(IAuditInfoContributionContext context);
    }
}
