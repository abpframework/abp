using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Users.MongoDB
{
    public abstract class MongoUserRepositoryBase<TDbContext, TUser> : MongoDbRepository<TDbContext, TUser, Guid>, IUserRepository<TUser>
        where TDbContext : IAbpMongoDbContext
        where TUser : class, IUser
    {
        protected MongoUserRepositoryBase(IMongoDbContextProvider<TDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public virtual async Task<TUser> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
        }

        public virtual async Task<List<TUser>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(u => ids.Contains(u.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TUser>> SearchAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .WhereIf<TUser, IMongoQueryable<TUser>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        u.Name.Contains(filter) ||
                        u.Surname.Contains(filter)
                )
                .OrderBy(sorting.IsNullOrEmpty() ? nameof(IUserData.UserName) : sorting)
                .As<IMongoQueryable<TUser>>()
                .PageBy<TUser, IMongoQueryable<TUser>>(skipCount, maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(string filter = null, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .WhereIf<TUser, IMongoQueryable<TUser>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        u.Name.Contains(filter) ||
                        u.Surname.Contains(filter)
                )
                .LongCountAsync(cancellationToken);
        }
    }
}
