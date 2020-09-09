﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public class MongoIdentityClaimTypeRepository : MongoDbRepository<IAbpIdentityMongoDbContext, IdentityClaimType, Guid>, IIdentityClaimTypeRepository
    {
        public MongoIdentityClaimTypeRepository(IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> AnyAsync(
            string name,
            Guid? ignoredId = null,
            CancellationToken cancellationToken = default)
        {
            if (ignoredId == null)
            {
                return await GetMongoQueryable()
                    .Where(ct => ct.Name == name)
                    .AnyAsync(GetCancellationToken(cancellationToken));
            }
            else
            {
                return await GetMongoQueryable()
                    .Where(ct => ct.Id != ignoredId && ct.Name == name)
                    .AnyAsync(GetCancellationToken(cancellationToken));
            }
        }

        public virtual async Task<List<IdentityClaimType>> GetListAsync(
            string sorting,
            int maxResultCount,
            int skipCount,
            string filter,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .WhereIf<IdentityClaimType, IMongoQueryable<IdentityClaimType>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(filter)
                )
                .OrderBy(sorting ?? nameof(IdentityClaimType.Name))
                .As<IMongoQueryable<IdentityClaimType>>()
                .PageBy<IdentityClaimType, IMongoQueryable<IdentityClaimType>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .WhereIf<IdentityClaimType, IMongoQueryable<IdentityClaimType>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(filter)
                )
                .As<IMongoQueryable<IdentityClaimType>>()
                .LongCountAsync(GetCancellationToken(cancellationToken));
        }
    }
}
