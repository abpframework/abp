using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

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

    public string RootPath { get; set; }

    public string RemoteServiceName { get; set; }

    public IDictionary<string, ControllerApiDescriptionModel> Controllers { get; set; }

    public ModuleApiDescriptionModel()
    {

    }

    public static ModuleApiDescriptionModel Create(string rootPath, string remoteServiceName)
    {
        return new ModuleApiDescriptionModel
        {
            RootPath = rootPath,
            RemoteServiceName = remoteServiceName,
            Controllers = new Dictionary<string, ControllerApiDescriptionModel>()
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

    public ControllerApiDescriptionModel GetOrAddController(string name, string groupName, Type type, [CanBeNull] HashSet<Type> ignoredInterfaces = null)
    {
        return Controllers.GetOrAdd(type.FullName, () => ControllerApiDescriptionModel.Create(name, groupName, type, ignoredInterfaces));
    }

    public ModuleApiDescriptionModel CreateSubModel(string[] controllers, string[] actions)
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
