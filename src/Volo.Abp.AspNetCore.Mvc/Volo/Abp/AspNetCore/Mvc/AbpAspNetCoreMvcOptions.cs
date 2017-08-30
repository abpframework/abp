using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpAspNetCoreMvcOptions
    {
        //TODO: Group into a class since they are related.
        public ControllerAssemblySettingList ControllerAssemblySettings { get; }
        public List<Type> FormBodyBindingIgnoredTypes { get; }

        public AbpAspNetCoreMvcOptions()
        {
            FormBodyBindingIgnoredTypes = new List<Type>
            {
                typeof(IFormFile)
            };
        }

        public AbpControllerAssemblySettingBuilder CreateControllersForAppServices(
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