using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Threading;

namespace IdentityServerHost
{
    public class IdentityServerDataSeeder : ITransientDependency
    {
        private readonly IApiResourceRepository _apiResourceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityResourceRepository _identityResourceRepository;

        public IdentityServerDataSeeder(
            IClientRepository clientRepository, 
            IApiResourceRepository apiResourceRepository, 
            IIdentityResourceRepository identityResourceRepository)
        {
            _clientRepository = clientRepository;
            _apiResourceRepository = apiResourceRepository;
            _identityResourceRepository = identityResourceRepository;
        }

        public void Seed()
        {
            AsyncHelper.RunSync(SeedAsync);
        }

        private async Task SeedAsync()
        {
            if (await _clientRepository.FindByCliendIdAsync("test-client") != null)
            {
                return;
            }

            await SaveApiResource();
            await SaveClientAsync();
            await SaveIdentityResourcesAsync();
        }

        private async Task SaveApiResource()
        {
            var apiResource = new ApiResource(
                Guid.NewGuid(),
                "api1",
                "My API",
                "My api resource description"
            );

            apiResource.AddUserClaim("email");
            apiResource.AddUserClaim("role");

            await _apiResourceRepository.InsertAsync(apiResource);
        }

        private async Task SaveClientAsync()
        {
            var client = new Client(
                Guid.NewGuid(), 
                "test-client"
                )
            {
                ClientName = "test-client",
                ProtocolType = "oidc",
                Description = "test-client",
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowOfflineAccess = true,
                AbsoluteRefreshTokenLifetime = 31536000 //365 days
            };

            client.AddScope("api1");
            client.AddScope("email");
            client.AddScope("openid");
            client.AddScope("profile");
            client.AddScope("roles");
            client.AddScope("unique_name");

            client.AddGrantType("client_credentials");
            client.AddGrantType("password");

            client.AddSecret("K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=");

            await _clientRepository.InsertAsync(client);
        }

        private async Task SaveIdentityResourcesAsync()
        {
            var identityResourceOpenId = new IdentityResource(Guid.NewGuid(), "openid", "OpenID", required: true);
            await _identityResourceRepository.InsertAsync(identityResourceOpenId);

            var identityResourceEmail = new IdentityResource(Guid.NewGuid(), "email", "Email", required: true);
            identityResourceEmail.AddUserClaim("email");
            identityResourceEmail.AddUserClaim("email_verified");
            await _identityResourceRepository.InsertAsync(identityResourceEmail);

            var identityResourceRole = new IdentityResource(Guid.NewGuid(), "roles", "Roles", required: true);
            identityResourceRole.AddUserClaim("role");
            await _identityResourceRepository.InsertAsync(identityResourceRole);

            var identityResourceProfile = new IdentityResource(Guid.NewGuid(), "profile", "Profile", required: true);
            identityResourceProfile.AddUserClaim("unique_name");
            await _identityResourceRepository.InsertAsync(identityResourceProfile);
        }
    }
}
