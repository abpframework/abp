using System;
using JetBrains.Annotations;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiScopeClaim : UserClaim
    {
        public Guid ApiResourceId { get; protected set; }

        [NotNull]
        public string Name { get; protected set; }

        protected ApiScopeClaim()
        {

        }

        protected internal ApiScopeClaim(Guid apiResourceId, [NotNull] string name, [NotNull] string type)
            : base(type)
        {
            Check.NotNull(name, nameof(name));

            ApiResourceId = apiResourceId;
            Name = name;
        }
    }
}