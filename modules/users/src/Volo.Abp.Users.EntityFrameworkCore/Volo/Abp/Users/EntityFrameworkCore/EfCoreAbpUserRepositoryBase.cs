using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.Users.EntityFrameworkCore
{
    public abstract class EfCoreUserRepositoryBase<TDbContext, TUser> : EfCoreRepository<TDbContext, TUser, Guid>, IUserRepository<TUser>
        where TDbContext : IEfCoreDbContext
        where TUser : class, IUser
    {
        protected EfCoreUserRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<TUser> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return await this.FirstOrDefaultAsync(u => u.UserName == userName, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<TUser>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(u => ids.Contains(u.Id)).ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}