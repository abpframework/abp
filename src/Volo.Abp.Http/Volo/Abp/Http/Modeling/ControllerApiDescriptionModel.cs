using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ControllerApiDescriptionModel
    {
        public string ControllerName { get; set; }

        public string TypeAsString { get; set; }

        public List<ControllerInterfaceApiDescriptionModel> Interfaces { get; set; }

        public Dictionary<string,  ActionApiDescriptionModel> Actions { get; set; }

        private ControllerApiDescriptionModel()
        {

        }

        public static ControllerApiDescriptionModel Create(string controllerName, Type type)
        {
            return new ControllerApiDescriptionModel
            {
                ControllerName = controllerName,
                TypeAsString = type.FullName,
                Actions = new Dictionary<string, ActionApiDescriptionModel>(),
                Interfaces = type
                    .GetInterfaces()
                    .Select(ControllerInterfaceApiDescriptionModel.Create)
                    .ToList()
            };
        }

        public ActionApiDescriptionModel AddAction(ActionApiDescriptionModel action)
        {
            if (Actions.ContainsKey(action.UniqueName))
            {
                throw new AbpException(
                    $"Can not add more than one action with same name to the same controller. Controller: {ControllerName}, Action: {action.UniqueName}."
                    );
            }

            return Actions[action.UniqueName] = action;
        }

        public ControllerApiDescriptionModel CreateSubModel(string[] actions)
        {
            var subModel = new ControllerApiDescriptionModel
            {
                TypeAsString = TypeAsString,
                Interfaces = Interfaces,
                ControllerName = ControllerName,
                Actions = new Dictionary<string, ActionApiDescriptionModel>()
            };

            foreach (var action in Actions.Values)
            {
                if (actions == null || actions.Contains(action.UniqueName))
                {
                    subModel.AddAction(action);
                }
            }

            return subModel;
        }

        public bool Implements(Type interfaceType)
        {
            return Interfaces.Any(i => i.TypeAsString == interfaceType.AssemblyQualifiedName);
        }
    }
}