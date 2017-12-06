using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity.Web.Areas.Identity.Models;

namespace Volo.Abp.Identity.Web.Areas.Identity.Controllers
{
    //[Area("Identity")]
    //[Authorize]
    //public class UsersController : AbpController
    //{
    //    private readonly IIdentityUserAppService _identityUserAppService;
    //    private readonly IIdentityRoleAppService _identityRoleAppService;

    //    public UsersController(IIdentityUserAppService identityUserAppService, IIdentityRoleAppService identityRoleAppService)
    //    {
    //        _identityUserAppService = identityUserAppService;
    //        _identityRoleAppService = identityRoleAppService;
    //    }

    //    public async Task<PartialViewResult> Update(Guid id)
    //    {
    //        var user = await _identityUserAppService.GetAsync(id);
    //        await _identityRoleAppService.GetAllListAsync();
    //        var model = await CreateViewModel(user);

    //        return PartialView("_Update", model);
    //    }

    //    private async Task<CreateOrUpdateUserViewModel> CreateViewModel(IdentityUserDto user)
    //    {
    //        var allRoles = await _identityRoleAppService.GetAllListAsync();

    //        var model = new CreateOrUpdateUserViewModel
    //        {
    //            User = user ?? new IdentityUserDto(),
    //            Roles = ObjectMapper.Map<List<IdentityRoleDto>, IdentityUserRoleDto[]>(allRoles)
    //        };

    //        var userRoles = new List<IdentityRoleDto>();

    //        if (user != null)
    //        {
    //            userRoles = (await _identityUserAppService.GetRolesAsync(user.Id)).Items.ToList();
    //        }

    //        foreach (var role in model.Roles)
    //        {
    //            if (userRoles.Select(x=>x.Name).Contains(role.Name))
    //            {
    //                role.IsAssigned = true;
    //            }
    //        }

    //        return model;
    //    }
    //}
}
