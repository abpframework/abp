using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.Account.Web.Pages
{
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
        public ConsentInputModel ConsentInput { get; set; }

        public string ClientName { get; set; }
        public string ClientUrl { get; set; }
        public string ClientLogoUrl { get; set; }
        public bool AllowRememberConsent { get; set; }

        public List<ScopeViewModel> IdentityScopes { get; set; }

        public List<ScopeViewModel> ResourceScopes { get; set; }
        
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

        public async Task OnGet()
        {
            var request = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
            if (request == null)
            {
                throw new ApplicationException($"No consent request matching request: {ReturnUrl}");
            }

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            if (client == null)
            {
                throw new ApplicationException($"Invalid client id: {request.ClientId}");
            }

            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
            if (resources == null || (!resources.IdentityResources.Any() && !resources.ApiResources.Any()))
            {
                throw new ApplicationException($"No scopes matching: {request.ScopesRequested.Aggregate((x, y) => x + ", " + y)}");
            }

            ConsentInput = new ConsentInputModel
            {
                RememberConsent = true,
                ScopesConsented = new List<string>()
            };

            ClientName = client.ClientId; //TODO: Consider to create a ClientInfoModel
            ClientUrl = client.ClientUri;
            ClientLogoUrl = client.LogoUri;
            AllowRememberConsent = client.AllowRememberConsent;

            IdentityScopes = resources.IdentityResources.Select(x => CreateScopeViewModel(x, true)).ToList();
            ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => CreateScopeViewModel(x, true)).ToList();

            if (resources.OfflineAccess)
            {
                ResourceScopes = ResourceScopes.Union(new[] {GetOfflineAccessScope(true)}).ToList();
            }
        }

        public async Task<IActionResult> OnPost(string userDecision)
        {
            var result = await ProcessConsentAsync();

            if (result.IsRedirect)
            {
                return Redirect(result.RedirectUri);
            }

            if (result.HasValidationError)
            {
                ModelState.AddModelError("", result.ValidationError);
            }

            throw new ApplicationException("Error: ");
        }

        private async Task<ProcessConsentResult> ProcessConsentAsync()
        {
            var result = new ProcessConsentResult();

            ConsentResponse grantedConsent = null;

            if (ConsentInput.UserDecision == "no")
            {
                grantedConsent = ConsentResponse.Denied;
            }
            else
            {
                if (ConsentInput.ScopesConsented != null && ConsentInput.ScopesConsented.Any())
                {
                    var scopes = ConsentInput.ScopesConsented;

                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = ConsentInput.RememberConsent,
                        ScopesConsented = scopes.ToArray()
                    };
                }
                else
                {
                    result.ValidationError = "You must pick at least one permission";
                }
            }

            if (grantedConsent != null)
            {
                // validate return url is still valid
                var request = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
                if (request == null) return result;

                // communicate outcome of consent back to identityserver
                await _interaction.GrantConsentAsync(request, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = ReturnUrl; //TODO: ReturnUrlHash?
            }

            return result;
        }


        private ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
            };
        }

        public ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                Required = scope.Required,
                Checked = check || scope.Required
            };
        }

        private ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
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
            public string UserDecision { get; set; }

            public List<string> ScopesConsented { get; set; }

            public bool RememberConsent { get; set; }
        }

        public class ScopeViewModel
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
            public string Description { get; set; }
            public bool Emphasize { get; set; }
            public bool Required { get; set; }
            public bool Checked { get; set; }
        }

        public class ProcessConsentResult
        {
            public bool IsRedirect => RedirectUri != null;
            public string RedirectUri { get; set; }

            public bool HasValidationError => ValidationError != null;
            public string ValidationError { get; set; }
        }
    }
}