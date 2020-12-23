using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.ApiScopes
{
    public class ApiScopeProperty : Entity
    {
        public virtual Guid ApiScopeId { get; set; }

        public virtual string Key { get; set; }

        public virtual string Value { get; set; }

        protected ApiScopeProperty()
        {

        }

        public virtual bool Equals(Guid apiScopeId, [NotNull] string key, string value)
        {
            return ApiScopeId == apiScopeId && Key == key && Value == value;
        }

        protected internal ApiScopeProperty(Guid apiScopeId, [NotNull] string key, [NotNull] string value)
        {
            Check.NotNull(key, nameof(key));

            ApiScopeId = apiScopeId;
            Key = key;
            Value = value;
        }

        public override object[] GetKeys()
        {
            return new object[] { ApiScopeId, Key };
        }
    }
}
