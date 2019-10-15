using System;
using System.Collections.Generic;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Http.Modeling
{
    public class AbpApiDescriptionModelOptions
    {
        public HashSet<Type> IgnoredInterfaces { get; }

        public AbpApiDescriptionModelOptions()
        {
            IgnoredInterfaces = new HashSet<Type>
            {
                typeof(ITransientDependency),
                typeof(ISingletonDependency),
                typeof(IDisposable),
                typeof(IAvoidDuplicateCrossCuttingConcerns)
            };
        }
    }
}
