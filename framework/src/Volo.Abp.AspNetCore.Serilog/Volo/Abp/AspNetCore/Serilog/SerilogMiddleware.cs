using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Serilog.Core;
using Serilog.Core.Enrichers;
using Volo.Abp.Clients;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Tracing;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Serilog
{
    public class SerilogMiddleware : IMiddleware, ITransientDependency
    {
        private const string TenantEnricherPropertyName = "TenantId";
        private const string UserEnricherPropertyName = "UserId";
        private const string ClientEnricherPropertyName = "ClientId";
        private const string CorrelationIdPropertyName = "CorrelationId";

        private readonly ICurrentClient _currentClient;
        private readonly ICurrentTenant _currentTenant;
        private readonly ICurrentUser _currentUser;
        private readonly ICorrelationIdProvider _correlationIdProvider;

        public SerilogMiddleware(
            ICurrentTenant currentTenant,
            ICurrentUser currentUser,
            ICurrentClient currentClient,
            ICorrelationIdProvider correlationIdProvider)
        {
            _currentTenant = currentTenant;
            _currentUser = currentUser;
            _currentClient = currentClient;
            _correlationIdProvider = correlationIdProvider;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var enrichers = new List<ILogEventEnricher>();

            if (_currentTenant?.Id != null)
            {
                enrichers.Add(new PropertyEnricher(TenantEnricherPropertyName, _currentTenant.Id));
            }

            if (_currentUser?.Id != null)
            {
                enrichers.Add(new PropertyEnricher(UserEnricherPropertyName, _currentUser.Id));
            }

            if (_currentClient?.Id != null)
            {
                enrichers.Add(new PropertyEnricher(ClientEnricherPropertyName, _currentClient.Id));
            }

            var correlationId = _correlationIdProvider.Get();
            if (!string.IsNullOrEmpty(correlationId))
            {
                enrichers.Add(new PropertyEnricher(CorrelationIdPropertyName, correlationId));
            }

            using (LogContext.Push(enrichers.ToArray()))
            {
                await next(context).ConfigureAwait(false);
            }
        }
    }
}