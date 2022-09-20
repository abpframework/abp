using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;

namespace Volo.Abp.Account.Web.Pages.Account;

[ExposeServices(typeof(LoginModel))]
public class OpenIddictSupportedLoginModel : LoginModel
{
    protected AbpOpenIddictRequestHelper OpenIddictRequestHelper { get; }
    public OpenIddictSupportedLoginModel(
        IAuthenticationSchemeProvider schemeProvider,
        IOptions<AbpAccountOptions> accountOptions,
        IOptions<IdentityOptions> identityOptions,
        AbpOpenIddictRequestHelper openIddictRequestHelper)
        : base(schemeProvider, accountOptions, identityOptions)
    {
        OpenIddictRequestHelper = openIddictRequestHelper;
    }

    public async override Task<IActionResult> OnGetAsync()
    {
        LoginInput = new LoginInputModel();

        var request = await OpenIddictRequestHelper.GetFromReturnUrlAsync(ReturnUrl);
        if (request?.ClientId != null)
        {
            ShowCancelButton = true;

            LoginInput.UserNameOrEmailAddress = request.LoginHint;

            //TODO: Reference AspNetCore MultiTenancy module and use options to get the tenant key!
            var tenant = request.GetParameter(TenantResolverConsts.DefaultTenantKey)?.ToString();
            if (!string.IsNullOrEmpty(tenant))
            {
                CurrentTenant.Change(Guid.Parse(tenant));
                Response.Cookies.Append(TenantResolverConsts.DefaultTenantKey, tenant);
            }
        }

        return await base.OnGetAsync();
    }

    public async override Task<IActionResult> OnPostAsync(string action)
    {
        if (action == "Cancel")
        {
            var request = await OpenIddictRequestHelper.GetFromReturnUrlAsync(ReturnUrl);

            var transaction = HttpContext.GetOpenIddictServerTransaction();
            if (request?.ClientId != null && transaction != null)
            {
                transaction.EndpointType = OpenIddictServerEndpointType.Authorization;
                transaction.Request = request;

                var notification = new OpenIddictServerEvents.ValidateAuthorizationRequestContext(transaction);
                transaction.SetProperty(typeof(OpenIddictServerEvents.ValidateAuthorizationRequestContext).FullName!, notification);

                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            return Redirect("~/");
        }

        return await base.OnPostAsync(action);
    }

    public async override Task<IActionResult> OnPostExternalLogin(string provider)
    {
        if (AccountOptions.WindowsAuthenticationSchemeName == provider)
        {
            return await ProcessWindowsLoginAsync();
        }

        return await base.OnPostExternalLogin(provider);
    }

    protected virtual async Task<IActionResult> ProcessWindowsLoginAsync()
    {
        var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
        if (result.Succeeded)
        {
            var props = new AuthenticationProperties()
            {
                RedirectUri = Url.Page("./Login", pageHandler: "ExternalLoginCallback", values: new { ReturnUrl, ReturnUrlHash }),
                Items =
                {
                    {
                        "LoginProvider", AccountOptions.WindowsAuthenticationSchemeName
                    }
                }
            };

            var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
            id.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.Principal.FindFirstValue(ClaimTypes.PrimarySid)));
            id.AddClaim(new Claim(ClaimTypes.Name, result.Principal.FindFirstValue(ClaimTypes.Name)));

            await HttpContext.SignInAsync(IdentityConstants.ExternalScheme, new ClaimsPrincipal(id), props);

            return Redirect(props.RedirectUri!);
        }

        return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
    }
}
