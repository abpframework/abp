using System.Threading;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Threading
{
    [Dependency(ReplaceServices = true)]
    public class HttpContextCancellationTokenProvider : CancellationTokenProviderBase, ITransientDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public override CancellationToken Token
        {
            get
            {
                if (OverrideValue != null)
                {
                    return OverrideValue.CancellationToken;
                }
                return _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;
            }
        }

        public HttpContextCancellationTokenProvider(
            IAmbientScopeProvider<CancellationTokenOverride> cancellationTokenOverrideScopeProvider,
            IHttpContextAccessor httpContextAccessor)
            : base(cancellationTokenOverrideScopeProvider)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
