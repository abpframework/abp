using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Volo.Abp.Auditing
{
    public class AuditingContractResolver : CamelCasePropertyNamesContractResolver
    {
        private readonly List<Type> _ignoredTypes;

        public AuditingContractResolver(List<Type> ignoredTypes)
        {
            _ignoredTypes = ignoredTypes;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (_ignoredTypes.Any(ignoredType => ignoredType.GetTypeInfo().IsAssignableFrom(property.PropertyType)))
            {
                property.ShouldSerialize = instance => false;
                return property;
            }

            if (member.DeclaringType != null && (member.DeclaringType.IsDefined(typeof(DisableAuditingAttribute)) || member.DeclaringType.IsDefined(typeof(JsonIgnoreAttribute))))
            {
                property.ShouldSerialize = instance => false;
                return property;
            }

            if (member.IsDefined(typeof(DisableAuditingAttribute)) || member.IsDefined(typeof(JsonIgnoreAttribute)))
            {
                property.ShouldSerialize = instance => false;
            }

            return property;
        }
    }
}
