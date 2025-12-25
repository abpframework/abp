using Microsoft.Extensions.Options;

namespace Volo.Abp.Options;

/// <summary>
/// This Options manager is similar to Microsoft UnnamedOptionsManager but without the locking mechanism.
/// Prevent deadlocks when accessing options in multiple threads.
/// </summary>
/// <typeparam name="TOptions"></typeparam>
public class AbpUnnamedOptionsManager<TOptions> : IOptions<TOptions>
    where TOptions : class
{
    private readonly IOptionsFactory<TOptions> _factory;
    private TOptions? _value;

    public AbpUnnamedOptionsManager(IOptionsFactory<TOptions> factory)
    {
        _factory = factory;
    }

    public TOptions Value
    {
        get
        {
            if (_value is { } value)
            {
                return value;
            }

            _value = _factory.Create(Microsoft.Extensions.Options.Options.DefaultName);
            return _value;
        }
    }
}
