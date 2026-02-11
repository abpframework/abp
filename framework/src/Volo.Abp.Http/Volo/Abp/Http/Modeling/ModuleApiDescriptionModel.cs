using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Http.Modeling;

[Serializable]
public class ModuleApiDescriptionModel
{
    /// <summary>
    /// "app".
    /// </summary>
    public const string DefaultRootPath = "app";

    /// <summary>
    /// "Default".
    /// </summary>
    public const string DefaultRemoteServiceName = "Default";

    public string RootPath { get; set; } = default!;

    public string RemoteServiceName { get; set; } = default!;

    public IDictionary<string, ControllerApiDescriptionModel> Controllers { get; set; } = default!;

    public ModuleApiDescriptionModel()
    {

    }

    public static ModuleApiDescriptionModel Create(string rootPath, string remoteServiceName)
    {
        return new ModuleApiDescriptionModel
        {
            RootPath = rootPath,
            RemoteServiceName = remoteServiceName,
            Controllers = new SortedDictionary<string, ControllerApiDescriptionModel>()
        };
    }

    public ControllerApiDescriptionModel AddController(ControllerApiDescriptionModel controller)
    {
        if (Controllers.ContainsKey(controller.Type))
        {
            throw new AbpException($"There is already a controller with type: {controller.Type} in module: {RootPath}");
        }

        return Controllers[controller.Type] = controller;
    }

    public ControllerApiDescriptionModel GetOrAddController(string name, string? groupName, bool isRemoteService, bool isIntegrationService, string? apiVersion, Type type, HashSet<Type>? ignoredInterfaces = null)
    {
        var key = (apiVersion.IsNullOrWhiteSpace() ? type.FullName : $"{apiVersion + "."}{type.FullName}")!;
        return Controllers.GetOrAdd(key, () => ControllerApiDescriptionModel.Create(name, groupName, isRemoteService, isIntegrationService, apiVersion, type, ignoredInterfaces));
    }

    public ModuleApiDescriptionModel CreateSubModel(string[]? controllers, string[]? actions)
    {
        var subModel = Create(RootPath, RemoteServiceName);

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
