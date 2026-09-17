using System;
using System.Collections.Generic;

namespace Volo.Abp.AI;

public class WorkspaceConfigurationDictionary : Dictionary<string, WorkspaceConfiguration>
{
    public void Configure<TWorkSpace>(Action<WorkspaceConfiguration>? configureAction = null)
        where TWorkSpace : class
    {
        Configure(WorkspaceNameAttribute.GetWorkspaceName<TWorkSpace>(), configureAction);
    }

    public void Configure(string name, Action<WorkspaceConfiguration>? configureAction = null)
    {
        if (!TryGetValue(name, out var configuration))
        {
            configuration = new WorkspaceConfiguration(name);
            this[name] = configuration;
        }

        configureAction?.Invoke(configuration);
    }

    public void ConfigureDefault(Action<WorkspaceConfiguration>? configureAction = null)
    {
        Configure(AbpAIModule.DefaultWorkspaceName, configureAction);
    }
}
