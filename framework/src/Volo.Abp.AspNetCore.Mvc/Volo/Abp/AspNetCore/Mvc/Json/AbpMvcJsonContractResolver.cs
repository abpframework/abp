using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;
using Volo.Abp.Json.Newtonsoft;
using Volo.Abp.Reflection;
using Volo.Abp.Timing;

namespace Volo.Abp.AspNetCore.Mvc.Json
{
    public class AbpMvcJsonContractResolver : DefaultContractResolver
    {
        private readonly Lazy<AbpJsonIsoDateTimeConverter> _dateTimeConverter;
        public AbpMvcJsonContractResolver(IServiceCollection services)
        {
            _dateTimeConverter = services.GetServiceLazy<AbpJsonIsoDateTimeConverter>();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

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
                property.Converter = _dateTimeConverter.Value;
            }

        }
    }
}
