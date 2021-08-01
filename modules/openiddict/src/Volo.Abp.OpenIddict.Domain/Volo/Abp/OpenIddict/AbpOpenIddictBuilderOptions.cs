using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.OpenIddict
{
    public class AbpOpenIddictBuilderOptions
    {
        /// <summary>
        /// Updates <see cref="JwtSecurityTokenHandler.DefaultInboundClaimTypeMap"/> to be compatible with openiddict server claims.
        /// Default: true.
        /// </summary>
        public bool UpdateJwtSecurityTokenHandlerDefaultInboundClaimTypeMap { get; set; } = true;

        /// <summary>
        /// Updates <see cref="AbpClaimTypes"/> to be compatible with openiddict server claims.
        /// Default: true.
        /// </summary>
        public bool UpdateAbpClaimTypes { get; set; } = true;

        /// <summary>
        /// Set false to suppress AddDeveloperSigningCredential() call on the OpenIddictServerBuilder.
        /// </summary>
        public bool AddDeveloperSigningCredential { get; set; } = true;

        public bool RequireProofKeyForCodeExchange { get; set; } = true;
    }
}
