using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AppServiceControllerOptions
    {
        public ControllerAssemblySettingList ControllerAssemblySettings { get; }

        public List<Type> FormBodyBindingIgnoredTypes { get; }
        
        public AppServiceControllerOptions()
        {
            ControllerAssemblySettings = new ControllerAssemblySettingList();

            FormBodyBindingIgnoredTypes = new List<Type>
            {
                typeof(IFormFile)
            };
        }

        public AppServiceControllerOptions Create(Assembly assembly, [CanBeNull] Action<AbpControllerAssemblySetting> optionsAction = null)
        {
            var setting = new AbpControllerAssemblySetting(assembly, ModuleApiDescriptionModel.DefaultRootPath);
            optionsAction?.Invoke(setting);
            setting.Initialize();
            ControllerAssemblySettings.Add(setting);
            return this;
        }
    }
}