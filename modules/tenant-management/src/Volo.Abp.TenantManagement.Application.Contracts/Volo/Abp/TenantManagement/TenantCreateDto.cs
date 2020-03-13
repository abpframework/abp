using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.TenantManagement.Localization;

namespace Volo.Abp.TenantManagement
{
    public class TenantCreateDto : TenantCreateOrUpdateDtoBase, IValidatableObject
    {
        public string AdminEmailAddress { get; set; }

        public string AdminPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var l = validationContext.GetRequiredService<IStringLocalizer<AbpTenantManagementResource>>();

            var isValidEmail =
                Regex.IsMatch(AdminEmailAddress, TenantConsts.EmailRegex, RegexOptions.IgnoreCase);

            if (string.IsNullOrWhiteSpace(AdminEmailAddress) || !isValidEmail)
            {
                yield return new ValidationResult(l["InvalidAdminProperties"], new[] { nameof(AdminEmailAddress) });
            }

            if (string.IsNullOrWhiteSpace(AdminPassword))
            {
                yield return new ValidationResult(l["InvalidAdminProperties"], new[] { nameof(AdminPassword) });
            }
        }
    }
}