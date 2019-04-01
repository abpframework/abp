﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Aspects;
using Volo.Abp.AspNetCore.Mvc.Validation;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Mvc
{
    public abstract class AbpController : Controller, IAvoidDuplicateCrossCuttingConcerns
    {
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        public IObjectMapper ObjectMapper { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        public ICurrentUser CurrentUser { get; set; }

        public ICurrentTenant CurrentTenant { get; set; }

        public IAuthorizationService AuthorizationService { get; set; }

        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;

        public IClock Clock { get; set; }

        public IModelStateValidator ModelValidator { get; set; }

        public IFeatureChecker FeatureChecker { get; set; }

        public List<string> AppliedCrossCuttingConcerns { get; } = new List<string>();

        protected virtual void ValidateModel()
        {
            ModelValidator?.Validate(ModelState);
        }

        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);
    }
}
