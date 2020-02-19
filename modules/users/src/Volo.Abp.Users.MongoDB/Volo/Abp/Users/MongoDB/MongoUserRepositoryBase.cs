using System;
using System.Collections.Generic;
using System.Linq;
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
            return await GetMongoQueryable().FirstOrDefaultAsync(u => u.UserName == userName, GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<TUser>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().Where(u => ids.Contains(u.Id)).ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}