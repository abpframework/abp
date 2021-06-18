using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;

namespace Volo.Abp.Account.Web.Pages.Account
{
    //TODO: Implement live password complexity check on the razor view!

    public class ResetPasswordModel : AccountPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? TenantId { get; set; }

        [Required]
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid UserId { get; set; }

        [Required]
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ResetToken { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DisableAuditing]
        public string Password { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DisableAuditing]
        public string ConfirmPassword { get; set; }

        protected virtual ITenantResolveResultAccessor TenantResolveResultAccessor { get; }

        protected virtual AbpAspNetCoreMultiTenancyOptions AspNetCoreMultiTenancyOptions { get; }

        protected virtual AbpMultiTenancyOptions MultiTenancyOptions { get; }

        public ResetPasswordModel(
            ITenantResolveResultAccessor tenantResolveResultAccessor,
            IOptions<AbpAspNetCoreMultiTenancyOptions> aspNetCoreMultiTenancyOptions,
            IOptions<AbpMultiTenancyOptions> multiTenancyOptions)
        {
            TenantResolveResultAccessor = tenantResolveResultAccessor;
            AspNetCoreMultiTenancyOptions = aspNetCoreMultiTenancyOptions.Value;
            MultiTenancyOptions = multiTenancyOptions.Value;
        }

        public virtual Task<IActionResult> OnGetAsync()
        {
            if (MultiTenancyOptions.IsEnabled &&
                TenantResolveResultAccessor.Result?.AppliedResolvers?.Contains(CookieTenantResolveContributor.ContributorName) == true)
            {
                if (CurrentTenant.Id != TenantId)
                {
                    if (TenantId != null)
                    {
                        Response.Cookies.Append(
                            AspNetCoreMultiTenancyOptions.TenantKey,
                            TenantId.ToString(),
                            new CookieOptions
                            {
                                Path = "/",
                                HttpOnly = false,
                                Expires = DateTimeOffset.Now.AddYears(10)
                            }
                        );
                    }
                    else
                    {
                        Response.Cookies.Delete(AspNetCoreMultiTenancyOptions.TenantKey);
                    }

                    return Task.FromResult<IActionResult>(Redirect(HttpContext.Request.GetEncodedUrl()));
                }
            }

            return Task.FromResult<IActionResult>(Page());
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ValidateModel();

                await AccountAppService.ResetPasswordAsync(
                    new ResetPasswordDto
                    {
                        UserId = UserId,
                        ResetToken = ResetToken,
                        Password = Password,
                        TenantId = TenantId
                    }
                );
            }
            catch (AbpIdentityResultException e)
            {
                if (!string.IsNullOrWhiteSpace(e.Message))
                {
                    Alerts.Warning(GetLocalizeExceptionMessage(e));
                    return Page();
                }

                throw;
            }
            catch (AbpValidationException e)
            {
                return Page();
            }

            //TODO: Try to automatically login!
            return RedirectToPage("./ResetPasswordConfirmation", new
            {
                returnUrl = ReturnUrl,
                returnUrlHash = ReturnUrlHash
            });
        }

        protected override void ValidateModel()
        {
            if (!Equals(Password, ConfirmPassword))
            {
                ModelState.AddModelError("ConfirmPassword",
                    L["'{0}' and '{1}' do not match.", "ConfirmPassword", "Password"]);
            }

            base.ValidateModel();
        }
    }
}
