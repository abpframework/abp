using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Volo.Abp.IdentityServer.Cors
{

    /// <summary>
    /// Implementation of ICorsPolicyService that consults the client configuration in the database for allowed CORS origins.
    /// </summary>
    /// <seealso cref="IdentityServer4.Services.ICorsPolicyService" />
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<CorsPolicyService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsPolicyService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">context</exception>
        public CorsPolicyService(IHttpContextAccessor context, ILogger<CorsPolicyService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }


        /// <summary>
        /// Determines whether origin is allowed.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <returns></returns>
        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            // doing this here and not in the ctor because: https://github.com/aspnet/CORS/issues/105
            var clientCorsOriginRepository = _context.HttpContext.RequestServices
                .GetRequiredService<IClientCorsOriginRepository>();

            var origins =await clientCorsOriginRepository.GetAllClientCorsOrigins();

            var distinctOrigins = origins.Where(x => x != null).Distinct();

            var isAllowed = distinctOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);

            _logger.LogDebug("Origin {origin} is allowed: {originAllowed}", origin, isAllowed);

            return isAllowed;
        }
    }
}
