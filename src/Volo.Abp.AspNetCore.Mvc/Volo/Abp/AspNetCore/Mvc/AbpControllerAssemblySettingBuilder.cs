using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpControllerAssemblySettingBuilder : IAbpControllerAssemblySettingBuilder
    {
        private readonly AbpControllerAssemblySetting _setting;

        public AbpControllerAssemblySettingBuilder(AbpControllerAssemblySetting setting)
        {
            _setting = setting;
        }

        public AbpControllerAssemblySettingBuilder Where(Func<Type, bool> predicate)
        {
            _setting.TypePredicate = predicate;
            return this;
        }

        public AbpControllerAssemblySettingBuilder ConfigureControllerModel(Action<ControllerModel> configurer)
        {
            _setting.ControllerModelConfigurer = configurer;
            return this;
        }
    }
}