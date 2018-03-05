using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.RazorPages;

namespace Volo.Abp.Account.Web.Pages.Account
{
    public class GrantsModel : AbpPageModel
    {
        public List<GrantViewModel> Grants { get; set; }

        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clients;
        private readonly IResourceStore _resources;

        public GrantsModel(IIdentityServerInteractionService interaction,
            IClientStore clients,
            IResourceStore resources)
        {
            _interaction = interaction;
            _clients = clients;
            _resources = resources;
        }
        
        public async Task OnGet()
        {
            Grants = new List<GrantViewModel>();

            foreach (var consent in await _interaction.GetAllUserConsentsAsync())
            {
                var client = await _clients.FindClientByIdAsync(consent.ClientId);
                if (client != null)
                {
                    var resources = await _resources.FindResourcesByScopeAsync(consent.Scopes);

                    var item = new GrantViewModel
                    {
                        ClientId = client.ClientId,
                        ClientName = client.ClientName ?? client.ClientId,
                        ClientLogoUrl = client.LogoUri,
                        ClientUrl = client.ClientUri,
                        Created = consent.CreationTime,
                        Expires = consent.Expiration,
                        IdentityGrantNames = resources.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                        ApiGrantNames = resources.ApiResources.Select(x => x.DisplayName ?? x.Name).ToArray()
                    };

                    Grants.Add(item);
                }
            }
        }

        public async Task<IActionResult> OnPostRevokeAsync(string clientId)
        {
            await _interaction.RevokeUserConsentAsync(clientId);
            return Redirect("/"); //TODO: ..?
        }

        public class GrantViewModel
        {
            public string ClientId { get; set; }
            public string ClientName { get; set; }
            public string ClientUrl { get; set; }
            public string ClientLogoUrl { get; set; }
            public DateTime Created { get; set; }
            public DateTime? Expires { get; set; }
            public IEnumerable<string> IdentityGrantNames { get; set; }
            public IEnumerable<string> ApiGrantNames { get; set; }
        }
    }
}