﻿using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity.MongoDB
{
    public class MongoOrganizationUnitRepository
        : MongoDbRepository<IAbpIdentityMongoDbContext, OrganizationUnit, Guid>,
        IOrganizationUnitRepository
    {
        public MongoOrganizationUnitRepository(
            IMongoDbContextProvider<IAbpIdentityMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<List<OrganizationUnit>> GetChildrenAsync(
            Guid? parentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .Where(ou => ou.ParentId == parentId)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(
            string code,
            Guid? parentId,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                    .Where(ou => ou.Code.StartsWith(code) && ou.Id != parentId.Value)
                    .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OrganizationUnit>> GetListAsync(
            IEnumerable<Guid> ids,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                    .Where(t => ids.Contains(t.Id))
                    .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<List<OrganizationUnit>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                    .OrderBy(sorting ?? nameof(OrganizationUnit.DisplayName))
                    .As<IMongoQueryable<OrganizationUnit>>()
                    .PageBy<OrganizationUnit, IMongoQueryable<OrganizationUnit>>(skipCount, maxResultCount)
                    .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<OrganizationUnit> GetAsync(
            string displayName,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await GetMongoQueryable()
                .FirstOrDefaultAsync(
                    ou => ou.DisplayName == displayName,
                    GetCancellationToken(cancellationToken)
                );
        }

        public virtual async Task<List<IdentityRole>> GetRolesAsync(
            OrganizationUnit organizationUnit,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var roleIds = organizationUnit.Roles.Select(r => r.RoleId).ToArray();
            return await DbContext.Roles.AsQueryable().Where(r => roleIds.Contains(r.Id))
                .OrderBy(sorting ?? nameof(IdentityRole.Name))
                .As<IMongoQueryable<IdentityRole>>()
                .PageBy<IdentityRole, IMongoQueryable<IdentityRole>>(skipCount, maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<int> GetRolesCountAsync(
            OrganizationUnit organizationUnit,
            CancellationToken cancellationToken = default)
        {
            var roleIds = organizationUnit.Roles.Select(r => r.RoleId).ToArray();
            return await DbContext.Roles.AsQueryable().Where(r => roleIds.Contains(r.Id))
                .As<IMongoQueryable<IdentityRole>>()
                .CountAsync(cancellationToken);
        }

        public async Task<List<IdentityRole>> GetUnaddedRolesAsync(
            OrganizationUnit organizationUnit,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var roleIds = organizationUnit.Roles.Select(r => r.RoleId).ToArray();
            return await DbContext.Roles.AsQueryable()
                .Where(r => !roleIds.Contains(r.Id))
                .WhereIf(!filter.IsNullOrWhiteSpace(), r => r.Name.Contains(filter))
                .OrderBy(sorting ?? nameof(IdentityRole.Name))
                .As<IMongoQueryable<IdentityRole>>()
                .PageBy<IdentityRole, IMongoQueryable<IdentityRole>>(skipCount, maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetUnaddedRolesCountAsync(
            OrganizationUnit organizationUnit,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            var roleIds = organizationUnit.Roles.Select(r => r.RoleId).ToArray();
            return await DbContext.Roles.AsQueryable()
                .Where(r => !roleIds.Contains(r.Id))
                .WhereIf(!filter.IsNullOrWhiteSpace(), r => r.Name.Contains(filter))
                .As<IMongoQueryable<IdentityRole>>()
                .CountAsync(cancellationToken);
        }

        public virtual async Task<List<IdentityUser>> GetMembersAsync(
            OrganizationUnit organizationUnit,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = CreateGetMembersFilteredQuery(organizationUnit, filter);

            return await query
                .OrderBy(sorting ?? nameof(IdentityUser.UserName))
                .As<IMongoQueryable<IdentityUser>>()
                .PageBy<IdentityUser, IMongoQueryable<IdentityUser>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public virtual async Task<int> GetMembersCountAsync(
            OrganizationUnit organizationUnit,
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            var query = CreateGetMembersFilteredQuery(organizationUnit, filter);

            return await query.CountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<IdentityUser>> GetUnaddedUsersAsync(
            OrganizationUnit organizationUnit,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbContext.Users.AsQueryable()
                .Where(u => !u.OrganizationUnits.Any(uou => uou.OrganizationUnitId == organizationUnit.Id))
                .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
                )
                .OrderBy(sorting ?? nameof(IdentityUser.UserName))
                .As<IMongoQueryable<IdentityUser>>()
                .PageBy<IdentityUser, IMongoQueryable<IdentityUser>>(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<int> GetUnaddedUsersCountAsync(OrganizationUnit organizationUnit, string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await DbContext.Users.AsQueryable()
                .Where(u => !u.OrganizationUnits.Any(uou => uou.OrganizationUnitId == organizationUnit.Id))
                .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
                )
                .As<IMongoQueryable<IdentityUser>>()
                .CountAsync(GetCancellationToken(cancellationToken));
        }

        public virtual Task RemoveAllRolesAsync(OrganizationUnit organizationUnit, CancellationToken cancellationToken = default)
        {
            organizationUnit.Roles.Clear();
            return Task.FromResult(0);
        }

        public virtual async Task RemoveAllMembersAsync(OrganizationUnit organizationUnit, CancellationToken cancellationToken = default)
        {
            var users = await DbContext.Users.AsQueryable()
                .Where(u => u.OrganizationUnits.Any(uou => uou.OrganizationUnitId == organizationUnit.Id))
                .As<IMongoQueryable<IdentityUser>>()
                .ToListAsync(GetCancellationToken(cancellationToken));

            foreach (var user in users)
            {
                user.RemoveOrganizationUnit(organizationUnit.Id);
                DbContext.Users.ReplaceOne(u => u.Id == user.Id, user);
            }
        }

        protected virtual IMongoQueryable<IdentityUser> CreateGetMembersFilteredQuery(OrganizationUnit organizationUnit, string filter = null)
        {
            return DbContext.Users.AsQueryable()
                .Where(u => u.OrganizationUnits.Any(uou => uou.OrganizationUnitId == organizationUnit.Id))
                .WhereIf<IdentityUser, IMongoQueryable<IdentityUser>>(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
                );
        }
    }
}
