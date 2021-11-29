using System;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiResourceProperty : Entity
    {
        public virtual Guid ApiResourceId { get; protected set; }

        public virtual string Key { get; set; }

        public virtual string Value { get; set; }

        protected ApiResourceProperty()
        {

        }

        public virtual bool Equals(Guid aiResourceId, [NotNull] string key, string value)
        {
            return ApiResourceId == aiResourceId && Key == key && Value == value;
        }

        protected internal ApiResourceProperty(Guid aiResourceId, [NotNull] string key, [NotNull] string value)
        {
            Check.NotNull(key, nameof(key));

            ApiResourceId = aiResourceId;
            Key = key;
            Value = value;
        }

        public override object[] GetKeys()
        {
            return new object[] { ApiResourceId, Key };
        }
    }
}
