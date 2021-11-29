using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;

namespace Volo.Abp.AspNetCore.SignalR
{
    public class HubConfig
    {
        [NotNull]
        public Type HubType { get; }

        [NotNull]
        public string RoutePattern { get; set; }

        [NotNull]
        public List<Action<HttpConnectionDispatcherOptions>> ConfigureActions { get; set; }

        public HubConfig(
            [NotNull] Type hubType,
            [NotNull] string routePattern,
            [CanBeNull] Action<HttpConnectionDispatcherOptions> configureAction = null)
        {
            HubType = Check.NotNull(hubType, nameof(hubType));
            RoutePattern = Check.NotNullOrWhiteSpace(routePattern, nameof(routePattern));
            ConfigureActions = new List<Action<HttpConnectionDispatcherOptions>>();

            if (configureAction != null)
            {
                ConfigureActions.Add(configureAction);
            }
        }

        public static HubConfig Create<THub>()
            where THub : Hub
        {
            return Create(typeof(THub));
        }

        public static HubConfig Create(Type hubType)
        {
            return new HubConfig(
                hubType,
                HubRouteAttribute.GetRoutePattern(hubType)
            );
        }
    }
}