using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace Volo.Abp.Identity.Integration;

public class IdentityUserIntegrationService : IdentityAppServiceBase, IIdentityUserIntegrationService
{
    protected IUserRoleFinder UserRoleFinder { get; }
    protected IdentityUserRepositoryExternalUserLookupServiceProvider UserLookupServiceProvider { get; }
    protected IIdentityUserRepository UserRepository { get; }
    protected IIdentityRoleRepository RoleRepository { get; }

    public IdentityUserIntegrationService(
        IUserRoleFinder userRoleFinder,
        IdentityUserRepositoryExternalUserLookupServiceProvider userLookupServiceProvider,
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository)
    {
        UserRoleFinder = userRoleFinder;
        UserLookupServiceProvider = userLookupServiceProvider;
        UserRepository = userRepository;
        RoleRepository = roleRepository;
    }

    public virtual async Task<string[]> GetRoleNamesAsync(Guid id)
    {
        return await UserRoleFinder.GetRoleNamesAsync(id);
    }

    public virtual async Task<UserData> FindByIdAsync(Guid id)
    {
        var userData = await UserLookupServiceProvider.FindByIdAsync(id);
        if (userData == null)
        {
            return null;
        }

        return new UserData(userData);
    }

    public virtual async Task<UserData> FindByUserNameAsync(string userName)
    {
        var userData = await UserLookupServiceProvider.FindByUserNameAsync(userName);
        if (userData == null)
        {
            return null;
        }

        return new UserData(userData);
    }

    public virtual async Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
    {
        var users = await UserLookupServiceProvider.SearchAsync(
            input.Sorting,
            input.Filter,
            input.MaxResultCount,
            input.SkipCount
        );

        return new ListResultDto<UserData>(
            users
                .Select(u => new UserData(u))
                .ToList()
        );
    }

    public virtual async Task<ListResultDto<UserData>> SearchByIdsAsync(Guid[] ids)
    {
        var users = await UserRepository.GetListByIdsAsync(ids);

        return new ListResultDto<UserData>(
            users
                .Select(u => new UserData(
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.Name,
                    u.Surname,
                    u.EmailConfirmed,
                    u.PhoneNumber,
                    u.PhoneNumberConfirmed,
                    u.TenantId,
                    u.IsActive,
                    u.ExtraProperties))
                .ToList()
        );
    }

    public virtual async Task<long> GetCountAsync(UserLookupCountInputDto input)
    {
        return await UserLookupServiceProvider.GetCountAsync(input.Filter);
    }

    public virtual async Task<ListResultDto<RoleData>> SearchRoleAsync(RoleLookupSearchInputDto input)
    {
        using (RoleRepository.DisableTracking())
        {
            var roles = await RoleRepository.GetListAsync(sorting: input.Sorting, maxResultCount: input.MaxResultCount, skipCount: input.SkipCount, filter: input.Filter);
            return new ListResultDto<RoleData>(roles.Select(r => new RoleData(r.Id, r.Name, r.IsDefault, r.IsStatic, r.IsPublic, r.TenantId, r.ExtraProperties)).ToList());
        }
    }

    public virtual async Task<ListResultDto<RoleData>> SearchRoleByNamesAsync(string[] names)
    {
        using (RoleRepository.DisableTracking())
        {
            var roles = await RoleRepository.GetListAsync(names);
            return new ListResultDto<RoleData>(roles.Select(r => new RoleData(r.Id, r.Name, r.IsDefault, r.IsStatic, r.IsPublic, r.TenantId, r.ExtraProperties)).ToList());
        }
    }

    public virtual async Task<long> GetRoleCountAsync(RoleLookupCountInputDto input)
    {
        return await RoleRepository.GetCountAsync(input.Filter);
    }
}
