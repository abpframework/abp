using System;
using JetBrains.Annotations;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiResourceClaim : UserClaim
    {
        public virtual Guid ApiResourceId { get; set; }

        protected ApiResourceClaim()
        {

        }

        public virtual bool Equals(Guid apiResourceId, [NotNull] string type)
        {
            return ApiResourceId == apiResourceId && Type == type;
        }

        protected internal ApiResourceClaim(Guid apiResourceId, [NotNull] string type)
            : base(type)
        {
            ApiResourceId = apiResourceId;
        }

        public override object[] GetKeys()
        {
            return new object[] {ApiResourceId, Type};
        }
    }
}
