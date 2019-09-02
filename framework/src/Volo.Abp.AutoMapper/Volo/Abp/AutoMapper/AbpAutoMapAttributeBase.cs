using System;
using AutoMapper;

namespace Volo.Abp.AutoMapper
{
    public abstract class AbpAutoMapAttributeBase : Attribute
    {
        public Type[] TargetTypes { get; }

        protected AbpAutoMapAttributeBase(params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }

        public abstract void CreateMap(IMapperConfigurationExpression configuration, Type type);
    }
}