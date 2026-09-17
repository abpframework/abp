using System;
using Microsoft.SemanticKernel;

namespace Volo.Abp.AI;

public class KernelConfiguration
{
    public IKernelBuilder? Builder { get; set; }

    public KernelBuilderConfigurerList BuilderConfigurers { get; } = new();

    public void ConfigureBuilder(Action<IKernelBuilder> configureAction)
    {
        BuilderConfigurers.Add(configureAction);
    }

    public void ConfigureBuilder(string name, Action<IKernelBuilder> configureAction)
    {
        BuilderConfigurers.Add(name, configureAction);
    }
}