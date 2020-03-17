using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.TenantManagement.Localization;

namespace Volo.Abp.TenantManagement
{
    public class TenantCreateDto : TenantCreateOrUpdateDtoBase
    {
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string AdminEmailAddress { get; set; }


        [Required]
        [MaxLength(128)]
        public string AdminPassword { get; set; }
    }
}