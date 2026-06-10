using System;
using Microsoft.Extensions.AI;

namespace Volo.Abp.AI;

public class ChatClientConfiguration
{    
    public ChatClientBuilder? Builder { get; set; }
    
    public BuilderConfigurerList BuilderConfigurers { get; } = new();
    
    public void ConfigureBuilder(Action<ChatClientBuilder> configureAction)
    {
        BuilderConfigurers.Add(configureAction);
    }
    
    public void ConfigureBuilder(string name, Action<ChatClientBuilder> configureAction)
    {
        BuilderConfigurers.Add(name, configureAction);
    }
}