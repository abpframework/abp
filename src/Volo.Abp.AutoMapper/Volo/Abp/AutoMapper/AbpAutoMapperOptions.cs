using System;
using System.Collections.Generic;
using AutoMapper;
using Volo.Abp.Collections;

namespace Volo.Abp.AutoMapper
{
    public class AbpAutoMapperOptions
    {
        public List<Action<IAbpAutoMapperConfigurationContext>> Configurators { get; }

        public ITypeList<Profile> ValidatingProfiles { get; set; }

        public bool UseStaticMapper { get; set; }

        public AbpAutoMapperOptions()
        {
            UseStaticMapper = true;
            Configurators = new List<Action<IAbpAutoMapperConfigurationContext>>();
            ValidatingProfiles = new TypeList<Profile>();
        }

        public void AddProfile<TProfile>(bool validate = false)
            where TProfile: Profile, new()
        {
            Configurators.Add(context =>
            {
                context.MapperConfiguration.AddProfile<TProfile>();
            });

            if (validate)
            {
                ValidatingProfiles.Add<TProfile>();
            }
        }
    }
}