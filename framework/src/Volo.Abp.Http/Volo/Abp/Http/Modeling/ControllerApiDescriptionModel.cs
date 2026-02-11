using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Http.Modeling;

[Serializable]
public class ControllerApiDescriptionModel
{
    public string ControllerName { get; set; } = default!;

    public string? ControllerGroupName { get; set; }

    public bool IsRemoteService { get; set; }

    public bool IsIntegrationService { get; set; }

    public string? ApiVersion { get; set; }

    public string Type { get; set; } = default!;

    public List<ControllerInterfaceApiDescriptionModel> Interfaces { get; set; } = default!;

    public Dictionary<string, ActionApiDescriptionModel> Actions { get; set; } = default!;

    public ControllerApiDescriptionModel()
    {

    }

    public static ControllerApiDescriptionModel Create(string controllerName, string? groupName, bool isRemoteService, bool isIntegrationService, string? apiVersion, Type type, HashSet<Type>? ignoredInterfaces = null)
    {
        return new ControllerApiDescriptionModel
        {
            ControllerName = controllerName,
            ControllerGroupName = groupName,
            IsRemoteService = isRemoteService,
            IsIntegrationService = isIntegrationService, //IntegrationServiceAttribute.IsDefinedOrInherited(type),
            ApiVersion = apiVersion,
            Type = type.FullName!,
            Actions = new Dictionary<string, ActionApiDescriptionModel>(),
            Interfaces = type
                .GetInterfaces()
                .WhereIf(ignoredInterfaces != null, i => !i.IsGenericType && !ignoredInterfaces!.Contains(i))
                .Select(ControllerInterfaceApiDescriptionModel.Create)
                .ToList()
        };
    }

    public ActionApiDescriptionModel AddAction(string uniqueName, ActionApiDescriptionModel action)
    {
        if (Actions.ContainsKey(uniqueName))
        {
            throw new AbpException(
                $"Can not add more than one action with same name to the same controller. Controller: {ControllerName}, Action: {action.Name}."
            );
        }

        return Actions[uniqueName] = action;
    }

    public ControllerApiDescriptionModel CreateSubModel(string[]? actions)
    {
        var subModel = new ControllerApiDescriptionModel
        {
            Type = Type,
            Interfaces = Interfaces,
            ControllerName = ControllerName,
            Actions = new Dictionary<string, ActionApiDescriptionModel>()
        };

        foreach (var action in Actions)
        {
            if (actions == null || actions.Contains(action.Key))
            {
                subModel.AddAction(action.Key, action.Value);
            }
        }

        return subModel;
    }

    public bool Implements(Type interfaceType)
    {
        return Interfaces.Any(i => i.Type == interfaceType.FullName);
    }

    public override string ToString()
    {
        return $"[ControllerApiDescriptionModel {ControllerName}]";
    }
}
