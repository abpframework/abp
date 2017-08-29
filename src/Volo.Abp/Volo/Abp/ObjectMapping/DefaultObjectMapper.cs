using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ObjectMapping
{
    //TODO: It can be slow to always check if service is available. Test it and optimize if necessary.

    public class DefaultObjectMapper : IObjectMapper, ISingletonDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultObjectMapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual TDestination Map<TSource, TDestination>(TSource source)
        {
            if (source == null)
            {
                return default(TDestination);
            }

            //Check if a specific mapper is registered
            using (var scope = _serviceProvider.CreateScope())
            {
                var specificMapper = scope.ServiceProvider.GetService<IObjectMapper<TSource, TDestination>>();
                if (specificMapper != null)
                {
                    return specificMapper.Map(source);
                }
            }

            return AutoMap<TSource, TDestination>(source);
        }

        public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (source == null)
            {
                return default(TDestination);
            }

            //Check if a specific mapper is registered
            using (var scope = _serviceProvider.CreateScope())
            {
                var specificMapper = scope.ServiceProvider.GetService<IObjectMapper<TSource, TDestination>>();
                if (specificMapper != null)
                {
                    return specificMapper.Map(source, destination);
                }
            }

            return AutoMap(source, destination);
        }

        protected virtual TDestination AutoMap<TSource, TDestination>(object source)
        {
            throw new NotImplementedException($"Can not map from given object ({source}) to {typeof(TDestination).AssemblyQualifiedName}.");
        }

        protected virtual TDestination AutoMap<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new NotImplementedException($"Can no map from {typeof(TSource).AssemblyQualifiedName} to {typeof(TDestination).AssemblyQualifiedName}.");
        }
    }
}