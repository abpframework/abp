using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Account.Web.Pages.Account;

[ExposeServices(typeof(LoginModel))]
public class OpenIddictSupportedLoginModel : LoginModel
{
    public OpenIddictSupportedLoginModel(
        IAuthenticationSchemeProvider schemeProvider,
        IOptions<AbpAccountOptions> accountOptions,
        IOptions<IdentityOptions> identityOptions)
        : base(schemeProvider, accountOptions, identityOptions)
    {
    }

    public async override Task<IActionResult> OnGetAsync()
    {
        LoginInput = new LoginInputModel();

        var request = await GetOpenIddictRequestFromReturnUrlAsync(ReturnUrl);
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
            var request = await GetOpenIddictRequestFromReturnUrlAsync(ReturnUrl);
            if (request?.ClientId == null)
            {
                return Redirect("~/");
            }

            var transaction = HttpContext.Features.Get<OpenIddictServerAspNetCoreFeature>()?.Transaction;

            transaction.EndpointType = OpenIddictServerEndpointType.Authorization;
            transaction.Request = request;

            var notification = new OpenIddictServerEvents.ValidateAuthorizationRequestContext(transaction);
            transaction.SetProperty(typeof(OpenIddictServerEvents.ValidateAuthorizationRequestContext).FullName!, notification);

            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        return await base.OnPostAsync(action);
    }

    protected virtual Task<OpenIddictRequest> GetOpenIddictRequestFromReturnUrlAsync(string returnUrl)
    {
        if (!returnUrl.IsNullOrWhiteSpace())
        {
            var qm = returnUrl.IndexOf("?", StringComparison.Ordinal);
            if (qm > 0)
            {
                return Task.FromResult(new OpenIddictRequest(returnUrl.Substring(qm + 1)
                    .Split("&")
                    .Select(x =>
                        x.Split("=").Length == 2
                            ? new KeyValuePair<string, string>(x.Split("=")[0], WebUtility.UrlDecode(x.Split("=")[1]))
                            : new KeyValuePair<string, string>(null, null))
                    .Where(x => x.Key != null)));
            }
        }

        return Task.FromResult<OpenIddictRequest>(null);
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
