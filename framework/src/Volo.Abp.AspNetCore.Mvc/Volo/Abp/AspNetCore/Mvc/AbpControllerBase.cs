using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Aspects;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc;

public abstract class AbpControllerBase : ControllerBase, IAvoidDuplicateCrossCuttingConcerns
{
    public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

    protected IUnitOfWorkManager UnitOfWorkManager => LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>();

    protected Type ObjectMapperContext { get; set; }
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetService<IObjectMapper>(provider =>
        ObjectMapperContext == null
            ? provider.GetRequiredService<IObjectMapper>()
            : (IObjectMapper)provider.GetRequiredService(typeof(IObjectMapper<>).MakeGenericType(ObjectMapperContext)));

    protected IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

    protected ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance);

    protected ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();

    protected ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    protected IAuthorizationService AuthorizationService => LazyServiceProvider.LazyGetRequiredService<IAuthorizationService>();

    protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;

    protected IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();

    protected IModelStateValidator ModelValidator => LazyServiceProvider.LazyGetRequiredService<IModelStateValidator>();

    protected IFeatureChecker FeatureChecker => LazyServiceProvider.LazyGetRequiredService<IFeatureChecker>();

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
    private IStringLocalizer _localizer;

    protected Type LocalizationResource
    {
        get => _localizationResource;
        set
        {
            _localizationResource = value;
            _localizer = null;
        }
    }
    private Type _localizationResource = typeof(DefaultResource);

    public List<string> AppliedCrossCuttingConcerns { get; } = new List<string>();

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

    protected virtual void ValidateModel()
    {
        ModelValidator?.Validate(ModelState);
    }
}
