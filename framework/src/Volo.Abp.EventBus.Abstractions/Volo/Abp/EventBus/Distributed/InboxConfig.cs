using System;
using JetBrains.Annotations;

namespace Volo.Abp.EventBus.Distributed;

public class InboxConfig
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

    public Func<Type, bool>? EventSelector { get; set; }

    public Func<Type, bool>? HandlerSelector { get; set; }

    /// <summary>
    /// Used to enable/disable processing incoming events.
    /// Default: true.
    /// </summary>
    public bool IsProcessingEnabled { get; set; } = true;

    public InboxConfig([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
    }
}
