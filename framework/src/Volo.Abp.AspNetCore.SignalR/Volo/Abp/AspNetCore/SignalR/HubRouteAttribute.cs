using System;
using System.Reflection;
using Microsoft.AspNetCore.SignalR;

namespace Volo.Abp.AspNetCore.SignalR
{
    public class HubRouteAttribute : Attribute
    {
        public string RoutePattern { get; set; }

        public HubRouteAttribute(string routePattern)
        {
            RoutePattern = routePattern;
        }

        public virtual string GetRoutePatternForType(Type hubType)
        {
            return RoutePattern;
        }

        public static string GetRoutePattern<THub>()
            where THub : Hub
        {
            return GetRoutePattern(typeof(THub));
        }

        public static string GetRoutePattern(Type hubType)
        {
            var routeAttribute = hubType.GetSingleAttributeOrNull<HubRouteAttribute>();
            if (routeAttribute != null)
            {
                return routeAttribute.GetRoutePatternForType(hubType);
            }

            return "/signalr-hubs/" + hubType.Name.RemovePostFix("Hub").ToKebabCase();
        }
    }
}