using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.UI;

namespace Volo.Abp.Account.Web.Pages;

//TODO: Move this into the Account folder!!!
public class ConsentModel : AbpPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string ReturnUrlHash { get; set; }

    [BindProperty]
    public ConsentModel.ConsentInputModel ConsentInput { get; set; }

    public ClientInfoModel ClientInfo { get; set; }

    private readonly IIdentityServerInteractionService _interaction;
    private readonly IClientStore _clientStore;
    private readonly IResourceStore _resourceStore;

    public ConsentModel(
        IIdentityServerInteractionService interaction,
        IClientStore clientStore,
        IResourceStore resourceStore)
    {
        _interaction = interaction;
        _clientStore = clientStore;
        _resourceStore = resourceStore;
    }

    public virtual async Task<IActionResult> OnGet()
    {
        var request = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
        if (request == null)
        {
            throw new ApplicationException($"No consent request matching request: {ReturnUrl}");
        }

        var client = await _clientStore.FindEnabledClientByIdAsync(request.Client.ClientId);
        if (client == null)
        {
            throw new ApplicationException($"Invalid client id: {request.Client.ClientId}");
        }

        var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ValidatedResources.RawScopeValues);
        if (resources == null || (!resources.IdentityResources.Any() && !resources.ApiResources.Any()))
        {
            throw new ApplicationException($"No scopes matching: {request.ValidatedResources.RawScopeValues.Aggregate((x, y) => x + ", " + y)}");
        }

        ClientInfo = new ClientInfoModel(client);
        ConsentInput = new ConsentInputModel
        {
            RememberConsent = true,
            IdentityScopes = resources.IdentityResources.Select(x => CreateScopeViewModel(x, true)).ToList(),
        };

        var apiScopes = new List<ScopeViewModel>();
        foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
        {
            var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
            if (apiScope != null)
            {
                var scopeVm = CreateScopeViewModel(parsedScope, apiScope, true);
                apiScopes.Add(scopeVm);
            }
        }

        if (resources.OfflineAccess)
        {
            apiScopes.Add(GetOfflineAccessScope(true));
        }

        ConsentInput.ApiScopes = apiScopes;

        return Page();
    }

    public virtual async Task<IActionResult> OnPost(string userDecision)
    {
        var result = await ProcessConsentAsync();

        if (result.IsRedirect)
        {
            return Redirect(result.RedirectUri);
        }

        if (result.HasValidationError)
        {
            //ModelState.AddModelError("", result.ValidationError);
            throw new ApplicationException("Error: " + result.ValidationError);
        }

        throw new ApplicationException("Unknown Error!");
    }

    protected virtual async Task<ConsentModel.ProcessConsentResult> ProcessConsentAsync()
    {
        var result = new ConsentModel.ProcessConsentResult();

        ConsentResponse grantedConsent;

        if (ConsentInput.UserDecision == "no")
        {
            grantedConsent = new ConsentResponse
            {
                Error = AuthorizationError.AccessDenied
            };
        }
        else
        {
            if (!ConsentInput.IdentityScopes.IsNullOrEmpty() || !ConsentInput.ApiScopes.IsNullOrEmpty())
            {
                grantedConsent = new ConsentResponse
                {
                    RememberConsent = ConsentInput.RememberConsent,
                    ScopesValuesConsented = ConsentInput.GetAllowedScopeNames()
                };
            }
            else
            {
                throw new UserFriendlyException("You must pick at least one permission"); //TODO: How to handle this
            }
        }

        if (grantedConsent != null)
        {
            var request = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
            if (request == null)
            {
                return result;
            }

            await _interaction.GrantConsentAsync(request, grantedConsent);

            result.RedirectUri = ReturnUrl; //TODO: ReturnUrlHash?
        }

        return result;
    }

    protected virtual ConsentModel.ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
    {
        return new ConsentModel.ScopeViewModel
        {
            Name = identity.Name,
            DisplayName = identity.DisplayName,
            Description = identity.Description,
            Emphasize = identity.Emphasize,
            Required = identity.Required,
            Checked = check || identity.Required
        };
    }

    protected virtual ConsentModel.ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
    {
        var displayName = apiScope.DisplayName ?? apiScope.Name;
        if (!string.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
        {
            displayName += ":" + parsedScopeValue.ParsedParameter;
        }

        return new ScopeViewModel
        {
            Name = parsedScopeValue.RawValue,
            DisplayName = displayName,
            Description = apiScope.Description,
            Emphasize = apiScope.Emphasize,
            Required = apiScope.Required,
            Checked = check || apiScope.Required
        };
    }

    protected virtual ConsentModel.ScopeViewModel GetOfflineAccessScope(bool check)
    {
        return new ConsentModel.ScopeViewModel
        {
            Name = IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess,
            DisplayName = "Offline Access", //TODO: Localize
            Description = "Access to your applications and resources, even when you are offline",
            Emphasize = true,
            Checked = check
        };
    }

    public class ConsentInputModel
    {
        public List<ConsentModel.ScopeViewModel> IdentityScopes { get; set; }

        public List<ConsentModel.ScopeViewModel> ApiScopes { get; set; }

        [Required]
        public string UserDecision { get; set; }

        public bool RememberConsent { get; set; }

        public List<string> GetAllowedScopeNames()
        {
            var identityScopes = IdentityScopes ?? new List<ConsentModel.ScopeViewModel>();
            var apiScopes = ApiScopes ?? new List<ConsentModel.ScopeViewModel>();
            return identityScopes.Union(apiScopes).Where(s => s.Checked).Select(s => s.Name).ToList();
        }
    }

    public class ScopeViewModel
    {
        [Required]
        [HiddenInput]
        public string Name { get; set; }

        public bool Checked { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Emphasize { get; set; }

        public bool Required { get; set; }
    }

    public class ProcessConsentResult
    {
        public bool IsRedirect => RedirectUri != null;
        public string RedirectUri { get; set; }

        public bool HasValidationError => ValidationError != null;
        public string ValidationError { get; set; }
    }

    public class ClientInfoModel
    {
        public string ClientName { get; set; }

        public string ClientUrl { get; set; }

        public string ClientLogoUrl { get; set; }

        public bool AllowRememberConsent { get; set; }

        public ClientInfoModel(Client client)
        {
            //TODO: Automap
            ClientName = client.ClientId;
            ClientUrl = client.ClientUri;
            ClientLogoUrl = client.LogoUri;
            AllowRememberConsent = client.AllowRememberConsent;
        }
    }
}
