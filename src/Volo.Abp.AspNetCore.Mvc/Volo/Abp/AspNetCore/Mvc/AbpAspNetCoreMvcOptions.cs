using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpAspNetCoreMvcOptions
    {
        public AppServiceControllerOptions AppServiceControllers { get; }

        public AbpAspNetCoreMvcOptions()
        {
            AppServiceControllers = new AppServiceControllerOptions();
        }
    }

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

        public AbpControllerAssemblySettingBuilder CreateFor(
            Assembly assembly,
            string moduleName = AbpControllerAssemblySetting.DefaultServiceModuleName,
            bool useConventionalHttpVerbs = true)
        {
            var setting = new AbpControllerAssemblySetting(moduleName, assembly, useConventionalHttpVerbs);
            ControllerAssemblySettings.Add(setting);
            return new AbpControllerAssemblySettingBuilder(setting);
        }
    }
}