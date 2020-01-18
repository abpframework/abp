﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.MongoDB;
using System.Linq.Dynamic.Core;

namespace Volo.Abp.IdentityServer.MongoDB
{
    public class MongoIdentityResourceRepository : MongoDbRepository<IAbpIdentityServerMongoDbContext, IdentityResource, Guid>, IIdentityResourceRepository
    {
        public MongoIdentityResourceRepository(IMongoDbContextProvider<IAbpIdentityServerMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<List<IdentityResource>> GetListAsync(string sorting, int skipCount, int maxResultCount, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .OrderBy(sorting ?? nameof(IdentityResource.Name))
                .As<IMongoQueryable<IdentityResource>>()
                .PageBy<IdentityResource, IMongoQueryable<IdentityResource>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public async Task<IdentityResource> FindByNameAsync(
            string name,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(x => x.Name == name)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public async Task<List<IdentityResource>> GetListByScopesAsync(string[] scopeNames, bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(ar => scopeNames.Contains(ar.Name))
                .ToListAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public virtual async Task<long> GetTotalCountAsync()
        {
            return await GetCountAsync().ConfigureAwait(false);
        }

        public async Task<bool> CheckNameExistAsync(string name, Guid? expectedId = null, CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable().AnyAsync(ir => ir.Id != expectedId && ir.Name == name, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}
