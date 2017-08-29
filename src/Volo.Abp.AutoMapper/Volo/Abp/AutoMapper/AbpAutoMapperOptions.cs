using System;
using System.Collections.Generic;

namespace Volo.Abp.AutoMapper
{
    public class AbpAutoMapperOptions
    {
        public List<Action<IAbpAutoMapperConfigurationContext>> Configurators { get; }

        public bool UseStaticMapper { get; set; }

        public AbpAutoMapperOptions()
        {
            UseStaticMapper = true;
            Configurators = new List<Action<IAbpAutoMapperConfigurationContext>>();
        }
    }
}