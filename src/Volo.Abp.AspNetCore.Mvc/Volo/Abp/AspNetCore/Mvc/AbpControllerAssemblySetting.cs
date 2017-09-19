using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpControllerAssemblySetting
    {
        public string ModuleName { get; }

        public Assembly Assembly { get; }

        public Func<Type, bool> TypePredicate { get; set; }

        public Action<ControllerModel> ControllerModelConfigurer { get; set; }

        public AbpControllerAssemblySetting(string moduleName, Assembly assembly)
        {
            ModuleName = moduleName;
            Assembly = assembly;

            TypePredicate = type => true;
            ControllerModelConfigurer = controller => { };
        }
    }
}