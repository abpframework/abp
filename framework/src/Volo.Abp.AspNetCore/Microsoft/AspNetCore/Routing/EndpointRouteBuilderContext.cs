using System;

namespace Microsoft.AspNetCore.Routing
{
    public class EndpointRouteBuilderContext
    {
        public IEndpointRouteBuilder Endpoints { get; }
        
        public IServiceProvider ScopeServiceProvider { get; }

        public EndpointRouteBuilderContext(
            IEndpointRouteBuilder endpoints, 
            IServiceProvider scopeServiceProvider)
        {
            Endpoints = endpoints;
            ScopeServiceProvider = scopeServiceProvider;
        }
    }
}