using Microsoft.AspNetCore.SignalR;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.SignalR.SampleHubs;

public abstract class RegularHubBase<THub> : Hub<THub> where THub : class
{

}

[DisableConventionalRegistration]
[ExposeServices(typeof(RegularHubClass1))]
public class RegularHubClass1 : RegularHubBase<RegularHubClass1>
{

}

[DisableConventionalRegistration]
[ExposeServices(typeof(RegularHubClass12), typeof(RegularHubClass1))]
public class RegularHubClass12 : RegularHubClass1
{

}

[DisableConventionalRegistration]
[ExposeServices(typeof(RegularHubClass2))]
public class RegularHubClass2 : RegularHubBase<RegularHubClass2>
{

}

[DisableConventionalRegistration]
[ExposeServices(typeof(RegularHubClass22), typeof(RegularHubClass2))]
public class RegularHubClass22 : RegularHubClass2
{

}
