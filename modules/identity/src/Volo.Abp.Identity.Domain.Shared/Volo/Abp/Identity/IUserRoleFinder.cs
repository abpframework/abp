using System;
using System.Threading.Tasks;

namespace Volo.Abp.Identity
{
    public interface IUserRoleFinder
    {
        Task<string[]> GetRolesAsync(Guid userId);
    }
}
