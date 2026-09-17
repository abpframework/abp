using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Volo.Abp.PermissionManagement.Web.Pages.AbpPermissionManagement;

public class ResourcePermissionManagementModal : AbpPageModel
{
    [Required]
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ResourceName { get; set; }

    [Required]
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ResourceKey { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ResourceDisplayName { get; set; }

    public bool HasAnyResourcePermission { get; set; }
    public bool HasAnyResourceProviderKeyLookupService { get; set; }

    protected IPermissionAppService PermissionAppService { get; }

    public ResourcePermissionManagementModal(IPermissionAppService permissionAppService)
    {
        ObjectMapperContext = typeof(AbpPermissionManagementWebModule);

        PermissionAppService = permissionAppService;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        HasAnyResourcePermission = (await PermissionAppService.GetResourceDefinitionsAsync(ResourceName)).Permissions.Any();
        if (HasAnyResourcePermission)
        {
            HasAnyResourceProviderKeyLookupService = (await PermissionAppService.GetResourceProviderKeyLookupServicesAsync(ResourceName)).Providers.Count > 0;
        }
        return Page();
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(NoContent());
    }
}
