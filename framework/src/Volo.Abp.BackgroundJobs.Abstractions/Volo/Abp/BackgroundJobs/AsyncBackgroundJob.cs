using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Volo.Abp.BackgroundJobs;

public abstract class AsyncBackgroundJob<TArgs> : IAsyncBackgroundJob<TArgs>
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; } = default!;

    protected IGuidGenerator GuidGenerator => LazyServiceProvider.GetRequiredService<IGuidGenerator>();
    protected ICurrentTenant CurrentTenant => LazyServiceProvider.GetRequiredService<ICurrentTenant>();
    protected IClock Clock => LazyServiceProvider.GetRequiredService<IClock>();
    protected IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    protected IUnitOfWork? CurrentUnitOfWork => UnitOfWorkManager?.Current;
    protected ILoggerFactory LoggerFactory => LazyServiceProvider.GetRequiredService<ILoggerFactory>();
    protected ILogger Logger => LazyServiceProvider.GetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance);
    protected ICancellationTokenProvider CancellationTokenProvider => LazyServiceProvider.GetRequiredService<ICancellationTokenProvider>();

    protected IStringLocalizerFactory StringLocalizerFactory => LazyServiceProvider.LazyGetRequiredService<IStringLocalizerFactory>();

    protected IStringLocalizer L
    {
        get
        {
            if (_localizer == null)
            {
                _localizer = CreateLocalizer();
            }

            return _localizer;
        }
    }
    private IStringLocalizer? _localizer;

    protected Type? LocalizationResource
    {
        get => _localizationResource;
        set
        {
            _localizationResource = value;
            _localizer = null;
        }
    }
    private Type? _localizationResource = typeof(DefaultResource);

    protected virtual IStringLocalizer CreateLocalizer()
    {
        if (LocalizationResource != null)
        {
            return StringLocalizerFactory.Create(LocalizationResource);
        }

        var localizer = StringLocalizerFactory.CreateDefaultOrNull();
        if (localizer == null)
        {
            throw new AbpException($"Set {nameof(LocalizationResource)} or define the default localization resource type (by configuring the {nameof(AbpLocalizationOptions)}.{nameof(AbpLocalizationOptions.DefaultResourceType)}) to be able to use the {nameof(L)} object!");
        }

        return localizer;
    }

    public abstract Task ExecuteAsync(TArgs args);
}
