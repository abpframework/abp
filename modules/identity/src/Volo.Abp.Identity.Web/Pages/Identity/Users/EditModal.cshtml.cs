using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Volo.Abp.Identity.Web.Pages.Identity.Users;

public class EditModalModel : IdentityPageModel
{
    [BindProperty]
    public UserInfoViewModel UserInfo { get; set; }

    [BindProperty]
    public AssignedRoleViewModel[] Roles { get; set; }

    public DetailViewModel Detail { get; set; }

    protected IIdentityUserAppService IdentityUserAppService { get; }

    protected IPermissionChecker PermissionChecker { get; }

    public bool IsEditCurrentUser { get; set; }

    public EditModalModel(IIdentityUserAppService identityUserAppService, IPermissionChecker permissionChecker)
    {
        IdentityUserAppService = identityUserAppService;
        PermissionChecker = permissionChecker;
    }

    public virtual async Task<IActionResult> OnGetAsync(Guid id)
    {
        var user = await IdentityUserAppService.GetAsync(id);
        UserInfo = ObjectMapper.Map<IdentityUserDto, UserInfoViewModel>(user);
        if (await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageRoles))
        {
            Roles = ObjectMapper.Map<IReadOnlyList<IdentityRoleDto>, AssignedRoleViewModel[]>((await IdentityUserAppService.GetAssignableRolesAsync()).Items);
        }
        IsEditCurrentUser = CurrentUser.Id == id;

        var userRoleNames = (await IdentityUserAppService.GetRolesAsync(UserInfo.Id)).Items.Select(r => r.Name).ToList();
        foreach (var role in Roles)
        {
            if (userRoleNames.Contains(role.Name))
            {
                role.IsAssigned = true;
            }
        }

        Detail = ObjectMapper.Map<IdentityUserDto, DetailViewModel>(user);

        Detail.CreatedBy = await GetUserNameOrNullAsync(user.CreatorId);
        Detail.ModifiedBy = await GetUserNameOrNullAsync(user.LastModifierId);

        return Page();
    }

    private async Task<string> GetUserNameOrNullAsync(Guid? userId)
    {
        if (!userId.HasValue)
        {
            return null;
        }

        var user = await IdentityUserAppService.GetAsync(userId.Value);
        return user.UserName;
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();

        var input = ObjectMapper.Map<UserInfoViewModel, IdentityUserUpdateDto>(UserInfo);
        input.RoleNames = Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToArray();
        await IdentityUserAppService.UpdateAsync(UserInfo.Id, input);

        return NoContent();
    }

    public class UserInfoViewModel : ExtensibleObject, IHasConcurrencyStamp
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [HiddenInput]
        public string ConcurrencyStamp { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
        public string Surname { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string Email { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public bool LockoutEnabled { get; set; }
    }

    public class AssignedRoleViewModel
    {
        [Required]
        [HiddenInput]
        public string Name { get; set; }

        public bool IsAssigned { get; set; }
    }

    public class DetailViewModel
    {
        public string CreatedBy { get; set; }
        public DateTime? CreationTime { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public DateTimeOffset? LastPasswordChangeTime { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public int AccessFailedCount { get; set; }
    }
}
