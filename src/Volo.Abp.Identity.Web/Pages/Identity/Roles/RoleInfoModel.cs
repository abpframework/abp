using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.Identity.Web.Pages.Identity.Roles
{
    public class RoleInfoModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [Required]
        [StringLength(IdentityRoleConsts.MaxNameLength)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }
}