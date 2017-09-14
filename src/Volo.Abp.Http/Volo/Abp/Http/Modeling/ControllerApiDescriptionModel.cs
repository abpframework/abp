using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Http.Modeling
{
    [Serializable]
    public class ControllerApiDescriptionModel
    {
        public string Name { get; }

        public Type Type { get; }

        public string TypeAsString { get; }

        public IDictionary<string,  ActionApiDescriptionModel> Actions { get; }

        private ControllerApiDescriptionModel()
        {

        }

        public ControllerApiDescriptionModel(string name, Type type)
        {
            Name = name;
            Type = type;
            TypeAsString = type.FullName;

            Actions = new Dictionary<string, ActionApiDescriptionModel>();
        }

        public ActionApiDescriptionModel AddAction(ActionApiDescriptionModel action)
        {
            if (Actions.ContainsKey(action.Name))
            {
                throw new AbpException(
                    $"Can not add more than one action with same name to the same controller. Controller: {Name}, Action: {action.Name}."
                    );
            }

            return Actions[action.Name] = action;
        }

        public ControllerApiDescriptionModel CreateSubModel(string[] actions)
        {
            var subModel = new ControllerApiDescriptionModel(Name, Type);

            foreach (var action in Actions.Values)
            {
                if (actions == null || actions.Contains(action.Name))
                {
                    subModel.AddAction(action);
                }
            }

            return subModel;
        }
    }
}