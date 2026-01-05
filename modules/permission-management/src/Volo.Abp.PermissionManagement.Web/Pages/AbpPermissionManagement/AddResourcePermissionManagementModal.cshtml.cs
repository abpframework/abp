using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.PermissionManagement.Web.Pages.AbpPermissionManagement;

public class AddResourcePermissionManagementModal : AbpPageModel
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
    [HiddenInput]
    public string ResourceDisplayName { get; set; }

    [BindProperty]
    public ResourcePermissionViewModel AddModel { get; set; }

    public GetResourcePermissionDefinitionListResultDto ResourcePermissionDefinitions { get; set; }
    public GetResourceProviderListResultDto ResourceProviders { get; set; }

    protected IPermissionAppService PermissionAppService { get; }

    public AddResourcePermissionManagementModal(IPermissionAppService permissionAppService)
    {
        ObjectMapperContext = typeof(AbpPermissionManagementWebModule);

        PermissionAppService = permissionAppService;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        ValidateModel();

        ResourcePermissionDefinitions = await PermissionAppService.GetResourceDefinitionsAsync(ResourceName);
        ResourceProviders = await PermissionAppService.GetResourceProviderKeyLookupServicesAsync(ResourceName);

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
                ProviderName = AddModel.ProviderName,
                ProviderKey = AddModel.ProviderKey,
                Permissions = AddModel.Permissions ?? new List<string>()
            }
        );

        return NoContent();
    }

    public class ResourcePermissionViewModel
    {
        [Required]
        public string ProviderName { get; set; }

        [Required]
        public string ProviderKey { get; set; }

        public List<string> Permissions { get; set; }
    }
}
