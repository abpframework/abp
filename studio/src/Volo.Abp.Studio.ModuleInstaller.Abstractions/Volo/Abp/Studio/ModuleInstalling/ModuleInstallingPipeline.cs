using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.Abp.Studio.ModuleInstalling;

public class ModuleInstallingPipeline
{
    public ModuleInstallingContext Context { get; }

    public List<ModuleInstallingPipelineStep> Steps { get; }

    public ModuleInstallingPipeline(ModuleInstallingContext context)
    {
        Context = context;
        Steps = new List<ModuleInstallingPipelineStep>();
    }

    public async Task ExecuteAsync()
    {
        foreach (var step in Steps)
        {
            await step.ExecuteAsync(Context);
        }
    }

    public void Add(ModuleInstallingPipelineStep step)
    {
        Steps.Add(step);
    }

    public void Remove(Type stepType)
    {
        Steps.RemoveAll(step => step.GetType() == stepType);
    }

    public void Replace(Type stepType, ModuleInstallingPipelineStep step)
    {
        Steps.ReplaceOne(step => step.GetType() == stepType, step);
    }

    public void InsertAfter(Type stepType, ModuleInstallingPipelineStep step)
    {
        Steps.InsertAfter(step => step.GetType() == stepType, step);
    }

    public void InsertBefore(Type stepType, ModuleInstallingPipelineStep step)
    {
        Steps.InsertBefore(step => step.GetType() == stepType, step);
    }
}
