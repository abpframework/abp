using System;
using System.Collections.Generic;
using Volo.Abp.Application.Services;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Modeling
{
    public class ApiDescriptionModelOptions
    {
        public HashSet<Type> IgnoredInterfaces { get; }

        public ApiDescriptionModelOptions()
        {
            IgnoredInterfaces = new HashSet<Type>
            {
                typeof(IApplicationService),
                typeof(IRemoteService),
                typeof(ITransientDependency),
                typeof(ISingletonDependency),
                typeof(IDisposable),
                typeof(IAvoidDuplicateCrossCuttingConcerns)
            };
        }
    }
}
