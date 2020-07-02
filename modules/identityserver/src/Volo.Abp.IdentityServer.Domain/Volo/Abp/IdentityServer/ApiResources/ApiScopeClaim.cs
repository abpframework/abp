using System;
using JetBrains.Annotations;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiScopeClaim : UserClaim
    {
        public Guid ApiScopeId { get; protected set; }

        [NotNull]
        public string Name { get; protected set; }

        protected ApiScopeClaim()
        {

        }

        public virtual bool Equals(Guid apiScopeId, [NotNull] string name, [NotNull] string type)
        {
            return ApiScopeId == apiScopeId && Name == name && Type == type;
        }

        protected internal ApiScopeClaim(Guid apiScopeId, [NotNull] string name, [NotNull] string type)
            : base(type)
        {
            Check.NotNull(name, nameof(name));

            ApiScopeId = apiScopeId;
            Name = name;
        }

        public override object[] GetKeys()
        {
            return new object[] { ApiScopeId, Name, Type };
        }
    }
}
