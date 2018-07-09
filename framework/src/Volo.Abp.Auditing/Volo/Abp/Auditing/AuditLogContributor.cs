using System.Threading.Tasks;

namespace Volo.Abp.Auditing
{
    public abstract class AuditLogContributor
    {
        public abstract Task ContributeAsync(AuditLogContributionContext context);
    }
}