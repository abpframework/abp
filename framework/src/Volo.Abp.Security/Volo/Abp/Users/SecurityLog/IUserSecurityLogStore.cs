using System.Threading.Tasks;

namespace Volo.Abp.Users.SecurityLog
{
    public interface IUserSecurityLogStore
    {
        Task SaveAsync(UserSecurityLogInfo userSecurityLogInfo);
    }
}
