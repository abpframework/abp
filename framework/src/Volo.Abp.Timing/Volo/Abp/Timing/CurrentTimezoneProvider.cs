using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Timing;

public class CurrentTimezoneProvider : ICurrentTimezoneProvider, ISingletonDependency
{
    public string? TimeZone
    {
        get => _currentScope.Value;
        set => _currentScope.Value = value;
    }

    private readonly AsyncLocal<string?> _currentScope;

    public CurrentTimezoneProvider()
    {
        _currentScope = new AsyncLocal<string?>();
    }
}
