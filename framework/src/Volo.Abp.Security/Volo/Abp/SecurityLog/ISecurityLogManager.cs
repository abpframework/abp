using System.Threading.Tasks;

namespace Volo.Abp.SecurityLog
{
    public interface ISecurityLogManager
    {
        Task<SecurityLogInfo> CreateAsync();

        Task SaveAsync(SecurityLogInfo securityLogInfo);
    }
}
