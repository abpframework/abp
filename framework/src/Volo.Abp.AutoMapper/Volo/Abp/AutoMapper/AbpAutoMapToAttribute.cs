using System;
using System.Collections.Generic;
using AutoMapper;

namespace Volo.Abp.AutoMapper
{
    public class AbpAutoMapToAttribute : AbpAutoMapAttributeBase
    {
        public MemberList MemberList { get; set; } = MemberList.Source;

        public AbpAutoMapToAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {

        }

        public AbpAutoMapToAttribute(MemberList memberList, params Type[] targetTypes)
            : this(targetTypes)
        {
            MemberList = memberList;
        }

        public override void CreateMap(IMapperConfigurationExpression configuration, Type type)
        {
            if (TargetTypes.IsNullOrEmpty())
            {
                return;
            }

            foreach (var targetType in TargetTypes)
            {
                configuration.CreateMap(type, targetType, MemberList);
            }
        }
    }
}