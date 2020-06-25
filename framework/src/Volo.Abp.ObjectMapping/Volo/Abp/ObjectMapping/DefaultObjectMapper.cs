using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.ObjectMapping
{
    public class DefaultObjectMapper<TContext> : DefaultObjectMapper, IObjectMapper<TContext>
    {
        public DefaultObjectMapper(
            IServiceProvider serviceProvider,
            IAutoObjectMappingProvider<TContext> autoObjectMappingProvider
            ) : base(
                serviceProvider,
                autoObjectMappingProvider)
        {

        }
    }

    public class DefaultObjectMapper : IObjectMapper, ITransientDependency
    {
        public IAutoObjectMappingProvider AutoObjectMappingProvider { get; }
        protected IServiceProvider ServiceProvider { get; }

        public DefaultObjectMapper(
            IServiceProvider serviceProvider,
            IAutoObjectMappingProvider autoObjectMappingProvider)
        {
            AutoObjectMappingProvider = autoObjectMappingProvider;
            ServiceProvider = serviceProvider;
        }

        //TODO: It can be slow to always check if service is available. Test it and optimize if necessary.

        public virtual TDestination Map<TSource, TDestination>(TSource source)
        {
            if (source == null)
            {
                return default;
            }

            using (var scope = ServiceProvider.CreateScope())
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

                    return (TDestination)Activator.CreateInstance(typeof(TDestination), source);
                }
                catch
                {
                    //TODO: Remove catch when TODOs are implemented above
                }
            }

            if (IsTypeSupportInterface(source.GetType(), typeof(ICollection<>)) && IsTypeSupportInterface(typeof(TDestination), typeof(ICollection<>)))
            {
                TDestination destination = (TDestination)Activator.CreateInstance(typeof(TDestination));
                return CollectionMap(source, destination);
            }

            return AutoMap<TSource, TDestination>(source);
        }

        public virtual TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            if (source == null)
            {
                return default;
            }

            using (var scope = ServiceProvider.CreateScope())
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

            if (IsTypeSupportInterface(source.GetType(), typeof(ICollection<>)) && IsTypeSupportInterface(typeof(TDestination), typeof(ICollection<>)))
            {
                return CollectionMap<TSource, TDestination>(source, destination);
            }

            return AutoMap(source, destination);
        }

        protected virtual TDestination AutoMap<TSource, TDestination>(object source)
        {
            return AutoObjectMappingProvider.Map<TSource, TDestination>(source);
        }

        protected virtual TDestination AutoMap<TSource, TDestination>(TSource source, TDestination destination)
        {
            return AutoObjectMappingProvider.Map<TSource, TDestination>(source, destination);
        }

        private TDestination CollectionMap<TSource, TDestination>(TSource source, TDestination destination)
        {
            Type destinationType = typeof(TDestination);
            Type sourceGenericType = source.GetType().GetGenericArguments().Single();
            Type destinationGenericType = destinationType.GetGenericArguments().Single();
            Type mappingDefinationClassType = typeof(IObjectMapper<,>).MakeGenericType(sourceGenericType, destinationGenericType);

            object mapperObject = null;

            using (var scope = ServiceProvider.CreateScope())
            {
                mapperObject = scope.ServiceProvider.GetService(mappingDefinationClassType);

                if (mapperObject == null)
                {
                    // Dependcy evil 1
                    return AutoMap<TSource, TDestination>(source);
                }
            }

            // Dependcy evil 2
            var mapMethodInfo = mappingDefinationClassType.GetMethod("Map",
                                  BindingFlags.Public | BindingFlags.Instance,
                                  null,
                                  CallingConventions.Any,
                                  new Type[] { sourceGenericType },
                                  null);

            var sourceCollection = (IEnumerable<object>)source;

            // Dependcy evil 3
            var addMethodInfo = destinationType.GetMethod("Add",
                              BindingFlags.Public | BindingFlags.Instance,
                              null,
                              CallingConventions.Any,
                              new Type[] { destinationGenericType },
                              null);

            if (addMethodInfo == null)
            {
                return default;
            }

            return ConvertToDestinationCollection(destination, mapperObject, mapMethodInfo, sourceCollection, addMethodInfo);
        }

        private TDestination ConvertToDestinationCollection<TDestination>(TDestination destination, object mapperObject, MethodInfo mapMethodInfo, IEnumerable<object> sourceCollection, MethodInfo addMethodInfo)
        {
            foreach (var item in sourceCollection)
            {
                var mappedObj = mapMethodInfo.Invoke(mapperObject, new object[] { item });
                addMethodInfo.Invoke(destination, new object[] { mappedObj });
            }

            return destination;
        }

        private bool IsTypeSupportInterface(Type type, Type inter)
        {
            if (inter.IsAssignableFrom(type))
            {
                return true;
            }

            if (type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == inter))
            {
                return true;
            }

            return false;
        }
    }
}