using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.ClientProxying
{
    public class ClientProxyRequestContext
    {
        [NotNull]
        public ActionApiDescriptionModel Action { get; }

        [NotNull]
        public IReadOnlyDictionary<string, object> Arguments { get; }

        [NotNull]
        public Type ServiceType { get; }

        public ClientProxyRequestContext(
            [NotNull] ActionApiDescriptionModel action,
            [NotNull] IReadOnlyDictionary<string, object> arguments,
            [NotNull] Type serviceType)
        {
            ServiceType = serviceType;
            Action = Check.NotNull(action, nameof(action));
            Arguments = Check.NotNull(arguments, nameof(arguments));
            ServiceType = Check.NotNull(serviceType, nameof(serviceType));
        }
    }
}
