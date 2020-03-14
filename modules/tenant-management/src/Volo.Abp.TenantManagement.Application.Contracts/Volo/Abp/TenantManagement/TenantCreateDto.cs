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
        public string AdminEmailAddress { get; set; }

        public string AdminPassword { get; set; }
    }
}