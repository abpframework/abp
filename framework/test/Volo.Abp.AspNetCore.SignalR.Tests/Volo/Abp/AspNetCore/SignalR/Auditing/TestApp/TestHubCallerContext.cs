using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;

namespace Volo.Abp.AspNetCore.SignalR.Auditing.TestApp;

public class TestHubCallerContext : HubCallerContext
{
    public override string ConnectionId => "test-connection";

    public override string UserIdentifier => null;

    public override ClaimsPrincipal User => null;

    public override IDictionary<object, object> Items { get; } = new Dictionary<object, object>();

    public override IFeatureCollection Features { get; } = new FeatureCollection();

    public override CancellationToken ConnectionAborted => CancellationToken.None;

    public override void Abort()
    {
    }
}
