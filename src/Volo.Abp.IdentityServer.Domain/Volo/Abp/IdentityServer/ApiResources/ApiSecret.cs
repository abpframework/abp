using System;
using IdentityServer4;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiSecret : Secret
    {
        public virtual Guid ApiResourceId { get; set; }

        protected ApiSecret()
        {
            
        }

        public ApiSecret(Guid id, string value, DateTime? expiration = null, string type = IdentityServerConstants.SecretTypes.SharedSecret, string description = null)
            : base(id, value, expiration, type, description)
        {
            
        }
    }
}