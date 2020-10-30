using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;

namespace Volo.Abp.AspNetCore.Mvc.Json
{
    public class AbpMvcJsonContractResolver : DefaultContractResolver, ITransientDependency
    {
        private readonly AbpJsonIsoDateTimeConverter _dateTimeConverter;

        public AbpMvcJsonContractResolver(AbpJsonIsoDateTimeConverter dateTimeConverter)
        {
            _dateTimeConverter = dateTimeConverter;

            NamingStrategy = new CamelCaseNamingStrategy();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            ModifyProperty(member, property);

            return property;
        }

        protected virtual void ModifyProperty(MemberInfo member, JsonProperty property)
        {
            if (property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(DateTime?))
            {
                return;
            }

            if (ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableDateTimeNormalizationAttribute>(member) == null)
            {
                property.Converter = _dateTimeConverter;
            }
        }
    }
}
