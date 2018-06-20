using System;
using AutoMapper;

namespace Volo.Abp.AutoMapper
{
    public abstract class AutoMapAttributeBase : Attribute
    {
        public Type[] TargetTypes { get; }

        protected AutoMapAttributeBase(params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }

        public abstract void CreateMap(IMapperConfigurationExpression configuration, Type type);
    }
}