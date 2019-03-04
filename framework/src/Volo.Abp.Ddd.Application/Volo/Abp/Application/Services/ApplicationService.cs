using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Aspects;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Volo.Abp.Validation;

namespace Volo.Abp.Application.Services
{
    public abstract class ApplicationService : 
        IApplicationService, 
        IAvoidDuplicateCrossCuttingConcerns,
        IValidationEnabled,
        IUnitOfWorkEnabled,
        IAuthorizationEnabled,
        IAuditingEnabled,
        ITransientDependency
    {
        public static string[] CommonPostfixes { get; set; } = { "AppService", "ApplicationService", "Service" };

        public List<string> AppliedCrossCuttingConcerns { get; } = new List<string>();

        public IUnitOfWorkManager UnitOfWorkManager { get; set; }

        public IObjectMapper ObjectMapper { get; set; }

        public IGuidGenerator GuidGenerator { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        public ICurrentTenant CurrentTenant { get; set; }

        public ICurrentUser CurrentUser { get; set; }

        public ISettingProvider SettingProvider { get; set; }

        public IClock Clock { get; set; }

        public IAuthorizationService AuthorizationService { get; set; }

        public IFeatureChecker FeatureChecker { get; set; }

        protected IUnitOfWork CurrentUnitOfWork => UnitOfWorkManager?.Current;

        protected ILogger Logger => _lazyLogger.Value;
        private Lazy<ILogger> _lazyLogger => new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName) ?? NullLogger.Instance, true);

        protected ApplicationService()
        {
            GuidGenerator = SimpleGuidGenerator.Instance;
        }

        /// <summary>
        /// Checks for given <paramref name="policyName"/>.
        /// Throws <see cref="AbpAuthorizationException"/> if given policy has not been granted.
        /// </summary>
        /// <param name="policyName">The policy name. This method does nothing if given <paramref name="policyName"/> is null or empty.</param>
        protected virtual async Task CheckPolicyAsync([CanBeNull] string policyName)
        {
            if (string.IsNullOrEmpty(policyName))
            {
                return;
            }

            await AuthorizationService.CheckAsync(policyName);
        }

        /// <summary>
        /// Checks for given <paramref name="policyName"/>.
        /// Throws <see cref="AbpAuthorizationException"/> if given policy has not been granted.
        /// </summary>
        /// <param name="policyName">The policy name. This method does nothing if given <paramref name="policyName"/> is null or empty.</param>
        protected virtual void CheckPolicy([CanBeNull] string policyName)
        {
            if (string.IsNullOrEmpty(policyName))
            {
                return;
            }

            AuthorizationService.Check(policyName);
        }
    }
}