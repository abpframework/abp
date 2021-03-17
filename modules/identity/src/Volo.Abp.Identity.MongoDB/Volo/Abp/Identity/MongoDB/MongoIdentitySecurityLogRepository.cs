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

namespace Volo.Abp.Identity.MongoDB
{
    public class MongoIdentitySecurityLogRepository :
        MongoDbRepository<IAbpIdentityMongoDbContext, IdentitySecurityLog, Guid>, IIdentitySecurityLogRepository
    {
        public MongoIdentitySecurityLogRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<IdentitySecurityLog>> GetListAsync(
            string sorting = null,
            int maxResultCount = 50,
            int skipCount = 0,
            DateTime? startTime = null,
            DateTime? endTime = null,
            string applicationName = null,
            string identity = null,
            string action = null,
            Guid? userId = null,
            string userName = null,
            string clientId = null,
            string correlationId = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = await GetListQueryAsync(
                startTime,
                endTime,
                applicationName,
                identity,
                action,
                userId,
                userName,
                clientId,
                correlationId,
                cancellationToken
            );

            return await query.OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(IdentitySecurityLog.CreationTime)} desc" : sorting)
                .As<IMongoQueryable<IdentitySecurityLog>>()
                .PageBy<IdentitySecurityLog, IMongoQueryable<IdentitySecurityLog>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<long> GetCountAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string applicationName = null,
            string identity = null,
            string action = null,
            Guid? userId = null,
            string userName = null,
            string clientId = null,
            string correlationId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetListQueryAsync(
                startTime,
                endTime,
                applicationName,
                identity,
                action,
                userId,
                userName,
                clientId,
                correlationId,
                cancellationToken
            );

            return await query.As<IMongoQueryable<IdentitySecurityLog>>()
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }


        public virtual async Task<IdentitySecurityLog> GetByUserIdAsync(Guid id, Guid userId, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await (await GetMongoQueryableAsync(cancellationToken)).OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId,
                GetCancellationToken(cancellationToken));
        }

        protected virtual async Task<IQueryable<IdentitySecurityLog>> GetListQueryAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            string applicationName = null,
            string identity = null,
            string action = null,
            Guid? userId = null,
            string userName = null,
            string clientId = null,
            string correlationId = null,
            CancellationToken cancellationToken = default)
        {
            return (await GetMongoQueryableAsync(cancellationToken))
                .WhereIf(startTime.HasValue, securityLog => securityLog.CreationTime >= startTime.Value)
                .WhereIf(endTime.HasValue, securityLog => securityLog.CreationTime < endTime.Value.AddDays(1).Date)
                .WhereIf(!applicationName.IsNullOrWhiteSpace(),
                    securityLog => securityLog.ApplicationName == applicationName)
                .WhereIf(!identity.IsNullOrWhiteSpace(), securityLog => securityLog.Identity == identity)
                .WhereIf(!action.IsNullOrWhiteSpace(), securityLog => securityLog.Action == action)
                .WhereIf(userId.HasValue, securityLog => securityLog.UserId == userId)
                .WhereIf(!userName.IsNullOrWhiteSpace(), securityLog => securityLog.UserName == userName)
                .WhereIf(!clientId.IsNullOrWhiteSpace(), securityLog => securityLog.ClientId == clientId)
                .WhereIf(!correlationId.IsNullOrWhiteSpace(),
                    securityLog => securityLog.CorrelationId == correlationId);
        }
    }
}
