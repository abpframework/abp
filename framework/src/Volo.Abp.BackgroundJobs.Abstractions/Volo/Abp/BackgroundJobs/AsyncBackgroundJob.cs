using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

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
