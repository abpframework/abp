using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.AspNetCore.Mvc.Conventions
{
    public class AbpConventionalControllerOptions
    {
        public ConventionalControllerSettingList ConventionalControllerSettings { get; }

        public List<Type> FormBodyBindingIgnoredTypes { get; }
        
        public AbpConventionalControllerOptions()
        {
            ConventionalControllerSettings = new ConventionalControllerSettingList();

            FormBodyBindingIgnoredTypes = new List<Type>
            {
                typeof(IFormFile)
            };
        }

        public AbpConventionalControllerOptions Create(Assembly assembly, [CanBeNull] Action<ConventionalControllerSetting> optionsAction = null)
        {
            var setting = new ConventionalControllerSetting(assembly, ModuleApiDescriptionModel.DefaultRootPath);
            optionsAction?.Invoke(setting);
            setting.Initialize();
            ConventionalControllerSettings.Add(setting);
            return this;
        }
    }
}