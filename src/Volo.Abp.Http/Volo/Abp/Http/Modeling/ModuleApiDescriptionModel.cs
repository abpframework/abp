using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ModuleApiDescriptionModel
    {
        /// <summary>
        /// "app".
        /// </summary>
        public const string DefaultModuleName = "app";

        public string Name { get; set; }

        public IDictionary<string, ControllerApiDescriptionModel> Controllers { get; set; }

        private ModuleApiDescriptionModel()
        {
            
        }

        public static ModuleApiDescriptionModel Create(string name)
        {
            return new ModuleApiDescriptionModel
            {
                Name = name,
                Controllers = new Dictionary<string, ControllerApiDescriptionModel>()
            };
        }

        public ControllerApiDescriptionModel AddController(ControllerApiDescriptionModel controller)
        {
            if (Controllers.ContainsKey(controller.ControllerName))
            {
                throw new AbpException($"There is already a controller with name: {controller.ControllerName} in module: {Name}");
            }

            return Controllers[controller.ControllerName] = controller;
        }

        public ControllerApiDescriptionModel GetOrAddController(string name, Type type)
        {
            return Controllers.GetOrAdd(name, () => ControllerApiDescriptionModel.Create(name, type));
        }
        
        public ModuleApiDescriptionModel CreateSubModel(string[] controllers, string[] actions)
        {
            var subModel = Create(Name);

            foreach (var controller in Controllers.Values)
            {
                if (controllers == null || controllers.Contains(controller.ControllerName))
                {
                    subModel.AddController(controller.CreateSubModel(actions));
                }
            }

            return subModel;
        }
    }
}