using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Http;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AppServiceControllerOptions
    {
        public ControllerAssemblySettingList ControllerAssemblySettings { get; }

        public List<Type> FormBodyBindingIgnoredTypes { get; }
        
        public List<IUrlActionNameNormalizer> UrlActionNameNormalizers { get; }

        public AppServiceControllerOptions()
        {
            ControllerAssemblySettings = new ControllerAssemblySettingList();

            FormBodyBindingIgnoredTypes = new List<Type>
            {
                typeof(IFormFile)
            };

            UrlActionNameNormalizers = new List<IUrlActionNameNormalizer>
            {
                new DefaultUrlActionNameNormalizer()
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