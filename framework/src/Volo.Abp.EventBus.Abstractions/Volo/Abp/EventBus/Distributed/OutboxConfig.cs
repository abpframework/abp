using System;
using JetBrains.Annotations;

namespace Volo.Abp.EventBus.Distributed;

public class OutboxConfig
{
    [NotNull]
    public string Name { get; }
    
    [NotNull]
    public string DatabaseName {
        get => _databaseName;
        set => _databaseName = Check.NotNullOrWhiteSpace(value, nameof(DatabaseName));
    }
    [NotNull] private string _databaseName = default!;

    public Type ImplementationType { get; set; } = default!;

    public Func<Type, bool>? Selector { get; set; }

    /// <summary>
    /// Used to enable/disable sending events from outbox to the message broker.
    /// Default: true.
    /// </summary>
    public bool IsSendingEnabled { get; set; } = true;

    public OutboxConfig([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
    }
}
