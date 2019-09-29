using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ObjectMapping
{
    //TODO: It can be slow to always check if service is available. Test it and optimize if necessary.

    public class DefaultObjectMapper : IObjectMapper, ITransientDependency
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
                return default;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var specificMapper = scope.ServiceProvider.GetService<IObjectMapper<TSource, TDestination>>();
                if (specificMapper != null)
                {
                    return specificMapper.Map(source);
                }
            }

            if (source is IMapTo<TDestination> mapperSource)
            {
                return mapperSource.MapTo();
            }

            if (typeof(IMapFrom<TSource>).IsAssignableFrom(typeof(TDestination)))
            {
                try
                {
                    //TODO: Check if TDestination has a proper constructor which takes TSource
                    //TODO: Check if TDestination has an empty constructor (in this case, use MapFrom)

                    return (TDestination) Activator.CreateInstance(typeof(TDestination), source);
                }
                catch
                {
                    //TODO: Remove catch when TODOs are implemented above
                }
            }

            return AutoMap<TSource, TDestination>(source);
        }

        public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (source == null)
            {
                return default;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var specificMapper = scope.ServiceProvider.GetService<IObjectMapper<TSource, TDestination>>();
                if (specificMapper != null)
                {
                    return specificMapper.Map(source, destination);
                }
            }

            if (source is IMapTo<TDestination> mapperSource)
            {
                mapperSource.MapTo(destination);
                return destination;
            }

            if (destination is IMapFrom<TSource> mapperDestination)
            {
                mapperDestination.MapFrom(source);
                return destination;
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