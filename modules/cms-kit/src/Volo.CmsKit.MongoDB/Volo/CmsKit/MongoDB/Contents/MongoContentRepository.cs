﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.MongoDB.Contents
{
    public class MongoContentRepository : MongoDbRepository<ICmsKitMongoDbContext, Content, Guid>, IContentRepository
    {
        public MongoContentRepository(IMongoDbContextProvider<ICmsKitMongoDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Task<Content> GetAsync(
            string entityType,
            string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            return GetAsync(x =>
                    !x.IsDeleted &&
                    x.EntityType == entityType &&
                    x.EntityId == entityId &&
                    x.TenantId == tenantId,
                cancellationToken: GetCancellationToken(cancellationToken)
            );
        }
        
        public Task<Content> FindAsync(
            string entityType,
            string entityId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            return FindAsync(x =>
                    !x.IsDeleted &&
                    x.EntityType == entityType &&
                    x.EntityId == entityId &&
                    x.TenantId == tenantId,
                cancellationToken: GetCancellationToken(cancellationToken)
                );
        }

        public Task DeleteAsync(string entityType, string entityId, Guid? tenantId = null, CancellationToken cancellationToken = default)
        {
            return DeleteAsync(x =>
                    x.EntityType == entityType &&
                    x.EntityId == entityId &&
                    x.TenantId == tenantId,
                cancellationToken: GetCancellationToken(cancellationToken));
        }
    }
}