using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.PermissionManagement.Web.Utils;

namespace Volo.Abp.PermissionManagement.Web.Pages.AbpPermissionManagement
{
    public class PermissionManagementModal : AbpPageModel
    {
        [Required]
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ProviderName { get; set; }

        [Required]
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ProviderKey { get; set; }

        [BindProperty]
        public List<PermissionGroupViewModel> Groups { get; set; }

        public string EntityDisplayName { get; set; }

        public bool SelectAllInThisTab { get; set; }

        public bool SelectAllInAllTabs { get; set; }

        private readonly IPermissionAppService _permissionAppService;

        public PermissionManagementModal(IPermissionAppService permissionAppService)
        {
            ObjectMapperContext = typeof(AbpPermissionManagementWebModule);

            _permissionAppService = permissionAppService;
        }

        public async Task OnGetAsync()
        {
            ValidateModel();

            var result = await _permissionAppService.GetAsync(ProviderName, ProviderKey);

            EntityDisplayName = result.EntityDisplayName;

            Groups = ObjectMapper
                .Map<List<PermissionGroupDto>, List<PermissionGroupViewModel>>(result.Groups)
                .OrderBy(g => g.DisplayName)
                .ToList();

            foreach (var group in Groups)
            {
                new FlatTreeDepthFinder<PermissionGrantInfoViewModel>().SetDepths(group.Permissions);
            }

            foreach (var group in Groups)
            {
                group.IsAllPermissionsGranted = group.Permissions.All(p => p.IsGranted);
            }

            SelectAllInAllTabs = Groups.All(g => g.IsAllPermissionsGranted);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            var updatePermissionDtos = Groups
                .SelectMany(g => g.Permissions)
                .Select(p => new UpdatePermissionDto
                {
                    Name = p.Name,
                    IsGranted = p.IsGranted
                })
                .ToArray();

            await _permissionAppService.UpdateAsync(
                ProviderName,
                ProviderKey,
                new UpdatePermissionsDto
                {
                    Permissions = updatePermissionDtos
                }
            );

            return NoContent();
        }

        public class PermissionGroupViewModel
        {
            public string Name { get; set; }

            public bool IsAllPermissionsGranted { get; set; }

            public string DisplayName { get; set; }

            public List<PermissionGrantInfoViewModel> Permissions { get; set; }

            public string GetNormalizedGroupName()
            {
                return Name.Replace(".", "_");
            }
        }

        public class PermissionGrantInfoViewModel : IFlatTreeItem
        {
            [Required]
            [HiddenInput]
            public string Name { get; set; }

            public string DisplayName { get; set; }

            public int Depth { get; set; }

            public string ParentName { get; set; }

            public bool IsGranted { get; set; }

            public List<string> AllowedProviders { get; set; }

            public List<ProviderInfoViewModel> GrantedProviders { get; set; }

            public bool IsDisabled(string currentProviderName)
            {
                return IsGranted && GrantedProviders.All(p => p.ProviderName != currentProviderName);
            }

            public string GetShownName(string currentProviderName)
            {
                if (!IsDisabled(currentProviderName))
                {
                    return DisplayName;
                }

                return string.Format(
                    "{0} <span class=\"text-muted\">({1})</span>",
                    DisplayName,
                    GrantedProviders
                        .Where(p => p.ProviderName != currentProviderName)
                        .Select(p => p.ProviderName)
                        .JoinAsString(", ")
                );
            }
        }

        public class ProviderInfoViewModel
        {
            public string ProviderName { get; set; }

            public string ProviderKey { get; set; }
        }
    }
}