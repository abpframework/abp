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

        public virtual bool Equals(Guid apiResourceId, [NotNull] string name, [NotNull] string type)
        {
            return ApiResourceId == apiResourceId && Name == name && Type == type;
        }

        protected internal ApiScopeClaim(Guid apiResourceId, [NotNull] string name, [NotNull] string type)
            : base(type)
        {
            Check.NotNull(name, nameof(name));

            ApiResourceId = apiResourceId;
            Name = name;
        }

        public override object[] GetKeys()
        {
            return new object[] { ApiResourceId, Name, Type };
        }
    }
}