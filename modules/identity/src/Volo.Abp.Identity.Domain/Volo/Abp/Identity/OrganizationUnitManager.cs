using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Volo.Abp.Identity
{
    /// <summary>
    /// Performs domain logic for Organization Units.
    /// </summary>
    public class OrganizationUnitManager : DomainService
    {
        protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
        protected IStringLocalizer<IdentityResource> Localizer { get; }
        protected IIdentityRoleRepository IdentityRoleRepository { get; }
        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        public OrganizationUnitManager(
            IOrganizationUnitRepository organizationUnitRepository,
            IStringLocalizer<IdentityResource> localizer,
            IIdentityRoleRepository identityRoleRepository,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            OrganizationUnitRepository = organizationUnitRepository;
            Localizer = localizer;
            IdentityRoleRepository = identityRoleRepository;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        [UnitOfWork]
        public virtual async Task CreateAsync(OrganizationUnit organizationUnit)
        {
            organizationUnit.Code = await GetNextChildCodeAsync(organizationUnit.ParentId);
            await ValidateOrganizationUnitAsync(organizationUnit);
            await OrganizationUnitRepository.InsertAsync(organizationUnit);
        }

        public virtual async Task UpdateAsync(OrganizationUnit organizationUnit)
        {
            await ValidateOrganizationUnitAsync(organizationUnit);
            await OrganizationUnitRepository.UpdateAsync(organizationUnit);
        }

        public virtual async Task<string> GetNextChildCodeAsync(Guid? parentId)
        {
            var lastChild = await GetLastChildOrNullAsync(parentId);
            if (lastChild != null)
            {
                return OrganizationUnit.CalculateNextCode(lastChild.Code);
            }

            var parentCode = parentId != null
                ? await GetCodeOrDefaultAsync(parentId.Value)
                : null;

            return OrganizationUnit.AppendCode(
                parentCode,
                OrganizationUnit.CreateCode(1)
            );
        }

        public virtual async Task<OrganizationUnit> GetLastChildOrNullAsync(Guid? parentId)
        {
            var children = await OrganizationUnitRepository.GetChildrenAsync(parentId);
            return children.OrderBy(c => c.Code).LastOrDefault();
        }

        [UnitOfWork]
        public virtual async Task DeleteAsync(Guid id)
        {
            var children = await FindChildrenAsync(id, true);

            foreach (var child in children)
            {
                await OrganizationUnitRepository.RemoveAllMembersAsync(child);
                await OrganizationUnitRepository.RemoveAllRolesAsync(child);
                await OrganizationUnitRepository.DeleteAsync(child);
            }

            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);

            await OrganizationUnitRepository.RemoveAllMembersAsync(organizationUnit);
            await OrganizationUnitRepository.RemoveAllRolesAsync(organizationUnit);
            await OrganizationUnitRepository.DeleteAsync(id);
        }

        [UnitOfWork]
        public virtual async Task MoveAsync(Guid id, Guid? parentId)
        {
            var organizationUnit = await OrganizationUnitRepository.GetAsync(id);
            if (organizationUnit.ParentId == parentId)
            {
                return;
            }

            //Should find children before Code change
            var children = await FindChildrenAsync(id, true);

            //Store old code of OU
            var oldCode = organizationUnit.Code;

            //Move OU
            organizationUnit.Code = await GetNextChildCodeAsync(parentId);
            organizationUnit.ParentId = parentId;

            await ValidateOrganizationUnitAsync(organizationUnit);

            //Update Children Codes
            foreach (var child in children)
            {
                child.Code = OrganizationUnit.AppendCode(organizationUnit.Code, OrganizationUnit.GetRelativeCode(child.Code, oldCode));
                await OrganizationUnitRepository.UpdateAsync(child);
            }

            await OrganizationUnitRepository.UpdateAsync(organizationUnit);
        }

        public virtual async Task<string> GetCodeOrDefaultAsync(Guid id)
        {
            var ou = await OrganizationUnitRepository.GetAsync(id);
            return ou?.Code;
        }

        protected virtual async Task ValidateOrganizationUnitAsync(OrganizationUnit organizationUnit)
        {
            var siblings = (await FindChildrenAsync(organizationUnit.ParentId))
                .Where(ou => ou.Id != organizationUnit.Id)
                .ToList();

            if (siblings.Any(ou => ou.DisplayName == organizationUnit.DisplayName))
            {
                throw new BusinessException(IdentityErrorCodes.DuplicateOrganizationUnitDisplayName)
                    .WithData("0", organizationUnit.DisplayName);
            }
        }

        public async Task<List<OrganizationUnit>> FindChildrenAsync(Guid? parentId, bool recursive = false)
        {
            if (!recursive)
            {
                return await OrganizationUnitRepository.GetChildrenAsync(parentId, includeDetails: true);
            }

            if (!parentId.HasValue)
            {
                return await OrganizationUnitRepository.GetListAsync(includeDetails: true);
            }

            var code = await GetCodeOrDefaultAsync(parentId.Value);

            return await OrganizationUnitRepository.GetAllChildrenWithParentCodeAsync(code, parentId, includeDetails: true);
        }

        public virtual Task<bool> IsInOrganizationUnitAsync(IdentityUser user, OrganizationUnit ou)
        {
            return Task.FromResult(user.IsInOrganizationUnit(ou.Id));
        }

        public virtual async Task AddRoleToOrganizationUnitAsync(Guid roleId, Guid ouId)
        {
            await AddRoleToOrganizationUnitAsync(
                await IdentityRoleRepository.GetAsync(roleId),
                await OrganizationUnitRepository.GetAsync(ouId, true)
                );
        }

        public virtual Task AddRoleToOrganizationUnitAsync(IdentityRole role, OrganizationUnit ou)
        {
            var currentRoles = ou.Roles;

            if (currentRoles.Any(r => r.OrganizationUnitId == ou.Id && r.RoleId == role.Id))
            {
                return Task.FromResult(0);
            }
            ou.AddRole(role.Id);
            return OrganizationUnitRepository.UpdateAsync(ou);
        }

        public virtual async Task RemoveRoleFromOrganizationUnitAsync(Guid roleId, Guid ouId)
        {
            await RemoveRoleFromOrganizationUnitAsync(
                await IdentityRoleRepository.GetAsync(roleId),
                await OrganizationUnitRepository.GetAsync(ouId, true)
                );
        }

        public virtual Task RemoveRoleFromOrganizationUnitAsync(IdentityRole role, OrganizationUnit organizationUnit)
        {
            organizationUnit.RemoveRole(role.Id);
            return OrganizationUnitRepository.UpdateAsync(organizationUnit);
        }
    }
}
