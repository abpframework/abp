using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class AbpControllerAssemblySetting
    {
        /// <summary>
        /// "app".
        /// </summary>
        public const string DefaultServiceModuleName = "app";

        public string ModuleName { get; }

        public Assembly Assembly { get; }

        public bool UseConventionalHttpVerbs { get; }

        public Func<Type, bool> TypePredicate { get; set; }

        public Action<ControllerModel> ControllerModelConfigurer { get; set; }

        public AbpControllerAssemblySetting(string moduleName, Assembly assembly, bool useConventionalHttpVerbs)
        {
            ModuleName = moduleName;
            Assembly = assembly;
            UseConventionalHttpVerbs = useConventionalHttpVerbs;

            TypePredicate = type => true;
            ControllerModelConfigurer = controller => { };
        }
    }
}