using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ApplicationApiDescriptionModel
    {
        public IDictionary<string, ModuleApiDescriptionModel> Modules { get; set; }

        private ApplicationApiDescriptionModel()
        {
            
        }

        public static ApplicationApiDescriptionModel Create()
        {
            return new ApplicationApiDescriptionModel
            {
                //TODO: Why ConcurrentDictionary?
                Modules = new ConcurrentDictionary<string, ModuleApiDescriptionModel>()
            };
        }

        public ModuleApiDescriptionModel AddModule(ModuleApiDescriptionModel module)
        {
            if (Modules.ContainsKey(module.Name))
            {
                throw new AbpException("There is already a module with same name: " + module.Name);
            }

            return Modules[module.Name] = module;
        }

        public ModuleApiDescriptionModel GetOrAddModule(string name)
        {
            return Modules.GetOrAdd(name, () => ModuleApiDescriptionModel.Create(name));
        }

        public ApplicationApiDescriptionModel CreateSubModel(string[] modules = null, string[] controllers = null, string[] actions = null)
        {
            var subModel = new ApplicationApiDescriptionModel();

            foreach (var module in Modules.Values)
            {
                if (modules == null || modules.Contains(module.Name))
                {
                    subModel.AddModule(module.CreateSubModel(controllers, actions));
                }
            }

            return subModel;
        }
    }
}