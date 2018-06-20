using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Authorization
{
    [Dependency(ReplaceServices = true)]
    public class AbpAuthorizationService : DefaultAuthorizationService, IAbpAuthorizationService, ITransientDependency
    {
        public IServiceProvider ServiceProvider { get; }

        public ClaimsPrincipal CurrentPrincipal => _currentPrincipalAccessor.Principal;

        private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

        public AbpAuthorizationService(
            IAuthorizationPolicyProvider policyProvider, 
            IAuthorizationHandlerProvider handlers, 
            ILogger<DefaultAuthorizationService> logger, 
            IAuthorizationHandlerContextFactory contextFactory, 
            IAuthorizationEvaluator evaluator, 
            IOptions<AuthorizationOptions> options,
            ICurrentPrincipalAccessor currentPrincipalAccessor, 
            IServiceProvider serviceProvider)
            : base(
                policyProvider,
                handlers, 
                logger, 
                contextFactory, 
                evaluator, 
                options)
        {
            _currentPrincipalAccessor = currentPrincipalAccessor;
            ServiceProvider = serviceProvider;
        }
    }
}