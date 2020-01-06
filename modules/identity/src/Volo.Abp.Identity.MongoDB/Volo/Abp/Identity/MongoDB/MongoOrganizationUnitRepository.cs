using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.Guids;
using Volo.Abp.Identity.Organizations;
using Volo.Abp.MongoDB;

namespace Volo.Abp.Identity.MongoDB
{
    public class MongoOrganizationUnitRepository : MongoDbRepository<IAbpIdentityMongoDbContext, OrganizationUnit, Guid>, IOrganizationUnitRepository
    {
        private readonly IGuidGenerator _guidGenerator;

        public MongoOrganizationUnitRepository(
            IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider,
            IGuidGenerator guidGenerator)
            : base(dbContextProvider)
        {
            _guidGenerator = guidGenerator;
        }

        public async Task<List<OrganizationUnit>> GetChildrenAsync(Guid? parentId, CancellationToken cancellationToken = default)
        {
            return await DbContext.OrganizationUnits.AsQueryable().Where(ou => ou.ParentId == parentId)
                    .ToListAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public async Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(string code, Guid? parentId, CancellationToken cancellationToken = default)
        {
            return await DbContext.OrganizationUnits.AsQueryable()
                    .Where(ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value)
                    .ToListAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public async Task<List<OrganizationUnit>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await DbContext.OrganizationUnits.AsQueryable()
                    .Where(t => ids.Contains(t.Id)).ToListAsync(GetCancellationToken(cancellationToken)).ConfigureAwait(false);
        }

        public async Task<OrganizationUnit> GetOrganizationUnit(string displayName, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await DbContext.OrganizationUnits.AsQueryable()
                    .FirstOrDefaultAsync(
                        ou => ou.DisplayName == displayName,
                        GetCancellationToken(cancellationToken)
                    ).ConfigureAwait(false);
        }
    }
}
