using System;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiSecret : Secret
    {
        public virtual string ApiResourceId { get; set; }

        protected ApiSecret()
        {
            
        }

        public ApiSecret(Guid id)
            : base(id)
        {
            
        }
    }
}