using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourceProperty : Entity
    {
        public virtual Guid IdentityResourceId { get; set; }

        public virtual string Key { get; set; }

        public virtual string Value { get; set; }

        protected IdentityResourceProperty()
        {

        }

        public virtual bool Equals(Guid identityResourceId, [NotNull] string key, string value)
        {
            return IdentityResourceId == identityResourceId && Key == key && Value == value;
        }

        protected internal IdentityResourceProperty(Guid identityResourceId, [NotNull] string key, [NotNull] string value)
        {
            Check.NotNull(key, nameof(key));

            IdentityResourceId = identityResourceId;
            Key = key;
            Value = value;
        }

        public override object[] GetKeys()
        {
            return new object[] { IdentityResourceId, Key };
        }
    }
}
