using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

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
        public ConsentViewModel Consent { get; set; }

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
            Consent = await BuildViewModelAsync(ReturnUrl);
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

        protected virtual async Task<ProcessConsentResult> ProcessConsentAsync()
        {
            var result = new ProcessConsentResult();

            // validate return url is still valid
            var request = await _interaction.GetAuthorizationContextAsync(ReturnUrl);
            if (request == null)
            {
                return result;
            }

            ConsentResponse grantedConsent = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if (Consent?.Button == "no")
            {
                grantedConsent = new ConsentResponse { Error = AuthorizationError.AccessDenied };
                // emit event
                //await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues));
            }
            // user clicked 'yes' - validate the data
            else if (Consent?.Button == "yes")
            {
                Consent.ScopesConsented =
                    Consent.ApiScopes.Union(Consent.IdentityScopes).Distinct().Select(x => x.Value).ToList();
                // if the user consented to some scope, build the response model
                if (!Consent.ScopesConsented.IsNullOrEmpty())
                {
                    var scopes = Consent.ScopesConsented;
                    if (ConsentOptions.EnableOfflineAccess == false)
                    {
                        scopes = scopes.Where(x => x != IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess).ToList();
                    }

                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = Consent.RememberConsent,
                        ScopesValuesConsented = scopes.ToArray(),
                        Description = Consent.Description
                    };

                    // emit event
                    //await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
                }
                else
                {
                    //throw new UserFriendlyException("You must pick at least one permission"); //TODO: How to handle this
                    result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
                }
            }
            else
            {
                result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
            }

            if (grantedConsent != null)
            {
                // communicate outcome of consent back to identityserver
                await _interaction.GrantConsentAsync(request, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = ReturnUrl;  //TODO: ReturnUrlHash?
                result.Client = request.Client;
            }
            else
            {
                // we need to redisplay the consent UI

                result.ViewModel = await BuildViewModelAsync(ReturnUrl, Consent);
            }

            return result;
        }

        private async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null)
        {
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (request != null)
            {
                return CreateConsentViewModel(model, returnUrl, request);
            }

            throw new ApplicationException($"No consent request matching request: {returnUrl}");
        }

        private ConsentViewModel CreateConsentViewModel(ConsentInputModel model, string returnUrl, AuthorizationRequest request)
        {
            var consentViewModel = new ConsentViewModel
            {
                RememberConsent = model?.RememberConsent ?? true,
                ScopesConsented = model?.ScopesConsented ?? new List<string>(),
                Description = model?.Description,

                ClientName = request.Client.ClientName ?? request.Client.ClientId,
                ClientUrl = request.Client.ClientUri,
                ClientLogoUrl = request.Client.LogoUri,
                AllowRememberConsent = request.Client.AllowRememberConsent
            };

            consentViewModel.IdentityScopes = request.ValidatedResources.Resources.IdentityResources.Select(x =>
                CreateScopeViewModel(x, consentViewModel.ScopesConsented.Contains(x.Name) || model == null))
                .ToList();

            var apiScopes = new List<ScopeViewModel>();
            foreach(var parsedScope in request.ValidatedResources.ParsedScopes)
            {
                var apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
                if (apiScope != null)
                {
                    var scopeVm = CreateScopeViewModel(parsedScope, apiScope,
                        consentViewModel.ScopesConsented.Contains(parsedScope.RawValue) || model == null);
                    apiScopes.Add(scopeVm);
                }
            }

            if (ConsentOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
            {
                apiScopes.Add(GetOfflineAccessScope(consentViewModel.ScopesConsented.Contains(IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess) || model == null));
            }

            consentViewModel.ApiScopes = apiScopes;

            return consentViewModel;
        }


        protected virtual ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new ScopeViewModel
            {
                Value = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
            };
        }

        protected virtual ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
        {
            var displayName = apiScope.DisplayName ?? apiScope.Name;
            if (!string.IsNullOrWhiteSpace(parsedScopeValue.ParsedParameter))
            {
                displayName += ":" + parsedScopeValue.ParsedParameter;
            }

            return new ScopeViewModel
            {
                Value = parsedScopeValue.RawValue,
                DisplayName = displayName,
                Description = apiScope.Description,
                Emphasize = apiScope.Emphasize,
                Required = apiScope.Required,
                Checked = check || apiScope.Required
            };
        }

        protected virtual ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
            {
                Value = IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = "Offline Access", //TODO: Localize
                Description = "Access to your applications and resources, even when you are offline",
                Emphasize = true,
                Checked = check
            };
        }

        public class ConsentInputModel
        {
            [Required]
            public string Button { get; set; }

            public List<string> ScopesConsented { get; set; }

            public bool RememberConsent { get; set; }

            public string Description { get; set; }
        }


        public class ConsentViewModel : ConsentInputModel
        {
            public string ClientName { get; set; }
            public string ClientUrl { get; set; }

            public string ClientLogoUrl { get; set; }

            public bool AllowRememberConsent { get; set; }

            public List<ScopeViewModel> IdentityScopes { get; set; }

            public List<ScopeViewModel> ApiScopes { get; set; }
        }

        public class ScopeViewModel
        {
            [Required]
            [HiddenInput]
            public string Value { get; set; }

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
            public Client Client { get; set; }

            public bool ShowView => ViewModel != null;
            public ConsentViewModel ViewModel { get; set; }

            public bool HasValidationError => ValidationError != null;
            public string ValidationError { get; set; }
        }
    }
}
