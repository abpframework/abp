using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
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
            return await this.OrderBy(x => x.Id).FirstOrDefaultAsync(u => u.UserName == userName, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<TUser>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await DbSet.Where(u => ids.Contains(u.Id)).ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<TUser>> SearchAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        u.Name.Contains(filter) ||
                        u.Surname.Contains(filter)
                )
                .OrderBy(sorting ?? nameof(IUser.UserName))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        u.Name.Contains(filter) ||
                        u.Surname.Contains(filter)
                )
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }
    }
}
