using System;

namespace Volo.Abp;

public class NamedAction<T> : NamedObject
{
    public Action<T> Action { get; set; }
    
    public NamedAction(string name, Action<T> action)
    : base(name)
    {
        Action = Check.NotNull(action, nameof(action));
    }
}