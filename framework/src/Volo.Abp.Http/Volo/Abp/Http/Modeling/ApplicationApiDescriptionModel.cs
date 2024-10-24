using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Http.Modeling;

[Serializable]
public class ApplicationApiDescriptionModel
{
    public IDictionary<string, ModuleApiDescriptionModel> Modules { get; set; } = default!;

    public IDictionary<string, TypeApiDescriptionModel> Types { get; set; } = default!;

    public ApplicationApiDescriptionModel()
    {

    }

    public static ApplicationApiDescriptionModel Create()
    {
        return new ApplicationApiDescriptionModel
        {
            Modules = new Dictionary<string, ModuleApiDescriptionModel>(), 
            Types = new SortedDictionary<string, TypeApiDescriptionModel>()
        };
    }

    public ModuleApiDescriptionModel AddModule(ModuleApiDescriptionModel module)
    {
        if (Modules.ContainsKey(module.RootPath))
        {
            throw new AbpException("There is already a module with same root path: " + module.RootPath);
        }

        return Modules[module.RootPath] = module;
    }

    public ModuleApiDescriptionModel GetOrAddModule(string rootPath, string remoteServiceName)
    {
        return Modules.GetOrAdd(rootPath, () => ModuleApiDescriptionModel.Create(rootPath, remoteServiceName));
    }

    public ApplicationApiDescriptionModel CreateSubModel(string[]? modules = null, string[]? controllers = null, string[]? actions = null)
    {
        var subModel = ApplicationApiDescriptionModel.Create(); ;

        foreach (var module in Modules.Values)
        {
            if (modules == null || modules.Contains(module.RootPath))
            {
                subModel.AddModule(module.CreateSubModel(controllers, actions));
            }
        }

        return subModel;
    }

    public void NormalizeOrder()
    {
        Modules = Modules.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
    }
}
