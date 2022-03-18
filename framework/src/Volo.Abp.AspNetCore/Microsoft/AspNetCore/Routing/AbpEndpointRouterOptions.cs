using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Routing;

public class AbpEndpointRouterOptions
{
    public List<Action<EndpointRouteBuilderContext>> EndpointConfigureActions { get; }

    public AbpEndpointRouterOptions()
    {
        EndpointConfigureActions = new List<Action<EndpointRouteBuilderContext>>();
    }
}
