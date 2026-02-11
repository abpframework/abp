using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.PermissionManagement.Web.Pages.AbpPermissionManagement;

public class UpdateResourcePermissionManagementModal : AbpPageModel
{
    [Required]
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ResourceName { get; set; }

    [Required]
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ResourceKey { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ResourceDisplayName { get; set; }

    [Required]
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ProviderName { get; set; }

    [Required]
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ProviderKey { get; set; }

    [BindProperty(SupportsGet = true)]
    public ResourcePermissionViewModel UpdateModel { get; set; }

    public GetResourcePermissionWithProviderListResultDto ResourcePermissions { get; set; }

    protected IPermissionAppService PermissionAppService { get; }

    public UpdateResourcePermissionManagementModal(IPermissionAppService permissionAppService)
    {
        ObjectMapperContext = typeof(AbpPermissionManagementWebModule);

        PermissionAppService = permissionAppService;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        ValidateModel();

        ResourcePermissions = await PermissionAppService.GetResourceByProviderAsync(ResourceName, ResourceKey, ProviderName, ProviderKey);

        return Page();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();

        await PermissionAppService.UpdateResourceAsync(
            ResourceName,
            ResourceKey,
            new UpdateResourcePermissionsDto()
            {
                ProviderName = ProviderName,
                ProviderKey = ProviderKey,
                Permissions = UpdateModel.Permissions ?? new List<string>()
            }
        );

        return NoContent();
    }

    public class ResourcePermissionViewModel
    {
        public List<string> Permissions { get; set; }
    }
}
