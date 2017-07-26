using System;
using Volo.DependencyInjection;

namespace Volo.Abp.ObjectMapping
{
    public sealed class NotImplementedObjectMapper : IObjectMapper, ISingletonDependency
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NotImplementedObjectMapper Instance { get; } = new NotImplementedObjectMapper();

        public TDestination Map<TDestination>(object source)
        {
            throw new NotImplementedException("Abp.ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new NotImplementedException("Abp.ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }
    }
}