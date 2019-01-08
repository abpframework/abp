using System;
using IdentityServer4;
using JetBrains.Annotations;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiSecret : Secret
    {
        public virtual Guid ApiResourceId { get; protected set; }

        protected ApiSecret()
        {

        }

        public virtual bool Equals(Guid apiResourceId, [NotNull] string value, string type = IdentityServerConstants.SecretTypes.SharedSecret)
        {
            return ApiResourceId == apiResourceId && Value == value && Type == type;
        }

        protected internal ApiSecret(
            Guid apiResourceId,
            [NotNull] string value, 
            DateTime? expiration = null, 
            string type = IdentityServerConstants.SecretTypes.SharedSecret, 
            string description = null
            ) : base(
                value, 
                expiration, 
                type, 
                description)
        {
            ApiResourceId = apiResourceId;
        }

        public override object[] GetKeys()
        {
            return new object[] { ApiResourceId, Type, Value };
        }
    }
}