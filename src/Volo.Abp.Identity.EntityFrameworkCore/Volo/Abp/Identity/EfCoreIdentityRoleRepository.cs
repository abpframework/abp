using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;

namespace Volo.Abp.Identity
{
    //TODO: Register for all repositories!
    public class EfCoreIdentityRoleRepository : EfCoreRepository<IdentityDbContext, IdentityRole>, IIdentityRoleRepository
    {
        public EfCoreIdentityRoleRepository(IDbContextProvider<IdentityDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public Task<IdentityRole> FindByNormalizedNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return DbSet.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
        }
    }
}