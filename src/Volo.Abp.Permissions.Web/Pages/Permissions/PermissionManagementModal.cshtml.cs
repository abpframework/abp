using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.Permissions.Web.Pages.Permissions
{
    public class PermissionManagementModal : AbpPageModel
    {
        [BindProperty]
        public PerminssionInfoModel[] Permissions { get; set; }

        public async Task OnGetAsync()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            ValidateModel();

            return NoContent();
        }

        public class PerminssionInfoModel
        {
            [Required]
            //[StringLength(IdentityRoleConsts.MaxNameLength)]
            [Display(Name = "RoleName")]
            public string Name { get; set; }
        }
    }
}