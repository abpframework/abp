using System.Threading.Tasks;

namespace Volo.Abp.SecurityLog
{
    public interface ISecurityLogStore
    {
        Task SaveAsync(SecurityLogInfo securityLogInfo);
    }
}
