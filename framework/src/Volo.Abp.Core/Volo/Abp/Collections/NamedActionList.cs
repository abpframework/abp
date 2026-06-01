using System;

namespace Volo.Abp.AI;

public class NamedActionList<T> : NamedObjectList<NamedAction<T>>
{
    public void Add(Action<T> action)
    {
        this.Add(Guid.NewGuid().ToString("N"), action);
    }
    
    public void Add(string name, Action<T> action)
        => this.Add(new NamedAction<T>(name, action));
}